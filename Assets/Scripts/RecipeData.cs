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

    // Check if provided ingredients match this recipe (Türkçe karakter desteği ile)
    public bool MatchesRecipe(Dictionary<string, int> providedIngredients)
    {
        Debug.Log($"[Recipe] ===== Tarif kontrolü: '{recipeName}' =====");
        Debug.Log($"[Recipe] Tabakta {providedIngredients.Count} malzeme, tarif {ingredients.Count} malzeme gerektiriyor");

        // Must have exact same number of ingredient types
        if (providedIngredients.Count != ingredients.Count)
        {
            Debug.Log($"[Recipe] ✗ Malzeme sayısı uyuşmuyor!");
            return false;
        }

        // Check each required ingredient
        foreach (RecipeIngredient recipeIng in ingredients)
        {
            Debug.Log($"[Recipe] Aranan malzeme: '{recipeIng.ingredientName}' (miktar: {recipeIng.quantity})");

            // Türkçe karakterleri doğru karşılaştırmak için manuel kontrol
            bool found = false;
            int foundQuantity = 0;

            foreach (var kvp in providedIngredients)
            {
                Debug.Log($"[Recipe]   Tabaktaki: '{kvp.Key}' (miktar: {kvp.Value}) - Karşılaştırılıyor...");

                // Case-insensitive ve culture-insensitive karşılaştırma
                if (string.Equals(kvp.Key, recipeIng.ingredientName, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    foundQuantity = kvp.Value;
                    Debug.Log($"[Recipe]   ✓ EŞLEŞME BULUNDU!");
                    break;
                }
            }

            if (!found)
            {
                Debug.Log($"[Recipe] ✗ Malzeme bulunamadı: '{recipeIng.ingredientName}'");
                Debug.Log($"[Recipe] Tabaktakiler:");
                foreach (var kvp in providedIngredients)
                {
                    Debug.Log($"[Recipe]   - '{kvp.Key}'");
                }
                return false;
            }

            if (foundQuantity != recipeIng.quantity)
            {
                Debug.Log($"[Recipe] ✗ Malzeme miktarı uyuşmuyor: '{recipeIng.ingredientName}' (gerekli: {recipeIng.quantity}, mevcut: {foundQuantity})");
                return false;
            }
        }

        Debug.Log($"[Recipe] ✓✓✓ Tarif tamamlandı: '{recipeName}' ✓✓✓");
        return true;
    }
}
