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
            // Türkçe karakterleri doğru karşılaştırmak için manuel kontrol
            bool found = false;
            int foundQuantity = 0;

            foreach (var kvp in providedIngredients)
            {
                // Case-insensitive ve culture-insensitive karşılaştırma
                if (string.Equals(kvp.Key, recipeIng.ingredientName, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    foundQuantity = kvp.Value;
                    break;
                }
            }

            if (!found || foundQuantity != recipeIng.quantity)
            {
                return false;
            }
        }

        return true;
    }
}
