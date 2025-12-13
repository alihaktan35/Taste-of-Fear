using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// table01 sahnesindeki skor UI'sini gunceller
/// scoreSection icindeki Text componentini ScoreManager'dan alinan degerle gunceller
/// </summary>
public class ScoreUIManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Skor sayisini gosteren Text componenti (scoreSection icindeki deger)")]
    public Text scoreText;

    [Header("Update Settings")]
    [Tooltip("Skoru her frame'de guncellemek ister misiniz? (Genelde false yeterli)")]
    public bool updateEveryFrame = false;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("[ScoreUIManager] *** ERROR: scoreText atanmamis! Inspector'dan Text componentini atayin. ***");
            return;
        }

        Debug.Log("[ScoreUIManager] Initialized successfully. Score text found.");

        // Baslangicta skoru guncelle
        UpdateScoreDisplay();
    }

    void Update()
    {
        // Eger her frame guncelleme istenmisse
        if (updateEveryFrame)
        {
            UpdateScoreDisplay();
        }
    }

    /// <summary>
    /// Skor UI'sini gunceller (ScoreManager'dan guncel skoru alir)
    /// </summary>
    public void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            // ScoreManager'dan formatlanmis skoru al (000000 formati)
            string formattedScore = ScoreManager.Instance.GetScoreFormatted();
            scoreText.text = formattedScore;

            Debug.Log($"[ScoreUIManager] *** Score UI updated to: {formattedScore} ***");
            Debug.Log($"[ScoreUIManager] *** scoreText.text is now: '{scoreText.text}' ***");
        }
        else
        {
            Debug.LogError("[ScoreUIManager] *** ERROR: Cannot update - scoreText is NULL! ***");
        }
    }

    /// <summary>
    /// Skor eklendiginde cagirilabilir (opsiyonel animasyon vs icin)
    /// </summary>
    public void OnScoreAdded(int addedPoints)
    {
        UpdateScoreDisplay();

        // Buraya istege bagli animasyon, ses efekti vs eklenebilir
        Debug.Log($"[ScoreUIManager] +{addedPoints} points added!");
    }
}
