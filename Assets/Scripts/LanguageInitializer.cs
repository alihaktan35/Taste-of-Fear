using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageInitializer : MonoBehaviour
{
    void Awake()
    {
        LoadLanguage();
    }

    private void LoadLanguage()
    {
        // Load the saved language preference (defaulting to Turkish if not set).
        // 1 for Turkish, 0 for English.
        int languageIndex = PlayerPrefs.GetInt("Language", 1); 

        // Set the language based on the loaded preference.
        if (LocalizationSettings.AvailableLocales.Locales.Count > languageIndex)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
            Debug.Log($"[LanguageInitializer] Language set to: {LocalizationSettings.SelectedLocale.Identifier.Code}");
        }
        else
        {
            Debug.LogError($"[LanguageInitializer] Invalid language index: {languageIndex}. Check PlayerPrefs 'Language' key and available locales.");
        }
    }
}
