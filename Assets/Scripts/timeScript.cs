using UnityEngine;
using UnityEngine.UI; // Legacy Text için gerekli
using UnityEngine.SceneManagement; // Scene management için gerekli

public class CountdownTimer : MonoBehaviour
{
    [Header("Zamanlayıcı Ayarları")]
    [Tooltip("Geri sayım süresi (saniye cinsinden)")]
    public float countdownTime = 60f;
    
    [Header("Renk Ayarları")]
    [Tooltip("30 saniyenin altında yanıp sönme başlar")]
    public float warningTime = 30f;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    [Header("Scene Settings")]
    [Tooltip("Süre bittiğinde gidilecek sahne adı")]
    public string jumpscareSceneName = "jumpscare test";

    private float currentTime;
    private Text timerText;
    private bool isWarningPhase = false;
    private bool timerEnded = false;
    
    void Start()
    {
        timerText = GetComponent<Text>();
        
        if (timerText == null)
        {
            Debug.LogError("Text componenti bulunamadı!");
            enabled = false;
            return;
        }
        
        currentTime = countdownTime;
        timerText.color = normalColor;
        UpdateTimerDisplay();
    }
    
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            // 30 saniyenin altına düştüğünde yanıp sönmeyi başlat
            if (currentTime <= warningTime && !isWarningPhase)
            {
                isWarningPhase = true;
            }

            // Her saniyede renk değiştir
            if (isWarningPhase)
            {
                int currentSecond = Mathf.FloorToInt(currentTime);
                if (currentSecond % 2 == 0)
                {
                    timerText.color = warningColor; // Kırmızı
                }
                else
                {
                    timerText.color = normalColor; // Beyaz
                }
            }
        }
        else if (!timerEnded)
        {
            currentTime = 0;
            timerText.color = warningColor; // Süre bittiğinde kırmızı kalsın
            UpdateTimerDisplay();
            TimerEnded();
        }
    }
    
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    void TimerEnded()
    {
        timerEnded = true;
        Debug.Log("Time's up! Loading jumpscare scene...");

        // Load jumpscare scene
        SceneManager.LoadScene(jumpscareSceneName);
    }

    // Public method to check if time is up
    public bool IsTimeUp()
    {
        return timerEnded;
    }

    // Public method to get remaining time
    public float GetRemainingTime()
    {
        return currentTime;
    }

    // Public method to stop timer (when recipe is completed successfully)
    public void StopTimer()
    {
        enabled = false;
        Debug.Log("Timer stopped - recipe completed!");
    }

    /// <summary>
    /// Subtracts time from the countdown (used as penalty for removing ingredients)
    /// </summary>
    /// <param name="seconds">Number of seconds to subtract</param>
    public void SubtractTime(float seconds)
    {
        currentTime -= seconds;

        // Make sure time doesn't go negative
        if (currentTime < 0)
        {
            currentTime = 0;
        }

        UpdateTimerDisplay();

        // Show penalty visual feedback
        StartCoroutine(ShowPenaltyFeedback());

        Debug.Log($"[Timer] {seconds} seconds subtracted. Remaining time: {currentTime:F1}s");
    }

    /// <summary>
    /// Shows red flash effect when time penalty is applied
    /// </summary>
    private System.Collections.IEnumerator ShowPenaltyFeedback()
    {
        // Store original color
        Color originalColorBeforePenalty = timerText.color;

        // Flash red multiple times for emphasis
        for (int i = 0; i < 3; i++)
        {
            // Turn red
            timerText.color = warningColor;
            yield return new WaitForSeconds(0.2f);

            // Turn back to original
            timerText.color = originalColorBeforePenalty;
            yield return new WaitForSeconds(0.2f);
        }

        // Stay red for a moment
        timerText.color = warningColor;
        yield return new WaitForSeconds(1f);

        // Return to original color (unless we're in warning phase)
        if (currentTime > warningTime)
        {
            timerText.color = normalColor;
        }
        else
        {
            // If in warning phase, let the normal update cycle handle the color
            isWarningPhase = true;
        }
    }
}