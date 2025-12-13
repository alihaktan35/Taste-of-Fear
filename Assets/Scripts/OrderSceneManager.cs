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

    void Start()
    {
        // GameFlowManager'dan yeni bir siparis hazirla
        GameFlowManager.Instance.PrepareNewOrder();

        // Karakter gorselini guncelle
        UpdateCharacterDisplay();

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
}
