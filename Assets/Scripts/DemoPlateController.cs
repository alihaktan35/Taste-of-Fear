using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Demo sahnesi (table01-yedek) için basitleştirilmiş tabak controller'ı.
/// 4 malzeme tabağa sürüklenince otomatik olarak hamburger görselini gösterir.
/// Recipe sistemi gerektirmez.
/// </summary>
public class DemoPlateController : MonoBehaviour, IDropHandler
{
    [Header("Demo Settings")]
    [Tooltip("Tabak görseli (bu görsel kalacak)")]
    public Image plateImage;

    [Tooltip("Hamburger gösterilecek Image component (tabağın üstünde)")]
    public Image hamburgerImage;

    [Tooltip("Hamburger sprite (1hamburger_0)")]
    public Sprite hamburgerSprite;

    [Tooltip("Kaç malzeme gerekli (varsayılan: 4)")]
    public int requiredIngredientCount = 4;

    [Tooltip("Hamburger'in tabağa göre Y pozisyon offset'i (yukarı taşımak için pozitif değer)")]
    public float hamburgerYOffset = 100f;

    private List<GameObject> ingredientObjectsOnPlate = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        // Sürüklenen objenin ingredient olup olmadığını kontrol et
        if (eventData.pointerDrag != null)
        {
            UIDraggableItem draggedItem = eventData.pointerDrag.GetComponent<UIDraggableItem>();
            if (draggedItem != null)
            {
                Debug.Log($"[Demo] Dropped ingredient onto plate");

                // Malzemeyi listeye ekle
                GameObject ingredient = eventData.pointerDrag.gameObject;
                ingredientObjectsOnPlate.Add(ingredient);

                // Malzemeyi tabağa taşı
                RectTransform ingredientRect = ingredient.GetComponent<RectTransform>();
                RectTransform plateRect = GetComponent<RectTransform>();

                if (ingredientRect != null && plateRect != null)
                {
                    // Parent'ı canvas'a ayarla (tabağın kardeşi yap)
                    ingredientRect.SetParent(plateRect.parent, false);

                    // Tabağın üzerine rastgele offset ile yerleştir
                    Vector2 randomOffset = new Vector2(
                        Random.Range(-50f, 50f),
                        Random.Range(-30f, 30f)
                    );
                    ingredientRect.anchoredPosition = plateRect.anchoredPosition + randomOffset;

                    // Küçült
                    ingredientRect.localScale = new Vector3(0.5f, 0.5f, 1f);

                    // En üste getir
                    ingredientRect.SetAsLastSibling();

                    // Sürüklemeyi devre dışı bırak
                    draggedItem.MarkAsPlacedOnPlate();
                }

                Debug.Log($"[Demo] Total ingredients on plate: {ingredientObjectsOnPlate.Count}/{requiredIngredientCount}");

                // 4 malzeme tamamlandı mı kontrol et
                CheckForCompletion();
            }
        }
    }

    void CheckForCompletion()
    {
        if (ingredientObjectsOnPlate.Count >= requiredIngredientCount)
        {
            Debug.Log($"[Demo] All {requiredIngredientCount} ingredients collected! Showing hamburger.");
            ShowHamburger();
        }
    }

    void ShowHamburger()
    {
        // Malzemeleri temizle
        foreach (GameObject ingredientObj in ingredientObjectsOnPlate)
        {
            if (ingredientObj != null)
            {
                Destroy(ingredientObj);
            }
        }
        ingredientObjectsOnPlate.Clear();

        // Hamburger'i göster (tabağın üstünde)
        if (hamburgerImage != null && hamburgerSprite != null)
        {
            hamburgerImage.sprite = hamburgerSprite;
            hamburgerImage.enabled = true;

            // Hamburger'i tabağın üstüne yerleştir (Y offset ile yukarı taşı)
            RectTransform hamburgerRect = hamburgerImage.GetComponent<RectTransform>();
            RectTransform plateRect = plateImage != null ? plateImage.GetComponent<RectTransform>() : GetComponent<RectTransform>();

            if (hamburgerRect != null && plateRect != null)
            {
                // Tabağın pozisyonunu al ve Y ekseninde yukarı taşı
                hamburgerRect.anchoredPosition = plateRect.anchoredPosition + new Vector2(0, hamburgerYOffset);

                // Uygun boyuta ayarla
                hamburgerRect.sizeDelta = new Vector2(400, 400);

                // En üste getir
                hamburgerRect.SetAsLastSibling();
            }

            Debug.Log("[Demo] Hamburger displayed successfully on plate!");
        }
        else
        {
            Debug.LogWarning("[Demo] Cannot display hamburger - HamburgerImage or HamburgerSprite is null!");
            if (hamburgerImage == null) Debug.LogWarning("  -> hamburgerImage is null");
            if (hamburgerSprite == null) Debug.LogWarning("  -> hamburgerSprite is null");
        }
    }
}
