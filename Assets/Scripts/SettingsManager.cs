using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class SettingsManager : MonoBehaviour
{
    [Header("Language")]
    public Button languageButton;
    public Text languageButtonText; // veya TextMeshProUGUI

    [Header("Sound")]
    public Button soundButton;
    public Text soundButtonText; // veya TextMeshProUGUI

    private bool isTurkish = true;
    private bool isSoundOn = true;

    // Localized strings for sound button
    private LocalizedString soundOnString = new LocalizedString("UI_Texts", "sound_on");
    private LocalizedString soundOffString = new LocalizedString("UI_Texts", "sound_off");
    
    void Start()
    {
        // Kaydedilmiş ayarları yükle
        LoadSettings();
        
        // Buton eventlerini bağla
        languageButton.onClick.AddListener(ToggleLanguage);
        soundButton.onClick.AddListener(ToggleSound);
        
        // UI'ı güncelle
        UpdateUI();
    }
    
    // Dil Değiştirme
    void ToggleLanguage()
    {
        isTurkish = !isTurkish;
        
        if (isTurkish)
        {
            SetLanguage("tr", 1); // Turkish index 1
        }
        else
        {
            SetLanguage("en", 0); // English index 0
        }
        
        // Ayarı kaydet
        PlayerPrefs.SetInt("Language", isTurkish ? 1 : 0);
        PlayerPrefs.Save();
        
        UpdateUI();
    }
    
    void SetLanguage(string localeCode, int localeIndex)
    {
        // Locale'i değiştir
        LocalizationSettings.SelectedLocale = 
            LocalizationSettings.AvailableLocales.Locales[localeIndex];
    }
    
    // Ses Açma/Kapama
    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        
        // Tüm sesleri kapat/aç
        AudioListener.volume = isSoundOn ? 1f : 0f;
        
        // Ayarı kaydet
        PlayerPrefs.SetInt("Sound", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        
        UpdateUI();
    }
    
    // UI'ı Güncelle
    void UpdateUI()
    {
        // Dil butonu - shows the current language name (not localized)
        languageButtonText.text = isTurkish ? "Türkçe" : "English";

        // Ses butonu - uses localized strings
        if (isSoundOn)
        {
            soundButtonText.text = soundOnString.GetLocalizedString();
        }
        else
        {
            soundButtonText.text = soundOffString.GetLocalizedString();
        }
    }
    
    // Ayarları Yükle
    void LoadSettings()
    {
        // Dil ayarını yükle (varsayılan Türkçe)
        isTurkish = PlayerPrefs.GetInt("Language", 1) == 1;
        SetLanguage(isTurkish ? "tr" : "en", isTurkish ? 1 : 0);
        
        // Ses ayarını yükle (varsayılan Açık)
        isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }
}