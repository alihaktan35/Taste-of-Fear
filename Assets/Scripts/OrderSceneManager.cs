using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization; // Localization kütüphanesi
using UnityEngine.Localization.Settings; // Ayarlar için gerekli

public class OrderSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public Button goToTableButton;
    public Text speechBubbleText; // Standart UI Text kullanıyorsan bu kalsın

    [Header("Localization Settings")]
    [Tooltip("Tablodaki 'order_speech_text' anahtarını buraya sürükleyin")]
    public LocalizedString speechLocalizedString;

    [Header("Sound")]
    public CharacterSoundPlayer characterSoundPlayer;

    void Start()
    {
        // 1. Yeni siparişi ve verileri hazırla
        GameFlowManager.Instance.PrepareNewOrder();

        // 2. Görselleri ve metinleri güncelle
        UpdateCharacterDisplay();
        UpdateSpeechBubbleText();

        // 3. Karakter sesini oynat
        PlayCharacterGreetingSound();

        // 4. Buton dinleyicisini ayarla
        if (goToTableButton != null)
        {
            goToTableButton.onClick.RemoveAllListeners();
            goToTableButton.onClick.AddListener(OnGoToTableClicked);
        }
    }

    private void UpdateCharacterDisplay()
    {
        if (characterImage == null) return;
        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        if (currentCharacter != null)
        {
            characterImage.sprite = currentCharacter.orderSprite;
        }
    }

    private void UpdateSpeechBubbleText()
    {
        if (speechBubbleText == null) return;

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;

        if (currentCharacter == null || currentRecipe == null)
        {
            Debug.LogError("[OrderSceneManager] Karakter veya Yemek verisi eksik!");
            return;
        }

        // --- DINAMIK ISIMLERI TABLODAN CEKME ---

        // Karakter Key oluştur (Örn: char_golge)
        string charKey = "char_" + currentCharacter.characterName.ToLower();
        string localizedCharName = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", charKey);

        // Yemek Key oluştur (Örn: food_zehirli_mantar_sepeti)
        // İsimdeki boşlukları alt tire yapar ve küçük harfe çevirir
        string foodKey = "food_" + currentRecipe.recipeName.Replace(" ", "_").ToLower();
        string localizedFoodName = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Texts", foodKey);

        // --- METNI SMART FORMAT ILE BIRLESTIRME ---

        if (speechLocalizedString != null && !speechLocalizedString.IsEmpty)
        {
            // {0} yerine Karakter İsmi, {1} yerine Yemek İsmi gider
            speechLocalizedString.Arguments = new object[] { localizedCharName, localizedFoodName };
            speechBubbleText.text = speechLocalizedString.GetLocalizedString();
        }
        else
        {
            Debug.LogWarning("[OrderSceneManager] Speech Localized String referansı seçilmemiş!");
        }
    }

    private void OnGoToTableClicked()
    {
        RecipeData currentRecipe = GameFlowManager.Instance.currentRecipe;
        if (currentRecipe != null)
        {
            TableSceneManager.LoadTableSceneWithRecipe(currentRecipe.recipeName);
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