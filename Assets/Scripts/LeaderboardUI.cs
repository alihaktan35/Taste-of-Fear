using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Controls the leaderboard UI scene/panel
/// Fetches and displays top scores
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform entriesContainer;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject loadingIndicator;

    private List<GameObject> spawnedEntries = new List<GameObject>();

    private void Start()
    {
        // Subscribe to leaderboard events
        LeaderboardManager.Instance.OnLeaderboardLoaded += OnLeaderboardDataLoaded;

        // Setup refresh button
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshLeaderboard);
        }

        // Auto-load on start
        RefreshLeaderboard();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.OnLeaderboardLoaded -= OnLeaderboardDataLoaded;
        }

        if (refreshButton != null)
        {
            refreshButton.onClick.RemoveListener(RefreshLeaderboard);
        }
    }

    /// <summary>
    /// Refreshes the leaderboard by fetching latest data
    /// </summary>
    public void RefreshLeaderboard()
    {
        SetLoadingState(true);
        SetStatusText("Loading leaderboard...");

        // Check internet connection first
        if (!FirebaseManager.Instance.HasInternetConnection())
        {
            SetLoadingState(false);
            SetStatusText("No internet connection");
            return;
        }

        // Fetch top scores
        LeaderboardManager.Instance.FetchTopScores();
    }

    /// <summary>
    /// Called when leaderboard data is loaded from Firebase
    /// </summary>
    /// <param name="entries">List of leaderboard entries</param>
    private void OnLeaderboardDataLoaded(List<LeaderboardEntry> entries)
    {
        SetLoadingState(false);

        // Clear existing entries
        ClearEntries();

        // Check if empty
        if (entries == null || entries.Count == 0)
        {
            SetStatusText("No scores yet. Be the first!");
            return;
        }

        SetStatusText($"Top {entries.Count} Players");

        // Spawn entry UI for each
        string currentUsername = UsernameManager.Instance.GetUsername();

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject entryObj = Instantiate(entryPrefab, entriesContainer);
            LeaderboardEntryUI entryUI = entryObj.GetComponent<LeaderboardEntryUI>();

            if (entryUI != null)
            {
                entryUI.SetData(i + 1, entries[i]);

                // Highlight if this is the current player
                if (entries[i].username == currentUsername)
                {
                    entryUI.SetHighlight(true);
                }
            }

            spawnedEntries.Add(entryObj);
        }
    }

    /// <summary>
    /// Clears all spawned entry objects
    /// </summary>
    private void ClearEntries()
    {
        foreach (GameObject entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();
    }

    /// <summary>
    /// Sets the loading indicator visibility
    /// </summary>
    /// <param name="loading">True if loading</param>
    private void SetLoadingState(bool loading)
    {
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(loading);
        }

        if (refreshButton != null)
        {
            refreshButton.interactable = !loading;
        }
    }

    /// <summary>
    /// Updates the status text
    /// </summary>
    /// <param name="message">Message to display</param>
    private void SetStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
