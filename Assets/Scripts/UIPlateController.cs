using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPlateController : MonoBehaviour, IDropHandler
{
    public static bool WasPoisonousDishMade = false; // Static variable to track if poisonous dish was made

    [Header("Recipe System")]
    public RecipeData currentRecipe; // Set by TableSceneManager

    [Header("UI References")]
    public Image plateImage; // The plate image that shows the final dish
    public CountdownTimer countdownTimer; // Reference to the timer

    private Dictionary<string, int> ingredientsOnPlate = new Dictionary<string, int>();
    private List<GameObject> ingredientObjectsOnPlate = new List<GameObject>();

    // Called when a UI object is dropped onto this plate
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dragged object is an ingredient
        if (eventData.pointerDrag != null)
        {
            UIDraggableItem draggedItem = eventData.pointerDrag.GetComponent<UIDraggableItem>();
            if (draggedItem != null && draggedItem.ingredientName != null)
            {
                string ingredientName = draggedItem.ingredientName;

                Debug.Log($"Dropped ingredient: {ingredientName}");

                // Add ingredient to plate
                if (ingredientsOnPlate.ContainsKey(ingredientName))
                {
                    ingredientsOnPlate[ingredientName]++;
                }
                else
                {
                    ingredientsOnPlate[ingredientName] = 1;
                }

                // Get the dragged GameObject
                GameObject ingredient = eventData.pointerDrag.gameObject;
                ingredientObjectsOnPlate.Add(ingredient);

                // Move ingredient to plate position
                RectTransform ingredientRect = ingredient.GetComponent<RectTransform>();
                RectTransform plateRect = GetComponent<RectTransform>();

                if (ingredientRect != null && plateRect != null)
                {
                    // Set parent to Canvas (plateRect.parent) NOT to PlateImage itself
                    // This is CRITICAL - ingredients must be siblings of PlateImage, not children
                    // They will render AFTER PlateImage because SetAsLastSibling() puts them at the end
                    ingredientRect.SetParent(plateRect.parent, false);

                    // Position ingredient on the plate with random slight offset
                    Vector2 randomOffset = new Vector2(
                        Random.Range(-50f, 50f),
                        Random.Range(-30f, 30f)
                    );
                    ingredientRect.anchoredPosition = plateRect.anchoredPosition + randomOffset;

                    // Scale down the ingredient to fit on plate
                    ingredientRect.localScale = new Vector3(0.5f, 0.5f, 1f);

                    // CRITICAL: This makes ingredient render AFTER PlateImage (on top of it)
                    ingredientRect.SetAsLastSibling();

                    // Mark as placed on plate
                    draggedItem.MarkAsPlacedOnPlate();
                }

                Debug.Log($"Ingredients on plate: {GetIngredientsSummary()}");

                // Check if recipe is complete
                CheckForRecipe();
            }
        }
    }

    void CheckForRecipe()
    {
        if (currentRecipe == null)
        {
            Debug.LogWarning("No recipe set for this cooking session!");
            return;
        }

        // Check if the ingredients match the recipe
        if (currentRecipe.MatchesRecipe(ingredientsOnPlate))
        {
            Debug.Log($"Recipe complete: {currentRecipe.recipeName}");
            CompleteDish();
        }
        else
        {
            Debug.Log("Ingredients don't match recipe yet.");
        }
    }

    void CompleteDish()
    {
        Debug.Log($"Dish completed: {currentRecipe.recipeName}");

        // Check if this is a poisonous dish
        if (currentRecipe.isPoisonous)
        {
            WasPoisonousDishMade = true;
            Debug.Log("WARNING: Poisonous dish was made!");
        }
        else
        {
            WasPoisonousDishMade = false;
        }

        // Stop the timer - recipe completed successfully!
        if (countdownTimer != null)
        {
            countdownTimer.StopTimer();
        }

        // Clear all ingredient objects from plate
        foreach (GameObject ingredientObj in ingredientObjectsOnPlate)
        {
            if (ingredientObj != null)
            {
                Destroy(ingredientObj);
            }
        }
        ingredientObjectsOnPlate.Clear();
        ingredientsOnPlate.Clear();

        // Show the final dish on the plate
        if (plateImage != null && currentRecipe.dishSprite != null)
        {
            plateImage.sprite = currentRecipe.dishSprite;
            plateImage.enabled = true;

            // Make sure the plate image is visible and properly sized
            RectTransform plateRect = plateImage.GetComponent<RectTransform>();
            if (plateRect != null)
            {
                // Optionally adjust size to fit the dish image better
                plateRect.sizeDelta = new Vector2(400, 400);
            }
        }
        else
        {
            Debug.LogWarning("Cannot display dish - PlateImage or dishSprite is null!");
        }

        // Wait a moment then end scene
        Invoke("EndScene", 2f);
    }

    void EndScene()
    {
        // TODO: Return to order scene or next scene
        Debug.Log("Scene complete! Returning to previous scene...");
        // SceneManager.LoadScene("order01");
    }

    string GetIngredientsSummary()
    {
        string summary = "";
        foreach (KeyValuePair<string, int> kvp in ingredientsOnPlate)
        {
            summary += $"{kvp.Key}:{kvp.Value} ";
        }
        return summary;
    }

    // Public method to set the recipe (called by TableSceneManager)
    public void SetRecipe(RecipeData recipe)
    {
        currentRecipe = recipe;
        Debug.Log($"Recipe set to: {recipe.recipeName}");
    }
}
