using UnityEngine;

/// <summary>
/// Singleton ScoreManager - Tum oyun boyunca skoru yonetir
/// Sahneler arasi gecislerde korunur (DontDestroyOnLoad)
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // Singleton instance
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Sahnede ScoreManager var mi kontrol et
                _instance = FindFirstObjectByType<ScoreManager>();

                // Yoksa yeni bir GameObject olustur
                if (_instance == null)
                {
                    GameObject scoreManagerObject = new GameObject("ScoreManager");
                    _instance = scoreManagerObject.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    // Guncel skor
    private int currentScore = 0;

    // PlayerPrefs key
    private const string SCORE_KEY = "GameScore";

    void Awake()
    {
        // Singleton kontrolu - birden fazla instance varsa yok et
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // Sahneler arasi korunur

        // Kaydedilmis skoru yukle
        LoadScore();

        Debug.Log($"[ScoreManager] Initialized with score: {currentScore}");
    }

    /// <summary>
    /// Skora puan ekler
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        SaveScore();
        Debug.Log($"[ScoreManager] Added {points} points. Total score: {currentScore}");
    }

    /// <summary>
    /// Guncel skoru dondurur
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Skoru 6 haneli string olarak dondurur (ornek: 000200)
    /// </summary>
    public string GetScoreFormatted()
    {
        return currentScore.ToString("D6");
    }

    /// <summary>
    /// Skoru sifirlar (yeni oyun baslatildiginda)
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        SaveScore();
        Debug.Log("[ScoreManager] Score reset to 0");
    }

    /// <summary>
    /// Skoru PlayerPrefs'e kaydeder
    /// </summary>
    private void SaveScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Skoru PlayerPrefs'ten yukler
    /// </summary>
    private void LoadScore()
    {
        currentScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    /// <summary>
    /// Kalan sureye gore skor hesaplar ve ekler
    /// Formul: Kalan Saniye x 10
    /// </summary>
    public int CalculateAndAddTimeBonus(float remainingTimeInSeconds)
    {
        // Kalan sureyi tam sayiya cevir ve 10 ile carp
        int timeBonus = Mathf.FloorToInt(remainingTimeInSeconds) * 10;

        if (remainingTimeInSeconds <= 0)
        {
            Debug.LogWarning($"[ScoreManager] *** WARNING: No time remaining! ({remainingTimeInSeconds:F2}s) - No points awarded! ***");
        }

        AddScore(timeBonus);

        Debug.Log($"[ScoreManager] Time bonus: {remainingTimeInSeconds:F2}s remaining = {timeBonus} points");
        Debug.Log($"[ScoreManager] Calculation: Floor({remainingTimeInSeconds:F2}) × 10 = {Mathf.FloorToInt(remainingTimeInSeconds)} × 10 = {timeBonus}");

        return timeBonus;
    }
}
