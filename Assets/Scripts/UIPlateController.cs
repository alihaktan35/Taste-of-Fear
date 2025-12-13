using System;
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
    public ScoreUIManager scoreUIManager; // Reference to score UI manager (optional)

    // Türkçe karakter desteği için StringComparer.OrdinalIgnoreCase kullan
    private Dictionary<string, int> ingredientsOnPlate = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    private List<GameObject> ingredientObjectsOnPlate = new List<GameObject>();

    /// <summary>
    /// Called when a UI object is dropped onto this plate
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dragged object is an ingredient
        if (eventData.pointerDrag != null)
        {
            UIDraggableItem draggedItem = eventData.pointerDrag.GetComponent<UIDraggableItem>();
            if (draggedItem != null && draggedItem.ingredientName != null)
            {
                string ingredientName = draggedItem.ingredientName;

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
                    // IMPORTANT: Disable raycasts FIRST before moving to plate
                    // This prevents the ingredient from blocking future drags
                    draggedItem.MarkAsPlacedOnPlate();

                    // Set parent to Canvas (plateRect.parent) NOT to PlateImage itself
                    // This is CRITICAL - ingredients must be siblings of PlateImage, not children
                    // They will render AFTER PlateImage because SetAsLastSibling() puts them at the end
                    ingredientRect.SetParent(plateRect.parent, false);

                    // Position ingredient on the plate with random slight offset
                    Vector2 randomOffset = new Vector2(
                        UnityEngine.Random.Range(-50f, 50f),
                        UnityEngine.Random.Range(-30f, 30f)
                    );
                    ingredientRect.anchoredPosition = plateRect.anchoredPosition + randomOffset;

                    // Scale down the ingredient to fit on plate (smaller to avoid clutter)
                    ingredientRect.localScale = new Vector3(0.2f, 0.2f, 1f);

                    // CRITICAL: This makes ingredient render AFTER PlateImage (on top of it)
                    ingredientRect.SetAsLastSibling();
                }

                // Check if recipe is complete
                CheckForRecipe();
            }
            else
            {
                Debug.LogWarning("[Plate] Dropped object is not a valid ingredient!");
            }
        }
    }

    void CheckForRecipe()
    {
        if (currentRecipe == null)
        {
            Debug.LogWarning("[Plate] No recipe set!");
            return;
        }

        // Check if the ingredients match the recipe
        if (currentRecipe.MatchesRecipe(ingredientsOnPlate))
        {
            CompleteDish();
        }
    }

    void CompleteDish()
    {
        Debug.Log($"[Plate] Dish completed: {currentRecipe.recipeName}");

        // Check if this is a poisonous dish
        WasPoisonousDishMade = currentRecipe.isPoisonous;

        // Get remaining time BEFORE stopping timer for score calculation
        float remainingTime = 0f;
        if (countdownTimer != null)
        {
            remainingTime = countdownTimer.GetRemainingTime();
            countdownTimer.StopTimer();
        }
        else
        {
            Debug.LogWarning("[Plate] Timer reference is missing!");
        }

        // Calculate and add time bonus score
        int earnedPoints = ScoreManager.Instance.CalculateAndAddTimeBonus(remainingTime);

        // Update score UI if available
        if (scoreUIManager != null)
        {
            scoreUIManager.OnScoreAdded(earnedPoints);
        }
        else
        {
            Debug.LogWarning("[Plate] ScoreUIManager reference is missing!");
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
                // Adjust size to fit the dish image better
                plateRect.sizeDelta = GameConstants.DISH_SIZE;

                // Move dish upward on Y axis so it's more visible
                Vector2 currentPos = plateRect.anchoredPosition;
                plateRect.anchoredPosition = new Vector2(currentPos.x, currentPos.y + GameConstants.DISH_Y_OFFSET);

                // Make dish render on top of all ingredients
                plateRect.SetAsLastSibling();
            }
        }
        else
        {
            Debug.LogWarning("[Plate] Cannot display dish - missing reference!");
        }

        // Wait a moment then end scene
        StartCoroutine(EndSceneAfterDelay());
    }

    /// <summary>
    /// Waits for a delay then transitions to the successful scene
    /// </summary>
    private System.Collections.IEnumerator EndSceneAfterDelay()
    {
        yield return new WaitForSeconds(GameConstants.DISH_COMPLETE_DELAY);
        SceneManager.LoadScene("successful");
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

    /// <summary>
    /// Sets the current recipe for this cooking session
    /// </summary>
    /// <param name="recipe">The recipe to cook</param>
    public void SetRecipe(RecipeData recipe)
    {
        currentRecipe = recipe;
    }
}
