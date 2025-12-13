using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// successful sahnesini yoneten script
/// Siparisin basarili oldugunu gosterir
/// Onceki karakterin "bekleme" halini gosterir
/// 2-3 saniye sonra order01'e doner
/// </summary>
public class SuccessfulSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Karakterin gosterilecegi Image component'i")]
    public Image characterImage;

    [Header("Settings")]
    [Tooltip("Kac saniye sonra order01'e donulsun")]
    public float waitTimeBeforeReturn = 2.5f;

    void Start()
    {
        // Karakter gorselini guncelle (bekleme hali)
        UpdateCharacterDisplay();

        // Basarili siparis olarak kaydet
        GameFlowManager.Instance.OnOrderSuccess();

        // Belirli sure sonra order01'e don
        Invoke(nameof(ReturnToOrderScene), waitTimeBeforeReturn);

        Debug.Log($"[SuccessfulSceneManager] Siparis basarili! {waitTimeBeforeReturn} saniye sonra order01'e donuluyor...");
    }

    /// <summary>
    /// Karakter gorselini gunceller (bekleme hali)
    /// </summary>
    private void UpdateCharacterDisplay()
    {
        if (characterImage == null)
        {
            Debug.LogWarning("[SuccessfulSceneManager] characterImage atanmamis! Karakter gorseli gosterilemiyor.");
            return;
        }

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;
        if (currentCharacter == null)
        {
            Debug.LogError("[SuccessfulSceneManager] currentCharacter null!");
            return;
        }

        // Karakter gorselini degistir (bekleme hali)
        characterImage.sprite = currentCharacter.waitingSprite;

        Debug.Log($"[SuccessfulSceneManager] Karakter bekleme hali gosteriliyor: {currentCharacter.characterName}");
    }

    /// <summary>
    /// order01 sahnesine doner
    /// </summary>
    private void ReturnToOrderScene()
    {
        Debug.Log("[SuccessfulSceneManager] order01 sahnesine donuluyor...");
        SceneManager.LoadScene("order01");
    }
}
