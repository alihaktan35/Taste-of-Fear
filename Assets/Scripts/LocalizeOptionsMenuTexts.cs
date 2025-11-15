using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

/// <summary>
/// Automatically localizes UI text elements in the optionsMenu scene.
/// Attach this to the Canvas in the optionsMenu scene.
/// Note: The language button and sound button texts are handled by SettingsManager.
/// </summary>
public class LocalizeOptionsMenuTexts : MonoBehaviour
{
    void Start()
    {
        // Localize static texts (not buttons managed by SettingsManager)
        LocalizeTextByName("goBackText", "UI_Texts", "btn_back");
        LocalizeTextByName("settingsHeadText", "UI_Texts", "title_settings");

        // Note: languageText and soundText are dynamically updated by SettingsManager
        // so we don't localize them here
    }

    void LocalizeTextByName(string objectName, string tableName, string entryKey)
    {
        // Find the GameObject by name
        GameObject textObject = GameObject.Find(objectName);

        if (textObject == null)
        {
            Debug.LogWarning($"Could not find GameObject: {objectName}");
            return;
        }

        SetupLocalization(textObject, tableName, entryKey);
    }

    void SetupLocalization(GameObject textObject, string tableName, string entryKey)
    {
        // Get or add LocalizeStringEvent component
        LocalizeStringEvent localizeEvent = textObject.GetComponent<LocalizeStringEvent>();
        if (localizeEvent == null)
        {
            localizeEvent = textObject.AddComponent<LocalizeStringEvent>();
        }

        // Set up the localized string reference
        localizeEvent.StringReference = new LocalizedString(tableName, entryKey);

        // Get the Text component
        Text textComponent = textObject.GetComponent<Text>();
        if (textComponent != null)
        {
            // Set up the update handler to update the text
            localizeEvent.OnUpdateString.RemoveAllListeners(); // Clear any existing listeners
            localizeEvent.OnUpdateString.AddListener((value) => {
                textComponent.text = value;
            });

            // Manually trigger the first update
            localizeEvent.RefreshString();
        }
        else
        {
            Debug.LogWarning($"No Text component found on {textObject.name}");
        }
    }
}
