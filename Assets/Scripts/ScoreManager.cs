using UnityEngine;

/// <summary>
/// Singleton ScoreManager - Manages score throughout the entire game
/// Persists across scene transitions (DontDestroyOnLoad)
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    /// <summary>
    /// Gets the singleton instance of ScoreManager
    /// </summary>
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ScoreManager>();

                if (_instance == null)
                {
                    GameObject scoreManagerObject = new GameObject("ScoreManager");
                    _instance = scoreManagerObject.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    private int currentScore = 0;
    private const string SCORE_KEY = "GameScore";

    void Awake()
    {
        // Singleton pattern - destroy duplicates
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    /// <summary>
    /// Adds points to the current score
    /// </summary>
    /// <param name="points">Points to add</param>
    public void AddScore(int points)
    {
        currentScore += points;
        SaveScore();
    }

    /// <summary>
    /// Gets the current score
    /// </summary>
    /// <returns>Current score value</returns>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Gets the score formatted as 6-digit string (e.g., 000200)
    /// </summary>
    /// <returns>Formatted score string</returns>
    public string GetScoreFormatted()
    {
        return currentScore.ToString($"D{GameConstants.SCORE_DISPLAY_DIGITS}");
    }

    /// <summary>
    /// Resets the score to zero (called when starting new game or after jumpscare)
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        SaveScore();
    }

    /// <summary>
    /// Saves the score to PlayerPrefs
    /// </summary>
    private void SaveScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the score from PlayerPrefs
    /// </summary>
    private void LoadScore()
    {
        currentScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    /// <summary>
    /// Calculates and adds time bonus based on remaining time
    /// Formula: Floor(remaining seconds) × 10
    /// </summary>
    /// <param name="remainingTimeInSeconds">Remaining time in seconds</param>
    /// <returns>Time bonus points awarded</returns>
    public int CalculateAndAddTimeBonus(float remainingTimeInSeconds)
    {
        int timeBonus = Mathf.FloorToInt(remainingTimeInSeconds) * GameConstants.TIME_BONUS_MULTIPLIER;

        if (remainingTimeInSeconds <= 0)
        {
            Debug.LogWarning("[ScoreManager] No time remaining - no points awarded!");
        }

        AddScore(timeBonus);
        return timeBonus;
    }
}
