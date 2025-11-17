using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Cooking/Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    public List<RecipeData> allRecipes = new List<RecipeData>();

    // Find a recipe by name
    public RecipeData GetRecipeByName(string recipeName)
    {
        return allRecipes.Find(recipe => recipe.recipeName == recipeName);
    }

    // Find which recipe matches the provided ingredients
    public RecipeData FindMatchingRecipe(Dictionary<string, int> ingredients)
    {
        foreach (RecipeData recipe in allRecipes)
        {
            if (recipe.MatchesRecipe(ingredients))
            {
                return recipe;
            }
        }
        return null;
    }
}
