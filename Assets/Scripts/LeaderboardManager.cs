using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages leaderboard operations: submit scores, fetch top scores
/// Includes anti-cheat validation
/// </summary>
public class LeaderboardManager : MonoBehaviour
{
    private static LeaderboardManager instance;
    public static LeaderboardManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("LeaderboardManager");
                instance = go.AddComponent<LeaderboardManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private const string LEADERBOARD_PATH = "leaderboard";

    /// <summary>
    /// Event triggered when leaderboard data is loaded
    /// </summary>
    public event Action<List<LeaderboardEntry>> OnLeaderboardLoaded;

    /// <summary>
    /// Event triggered when score submission completes
    /// </summary>
    public event Action<bool, string> OnScoreSubmitted;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Submits a score to the leaderboard with anti-cheat validation
    /// </summary>
    /// <param name="username">Player username</param>
    /// <param name="score">Score to submit</param>
    public void SubmitScore(string username, int score)
    {
        // Validate Firebase ready
        if (!FirebaseManager.Instance.ValidateFirebaseReady())
        {
            OnScoreSubmitted?.Invoke(false, "No internet connection");
            Debug.LogWarning("Cannot submit score: Firebase not ready or no internet");
            return;
        }

        // Anti-cheat validation
        if (score > GameConstants.MAX_VALID_SCORE)
        {
            Debug.LogError($"Score rejected: {score} exceeds maximum valid score {GameConstants.MAX_VALID_SCORE}");
            OnScoreSubmitted?.Invoke(false, "Invalid score");
            return;
        }

        if (score < 0)
        {
            Debug.LogError("Score rejected: negative value");
            OnScoreSubmitted?.Invoke(false, "Invalid score");
            return;
        }

        // Sanitize username
        if (string.IsNullOrEmpty(username))
        {
            username = GameConstants.DEFAULT_USERNAME;
        }

        // Create entry
        LeaderboardEntry entry = new LeaderboardEntry(username, score);

        // Generate unique key for this entry
        string entryKey = FirebaseManager.Instance.Database
            .Child(LEADERBOARD_PATH)
            .Push().Key;

        // Create JSON data
        Dictionary<string, object> entryData = new Dictionary<string, object>
        {
            { "username", entry.username },
            { "score", entry.score },
            { "timestamp", entry.timestamp }
        };

        // Submit to Firebase
        FirebaseManager.Instance.Database
            .Child(LEADERBOARD_PATH)
            .Child(entryKey)
            .SetValueAsync(entryData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Debug.Log($"Score submitted successfully: {username} - {score}");
                    OnScoreSubmitted?.Invoke(true, "Score submitted!");
                }
                else
                {
                    Debug.LogError($"Failed to submit score: {task.Exception}");
                    OnScoreSubmitted?.Invoke(false, "Submission failed");
                }
            });
    }

    /// <summary>
    /// Fetches the top N scores from the leaderboard
    /// </summary>
    /// <param name="count">Number of top scores to fetch (default 10)</param>
    public void FetchTopScores(int count = GameConstants.LEADERBOARD_TOP_COUNT)
    {
        // Validate Firebase ready
        if (!FirebaseManager.Instance.ValidateFirebaseReady())
        {
            Debug.LogWarning("Cannot fetch leaderboard: Firebase not ready or no internet");
            OnLeaderboardLoaded?.Invoke(new List<LeaderboardEntry>());
            return;
        }

        // Query top scores ordered by score
        FirebaseManager.Instance.Database
            .Child(LEADERBOARD_PATH)
            .OrderByChild("score")
            .LimitToLast(count * 2) // Fetch more than needed, then filter
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Failed to fetch leaderboard: {task.Exception}");
                    OnLeaderboardLoaded?.Invoke(new List<LeaderboardEntry>());
                    return;
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        try
                        {
                            string username = childSnapshot.Child("username").Value?.ToString() ?? GameConstants.DEFAULT_USERNAME;
                            int score = Convert.ToInt32(childSnapshot.Child("score").Value ?? 0);
                            long timestamp = Convert.ToInt64(childSnapshot.Child("timestamp").Value ?? 0);

                            // Anti-cheat filter
                            if (score > 0 && score <= GameConstants.MAX_VALID_SCORE)
                            {
                                LeaderboardEntry entry = new LeaderboardEntry
                                {
                                    username = username,
                                    score = score,
                                    timestamp = timestamp
                                };
                                entries.Add(entry);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning($"Failed to parse leaderboard entry: {e.Message}");
                        }
                    }

                    // Sort by score descending, then by timestamp ascending (earlier = higher rank)
                    entries = entries
                        .OrderByDescending(e => e.score)
                        .ThenBy(e => e.timestamp)
                        .Take(count)
                        .ToList();

                    Debug.Log($"Fetched {entries.Count} leaderboard entries");
                    OnLeaderboardLoaded?.Invoke(entries);
                }
            });
    }

    /// <summary>
    /// Clears all leaderboard data (use for testing only!)
    /// </summary>
    public void ClearLeaderboard()
    {
        if (!FirebaseManager.Instance.ValidateFirebaseReady())
        {
            Debug.LogWarning("Cannot clear leaderboard: Firebase not ready");
            return;
        }

        FirebaseManager.Instance.Database
            .Child(LEADERBOARD_PATH)
            .RemoveValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Debug.Log("Leaderboard cleared successfully");
                }
                else
                {
                    Debug.LogError($"Failed to clear leaderboard: {task.Exception}");
                }
            });
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        // Clean up singleton when application quits (including exiting Play mode in editor)
        if (instance == this)
        {
            Destroy(gameObject);
        }
    }
}
