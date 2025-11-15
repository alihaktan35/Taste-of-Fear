using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

/// <summary>
/// Automatically localizes UI text elements in the startMenu scene.
/// Attach this to the Canvas in the startMenu scene.
/// </summary>
public class LocalizeStartMenuTexts : MonoBehaviour
{
    void Start()
    {
        // Localize the game title
        LocalizeTextByName("LogoText", "UI_Texts", "title_game");

        // Localize button texts (find Text component inside button)
        LocalizeButtonText("startButton", "UI_Texts", "btn_start");
        LocalizeButtonText("SettingsButton", "UI_Texts", "btn_settings");
        LocalizeButtonText("QuitButton", "UI_Texts", "btn_quit");
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

    void LocalizeButtonText(string buttonName, string tableName, string entryKey)
    {
        // Find the button
        GameObject button = GameObject.Find(buttonName);

        if (button == null)
        {
            Debug.LogWarning($"Could not find button: {buttonName}");
            return;
        }

        // Find the Text component (usually a child)
        Text textComponent = button.GetComponentInChildren<Text>();

        if (textComponent == null)
        {
            Debug.LogWarning($"No Text component found in {buttonName}");
            return;
        }

        SetupLocalization(textComponent.gameObject, tableName, entryKey);
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
