using UnityEngine;
using UnityEngine.UI;

public class TableSceneManager : MonoBehaviour
{
    public static string RequestedRecipeName = ""; // Static variable to pass recipe name between scenes

    [Header("Recipe Database")]
    public RecipeDatabase recipeDatabase;

    [Header("UI References")]
    public Text instructionText; // Unity UI Text component
    public UIPlateController plateController;

    [Header("Instruction Text Template")]
    public string instructionTemplate = "XXXX YAPINIZ"; // Template with XXXX as placeholder

    private RecipeData currentRecipe;

    void Start()
    {
        // Get the requested recipe
        if (string.IsNullOrEmpty(RequestedRecipeName))
        {
            Debug.LogWarning("No recipe requested! Using default for testing.");
            // For testing, use first recipe
            if (recipeDatabase != null && recipeDatabase.allRecipes.Count > 0)
            {
                currentRecipe = recipeDatabase.allRecipes[0];
            }
        }
        else
        {
            // Find the recipe in the database
            currentRecipe = recipeDatabase.GetRecipeByName(RequestedRecipeName);

            if (currentRecipe == null)
            {
                Debug.LogError($"Recipe '{RequestedRecipeName}' not found in database!");
                return;
            }
        }

        // Set up the scene with this recipe
        SetupRecipe();
    }

    void SetupRecipe()
    {
        if (currentRecipe == null)
        {
            Debug.LogError("No recipe to set up!");
            return;
        }

        Debug.Log($"Setting up recipe: {currentRecipe.recipeName}");

        // Update instruction text with recipe name in uppercase
        if (instructionText != null)
        {
            string recipeNameUpper = currentRecipe.recipeName.ToUpper();
            instructionText.text = instructionTemplate.Replace("XXXX", recipeNameUpper);
        }

        // Set the recipe in the plate controller
        if (plateController != null)
        {
            plateController.SetRecipe(currentRecipe);
        }

        // Log the required ingredients for debugging
        Debug.Log($"Required ingredients for {currentRecipe.recipeName}:");
        foreach (RecipeIngredient ingredient in currentRecipe.ingredients)
        {
            Debug.Log($"  - {ingredient.quantity}x {ingredient.ingredientName}");
        }
    }

    // Static method to load the table scene with a specific recipe
    public static void LoadTableSceneWithRecipe(string recipeName)
    {
        RequestedRecipeName = recipeName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("table01");
    }
}
