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

    // Check if provided ingredients match this recipe
    public bool MatchesRecipe(Dictionary<string, int> providedIngredients)
    {
        // Must have exact same number of ingredient types
        if (providedIngredients.Count != ingredients.Count)
            return false;

        // Check each required ingredient
        foreach (RecipeIngredient recipeIng in ingredients)
        {
            if (!providedIngredients.ContainsKey(recipeIng.ingredientName))
                return false;

            if (providedIngredients[recipeIng.ingredientName] != recipeIng.quantity)
                return false;
        }

        return true;
    }
}
