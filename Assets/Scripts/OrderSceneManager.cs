using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class OrderSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public Button goToTableButton;
    public Text speechBubbleText;

    [Header("Localization Settings")]
    public string greetingKey = "order_greeting";
    public string instructionKey = "order_instruction_button";

    [Header("Sound")]
    public CharacterSoundPlayer characterSoundPlayer;

    void Start()
    {
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

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;

        if (currentCharacter == null || currentRecipe == null) return;

        string charKey = "char_" + currentCharacter.characterName.Trim().ToLowerInvariant().Replace(" ", "_");
        string foodKey = "food_" + currentRecipe.recipeName.Trim().ToLowerInvariant().Replace(" ", "_");

        string localizedCharName = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", charKey);
        string localizedFoodName = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", foodKey);

        string greeting = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", greetingKey, new object[] { localizedCharName, localizedFoodName });
        string instruction = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", instructionKey);

        speechBubbleText.text = $"{greeting} {instruction}";
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
} // <--- Bu parantez CS1513 hatasını çözer