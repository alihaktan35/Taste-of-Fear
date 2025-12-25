using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecipeIngredient
{
    public string ingredientName; // Ingredient GameObject name (e.g., "asit", "göz")
    public int quantity; // How many needed
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName; // Turkish name (e.g., "Göz Küresi Çorbası")
    public Sprite dishSprite; // Final dish image
    public bool isPoisonous = false; // Zehirli mantar sepeti gibi özel tarifler için

    [Header("Ingredients")]
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

    /// <summary>
    /// Check if provided ingredients match this recipe (with Turkish character support)
    /// </summary>
    /// <param name="providedIngredients">Dictionary of ingredient names and quantities on plate</param>
    /// <returns>True if ingredients match recipe exactly</returns>
    public bool MatchesRecipe(Dictionary<string, int> providedIngredients)
    {
        // Must have exact same number of ingredient types
        if (providedIngredients.Count != ingredients.Count)
        {
            return false;
        }

        // Check each required ingredient
        foreach (RecipeIngredient recipeIng in ingredients)
        {
            // Use TryGetValue for efficient, case-insensitive lookup
            // This works because the dictionary was created with StringComparer.OrdinalIgnoreCase
            if (!providedIngredients.TryGetValue(recipeIng.ingredientName, out int providedQuantity) || providedQuantity != recipeIng.quantity)
            {
                return false;
            }
        }

        return true;
    }
}
