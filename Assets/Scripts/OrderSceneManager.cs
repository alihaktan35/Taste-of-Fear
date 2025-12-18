using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// order01 sahnesini yoneten script
/// GameFlowManager'dan karakter ve yemek bilgisini alir
/// Karakteri gosterir ve GO TO TABLE butonuna yemek bilgisini aktarir
/// </summary>
public class OrderSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Canvas > background > character (karakterin gorseli)")]
    public Image characterImage;

    [Tooltip("GO TO TABLE butonu (goToButton)")]
    public Button goToTableButton;

    [Tooltip("Canvas > background > speechBubble > text_speechBubble (konusma balonu metni)")]
    public Text speechBubbleText;

    [Header("Sound")]
    [Tooltip("Karakter ses efekti oynatici (CharacterSoundPlayer script'i olan obje)")]
    public CharacterSoundPlayer characterSoundPlayer;

    void Start()
    {
        // GameFlowManager'dan yeni bir siparis hazirla
        GameFlowManager.Instance.PrepareNewOrder();

        // Karakter gorselini guncelle
        UpdateCharacterDisplay();

        // Konusma balonu metnini guncelle
        UpdateSpeechBubbleText();

        // Karakter giris sesini oynat
        PlayCharacterGreetingSound();

        // GO TO TABLE butonuna listener ekle
        if (goToTableButton != null)
        {
            // Onceki listener'lari temizle (중ift tiklama onleme)
            goToTableButton.onClick.RemoveAllListeners();

            // Yeni listener ekle
            goToTableButton.onClick.AddListener(OnGoToTableClicked);
        }
        else
        {
            Debug.LogError("[OrderSceneManager] goToTableButton atanmamis!");
        }
    }

    /// <summary>
    /// Karakter gorselini gunceller (siparis verme hali)
    /// </summary>
    private void UpdateCharacterDisplay()
    {
        if (characterImage == null)
        {
            Debug.LogError("[OrderSceneManager] characterImage atanmamis! Inspector'dan atayin.");
            return;
        }

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        if (currentCharacter == null)
        {
            Debug.LogError("[OrderSceneManager] currentCharacter null! GameFlowManager kontrol edin.");
            return;
        }

        // Karakter gorselini degistir (siparis verme hali)
        characterImage.sprite = currentCharacter.orderSprite;

        Debug.Log($"[OrderSceneManager] Karakter gorseli guncellendi: {currentCharacter.characterName}");
    }

    /// <summary>
    /// Konusma balonu metnini karakter ve siparis bilgisine gore gunceller
    /// </summary>
    private void UpdateSpeechBubbleText()
    {
        if (speechBubbleText == null)
        {
            Debug.LogError("[OrderSceneManager] speechBubbleText atanmamis! Inspector'dan atayin.");
            return;
        }

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;

        if (currentCharacter == null || currentRecipe == null)
        {
            Debug.LogError("[OrderSceneManager] currentCharacter veya currentRecipe null!");
            return;
        }

        // Karakter adini Ingilizceye cevir
        string characterTitle = GetCharacterTitle(currentCharacter.characterName);

        // Dinamik metin olustur
        string speechText = $"Hey! It is {characterTitle}.\n" +
                           $"Welcome to the game.\n" +
                           $"I want {currentRecipe.recipeName}.\n" +
                           $"When you are ready,\n" +
                           $"press the button\n" +
                           $"named \"Go To Table\".";

        speechBubbleText.text = speechText;

        Debug.Log($"[OrderSceneManager] Konusma balonu guncellendi: {characterTitle} - {currentRecipe.recipeName}");
    }

    /// <summary>
    /// Karakter adini Ingilizce unvana cevirir
    /// </summary>
    private string GetCharacterTitle(string characterName)
    {
        switch (characterName.ToLower())
        {
            case "golge":
                return "Mr. Shadow";
            case "hayalet":
                return "Mr. Ghost";
            case "palyaco":
                return "Mr. Clown";
            case "vampir":
                return "Mr. Vampire";
            case "zombie":
                return "Mr. Zombie";
            default:
                Debug.LogWarning($"[OrderSceneManager] Bilinmeyen karakter adi: {characterName}");
                return "Mr. " + characterName;
        }
    }

    /// <summary>
    /// GO TO TABLE butonuna tiklandiginda cagrilir
    /// table01 sahnesine gecer ve yemek bilgisini aktarir
    /// </summary>
    private void OnGoToTableClicked()
    {
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;

        if (currentRecipe == null)
        {
            Debug.LogError("[OrderSceneManager] currentRecipe null! table01'e gecilemedi.");
            return;
        }

        Debug.Log($"[OrderSceneManager] GO TO TABLE - Yemek: {currentRecipe.recipeName}");

        // TableSceneManager'in static metodunu kullanarak table01'e gec
        TableSceneManager.LoadTableSceneWithRecipe(currentRecipe.recipeName);
    }

    /// <summary>
    /// Karakter giris sesini oynatir
    /// </summary>
    private void PlayCharacterGreetingSound()
    {
        if (characterSoundPlayer != null)
        {
            characterSoundPlayer.PlayCharacterGreeting();
        }
        else
        {
            Debug.LogWarning("[OrderSceneManager] characterSoundPlayer atanmamis! Ses calmayi atliyorum.");
        }
    }
}
