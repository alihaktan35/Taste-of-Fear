using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class OrderSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public Button goToTableButton;
    public Text speechBubbleText;

    [Header("Localization Settings")]
    public LocalizedString speechLocalizedString;

    [Header("Sound")]
    public CharacterSoundPlayer characterSoundPlayer;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void Start()
    {
        // Ensure the LocalizedString is configured to prevent errors.
        if (speechLocalizedString.IsEmpty)
        {
            speechLocalizedString.TableReference = "UI_Texts";
            // Use the original, single key for the speech bubble.
            speechLocalizedString.TableEntryReference = "order_speech_text";
        }

        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.PrepareNewOrder();
        }

        UpdateCharacterDisplay();
        UpdateSpeechBubbleText();
        PlayCharacterGreetingSound();

        if (goToTableButton != null)
        {
            goToTableButton.onClick.RemoveAllListeners();
            goToTableButton.onClick.AddListener(OnGoToTableClicked);
        }
    }

    private void OnLocaleChanged(Locale obj)
    {
        UpdateSpeechBubbleText();
    }

    private void UpdateCharacterDisplay()
    {
        if (characterImage == null || GameFlowManager.Instance == null) return;
        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        if (currentCharacter != null)
        {
            characterImage.sprite = currentCharacter.orderSprite;
        }
    }

    private void UpdateSpeechBubbleText()
    {
        if (speechBubbleText == null || GameFlowManager.Instance == null) return;
        StartCoroutine(UpdateSpeechBubbleTextCoroutine());
    }
    
    // This is the new, resilient logic.
    private IEnumerator UpdateSpeechBubbleTextCoroutine()
    {
        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;

        if (currentCharacter == null || currentRecipe == null)
        {
            speechBubbleText.text = "Error: Character or Recipe not set.";
            yield break;
        }

        // --- Resiliently find Character Name ---
        var localizedCharNameOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Texts", "char_" + currentCharacter.characterName.Trim().ToLowerInvariant().Replace(" ", "_"));
        yield return localizedCharNameOp;
        string localizedCharName = localizedCharNameOp.Result ?? currentCharacter.characterName;

        // --- Resiliently find Food Name ---
        string localizedFoodName = null;
        string originalRecipeName = currentRecipe.recipeName;

        // Key generation logic from my previous attempt.
        StringBuilder sb = new StringBuilder(originalRecipeName.ToLowerInvariant().Trim());
        sb.Replace('ç', 'c'); sb.Replace('ğ', 'g'); sb.Replace('ı', 'i'); sb.Replace('ö', 'o'); sb.Replace('ş', 's'); sb.Replace('ü', 'u');
        sb.Replace('Ç', 'c'); sb.Replace('Ğ', 'g'); sb.Replace('İ', 'i'); sb.Replace('Ö', 'o'); sb.Replace('Ş', 's'); sb.Replace('Ü', 'u');
        sb.Replace(' ', '_');
        
        string[] possibleKeys = new string[]
        {
            // The original, messy keys are the most likely to exist. Try them first.
            "food_" + originalRecipeName.Trim().ToLower().Replace(" ", "_"),
            "food_" + originalRecipeName.Trim().ToLowerInvariant().Replace(" ", "_"),
            // My ASCII-safe key logic.
            "food_" + sb.ToString(),
            // The key with potential trailing tabs that I saw before.
            "food_" + originalRecipeName.Trim().ToLower().Replace(" ", "_") + "\t\t\t"
        };
        
        foreach (var key in possibleKeys)
        {
            var foodNameOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Texts", key);
            yield return foodNameOp;

            if (foodNameOp.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && foodNameOp.Result != key)
            {
                localizedFoodName = foodNameOp.Result;
                Debug.Log($"SUCCESS: Found translation for '{originalRecipeName}' using key: '{key}'");
                break;
            }
        }
        
        if (localizedFoodName == null)
        {
            localizedFoodName = $"[No Translation: {originalRecipeName}]";
            Debug.LogError($"FAILURE: Could not find any translation for recipe '{originalRecipeName}'. Checked multiple key variations.");
        }

        // Set arguments and get the final string
        speechLocalizedString.Arguments = new object[] { localizedCharName, localizedFoodName };
        var speechOp = speechLocalizedString.GetLocalizedStringAsync();
        yield return speechOp;
        
        speechBubbleText.text = speechOp.Result ?? "Loading...";
    }

    public void OnGoToTableClicked()
    {
        if (GameFlowManager.Instance != null && GameFlowManager.Instance.currentRecipe != null)
        {
            TableSceneManager.LoadTableSceneWithRecipe(GameFlowManager.Instance.currentRecipe.recipeName);
        }
    }

    private void PlayCharacterGreetingSound()
    {
        if (characterSoundPlayer != null)
        {
            characterSoundPlayer.PlayCharacterGreeting();
        }
    }
}