using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlateController : MonoBehaviour, IDropHandler
{
    public GameObject hamburgerPrefab;
    public GameObject instructionText; // Reference to the instruction text to hide when complete

    // Required ingredient names (must match the Image GameObject names exactly)
    private readonly List<string> requiredIngredients = new List<string>
    {
        "BreadImage",
        "WormsImage",
        "PotionImage",
        "MeatSliceImage"
    };

    private List<string> ingredientsOnPlate = new List<string>();
    private List<GameObject> ingredientObjectsOnPlate = new List<GameObject>();

    // Called when a UI object is dropped onto this plate
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dragged object is an ingredient
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIDraggableItem>() != null)
        {
            string originalName = eventData.pointerDrag.gameObject.name;
            string ingredientName = originalName.Replace("(Clone)", "").Trim();

            Debug.Log($"Dropped item: '{originalName}' -> cleaned: '{ingredientName}'");
            Debug.Log($"Current ingredients on plate: {string.Join(", ", ingredientsOnPlate)}");
            Debug.Log($"Required ingredients: {string.Join(", ", requiredIngredients)}");

            // Check if this is a valid ingredient
            if (!requiredIngredients.Contains(ingredientName))
            {
                Debug.LogWarning($"'{ingredientName}' is not a required ingredient!");
                return;
            }

            // Check if we already have this ingredient
            if (!ingredientsOnPlate.Contains(ingredientName))
            {
                ingredientsOnPlate.Add(ingredientName);
                GameObject ingredient = eventData.pointerDrag.gameObject;
                ingredientObjectsOnPlate.Add(ingredient);

                Debug.Log($"Added '{ingredientName}' to plate. Total: {ingredientsOnPlate.Count}/{requiredIngredients.Count}");

                // Move ingredient to plate position instead of destroying it
                RectTransform ingredientRect = ingredient.GetComponent<RectTransform>();
                RectTransform plateRect = GetComponent<RectTransform>();

                if (ingredientRect != null && plateRect != null)
                {
                    // Set parent to plate's parent (same canvas level)
                    ingredientRect.SetParent(plateRect.parent, false);

                    // Position ingredient on the plate with slight offset based on count
                    Vector2 offset = new Vector2(0, (ingredientsOnPlate.Count - 1) * 10); // Stack with 10 pixel offset
                    ingredientRect.anchoredPosition = plateRect.anchoredPosition + offset;

                    // Scale down the ingredient slightly to fit on plate
                    ingredientRect.localScale = new Vector3(0.5f, 0.5f, 1f);

                    // Disable raycasting on ingredient so it doesn't block the plate
                    Image ingredientImage = ingredient.GetComponent<Image>();
                    if (ingredientImage != null)
                    {
                        ingredientImage.raycastTarget = false;
                    }

                    // Disable dragging for this ingredient
                    UIDraggableItem draggable = ingredient.GetComponent<UIDraggableItem>();
                    if (draggable != null)
                    {
                        draggable.enabled = false;
                    }
                }

                CheckForRecipe();
            }
            else
            {
                Debug.LogWarning($"'{ingredientName}' already on plate!");
            }
        }
    }

    void CheckForRecipe()
    {
        Debug.Log($"Checking recipe: {ingredientsOnPlate.Count} of {requiredIngredients.Count} ingredients");

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
                Debug.LogWarning($"Missing required ingredient: {required}");
                break;
            }
        }

        if (hasAllIngredients)
        {
            Debug.Log("All ingredients present! Crafting hamburger...");
            CraftHamburger();
        }
    }

    void CraftHamburger()
    {
        Debug.Log("Recipe complete! Hamburger ready.");

        // Hide instruction text
        if (instructionText != null)
        {
            instructionText.SetActive(false);
        }

        // Destroy all ingredient objects that are on the plate
        foreach (GameObject ingredientObj in ingredientObjectsOnPlate)
        {
            if (ingredientObj != null)
            {
                Destroy(ingredientObj);
            }
        }

        // Create hamburger at plate position
        GameObject craftedHamburger = Instantiate(hamburgerPrefab, transform.position, Quaternion.identity);
        craftedHamburger.transform.SetParent(transform.parent, false);

        // Adjust hamburger RectTransform if it's a UI element
        RectTransform hamburgerRect = craftedHamburger.GetComponent<RectTransform>();
        if (hamburgerRect != null)
        {
            hamburgerRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            hamburgerRect.sizeDelta = new Vector2(256, 212);
        }

        ingredientsOnPlate.Clear();
        ingredientObjectsOnPlate.Clear();
    }
}
