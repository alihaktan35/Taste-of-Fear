using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI olaylarý için bu namespace'i ekleyin
using UnityEngine.UI; // Image bileþeni için

public class UIPlateController : MonoBehaviour, IDropHandler
{
    public GameObject hamburgerPrefab; // Canvas içinde görüntülenecekse UI Image prefab olmalý

    // Tarif için gereken malzeme isimleri (Malzeme Image nesnelerinizin isimleri ile ayný olmalý)
    private readonly List<string> requiredIngredients = new List<string>
    {
        "BreadImage",
        "WormsImage",
        "PotionImage",
        "MeatSliceImage"
    };

    private List<string> ingredientsOnPlate = new List<string>();

    // Bir UI nesnesi bu tabaðýn üzerine býrakýldýðýnda çalýþýr
    public void OnDrop(PointerEventData eventData)
    {
        // Sürüklenen nesnenin bir malzeme olup olmadýðýný kontrol et
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIDraggableItem>() != null)
        {
            string ingredientName = eventData.pointerDrag.gameObject.name.Replace("(Clone)", "").Trim();

            if (!ingredientsOnPlate.Contains(ingredientName))
            {
                ingredientsOnPlate.Add(ingredientName);

                // Malzeme görselini Canvas'tan kaldýr
                Destroy(eventData.pointerDrag.gameObject);

                CheckForRecipe();
            }
        }
    }

    void CheckForRecipe()
    {
        if (ingredientsOnPlate.Count != requiredIngredients.Count)
        {
            return;
        }

        bool hasAllIngredients = true;
        foreach (string required in requiredIngredients)
        {
            if (!ingredientsOnPlate.Contains(required))
            {
                hasAllIngredients = false;
                break;
            }
        }

        if (hasAllIngredients)
        {
            CraftHamburger();
        }
    }

    void CraftHamburger()
    {
        Debug.Log("Tarif tamamlandý! Hamburger hazýr.");

        // Hamburgeri tabaðýn konumunda Canvas içinde oluþtur
        GameObject craftedHamburger = Instantiate(hamburgerPrefab, transform.position, Quaternion.identity);
        craftedHamburger.transform.SetParent(transform.parent, false); // Canvas'ýn altýna koy

        // Hamburger UI elementiyse RectTransform'unu ayarla
        RectTransform hamburgerRect = craftedHamburger.GetComponent<RectTransform>();
        if (hamburgerRect != null)
        {
            hamburgerRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            hamburgerRect.sizeDelta = new Vector2(256, 212); // Boyutunu ayarlayabilirsiniz
        }

        ingredientsOnPlate.Clear();
    }
}