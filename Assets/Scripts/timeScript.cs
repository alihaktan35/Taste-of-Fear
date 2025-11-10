using UnityEngine;
using UnityEngine.UI; // Legacy Text için gerekli

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
    
    private float currentTime;
    private Text timerText;
    private bool isWarningPhase = false;
    
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
        else
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
        Debug.Log("Süre bitti!");
        enabled = false;
    }
}