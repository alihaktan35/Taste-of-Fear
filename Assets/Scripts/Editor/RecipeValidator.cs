using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Unity Editor tool to validate all recipes and find potential ingredient name mismatches
/// </summary>
public class RecipeValidator : EditorWindow
{
    private Vector2 scrollPosition;
    private List<ValidationResult> results = new List<ValidationResult>();
    private bool hasValidated = false;

    [System.Serializable]
    public class ValidationResult
    {
        public RecipeData recipe;
        public List<string> warnings = new List<string>();
        public List<string> errors = new List<string>();
        public bool hasIssues => warnings.Count > 0 || errors.Count > 0;
    }

    [MenuItem("Tools/Validate Recipes")]
    public static void ShowWindow()
    {
        GetWindow<RecipeValidator>("Recipe Validator");
    }

    void OnGUI()
    {
        GUILayout.Label("Recipe Validation Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Validate All Recipes", GUILayout.Height(30)))
        {
            ValidateAllRecipes();
        }

        GUILayout.Space(10);

        if (hasValidated)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (results.Count == 0)
            {
                GUILayout.Label("No recipes found!", EditorStyles.boldLabel);
            }
            else
            {
                int issueCount = results.Count(r => r.hasIssues);
                if (issueCount == 0)
                {
                    GUILayout.Label("✓ All recipes are valid!", EditorStyles.boldLabel);
                }
                else
                {
                    GUILayout.Label($"Found {issueCount} recipe(s) with issues:", EditorStyles.boldLabel);
                }

                GUILayout.Space(10);

                foreach (var result in results)
                {
                    if (!result.hasIssues)
                    {
                        // Show valid recipes in green
                        GUI.color = Color.green;
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.Label($"✓ {result.recipe.recipeName}", EditorStyles.boldLabel);
                        GUILayout.EndVertical();
                        GUI.color = Color.white;
                    }
                    else
                    {
                        // Show recipes with issues
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        GUI.color = Color.red;
                        GUILayout.Label($"✗ {result.recipe.recipeName}", EditorStyles.boldLabel);
                        GUI.color = Color.white;

                        if (result.errors.Count > 0)
                        {
                            GUI.color = new Color(1f, 0.5f, 0.5f);
                            foreach (var error in result.errors)
                            {
                                GUILayout.Label($"  ERROR: {error}");
                            }
                            GUI.color = Color.white;
                        }

                        if (result.warnings.Count > 0)
                        {
                            GUI.color = Color.yellow;
                            foreach (var warning in result.warnings)
                            {
                                GUILayout.Label($"  WARNING: {warning}");
                            }
                            GUI.color = Color.white;
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.Space(5);
                }
            }

            GUILayout.EndScrollView();
        }
    }

    void ValidateAllRecipes()
    {
        results.Clear();
        hasValidated = true;

        // Find all RecipeData assets
        string[] guids = AssetDatabase.FindAssets("t:RecipeData");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RecipeData recipe = AssetDatabase.LoadAssetAtPath<RecipeData>(path);

            if (recipe != null)
            {
                ValidationResult result = new ValidationResult { recipe = recipe };
                ValidateRecipe(recipe, result);
                results.Add(result);
            }
        }

        // Sort: issues first, then alphabetically
        results = results.OrderByDescending(r => r.hasIssues).ThenBy(r => r.recipe.recipeName).ToList();

        Debug.Log($"[RecipeValidator] Validated {results.Count} recipes. Found {results.Count(r => r.hasIssues)} with issues.");
    }

    void ValidateRecipe(RecipeData recipe, ValidationResult result)
    {
        // Check 1: Empty recipe name
        if (string.IsNullOrEmpty(recipe.recipeName))
        {
            result.errors.Add("Recipe name is empty!");
        }

        // Check 2: No ingredients
        if (recipe.ingredients == null || recipe.ingredients.Count == 0)
        {
            result.errors.Add("Recipe has no ingredients!");
        }
        else
        {
            // Check 3: Ingredient validation
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ingredient = recipe.ingredients[i];

                if (string.IsNullOrEmpty(ingredient.ingredientName))
                {
                    result.errors.Add($"Ingredient #{i + 1} has empty name!");
                }
                else
                {
                    // Check for Turkish character issues
                    string ingredientName = ingredient.ingredientName;

                    // Check for common typos in Turkish characters
                    if (ingredientName.Contains("ıla") && !ingredientName.Contains("ılaş"))
                    {
                        if (recipe.recipeName.Contains("Göz Küresi") && ingredientName.Contains("pıhtılan"))
                        {
                            result.errors.Add($"Ingredient '{ingredientName}' - Possible typo: 'pıhtıLANmış' should be 'pıhtıLAŞmış'?");
                        }
                    }

                    // Check for special characters that might cause issues
                    if (ingredientName.Contains("(") || ingredientName.Contains(")"))
                    {
                        result.warnings.Add($"Ingredient '{ingredientName}' contains parentheses - this might cause matching issues");
                    }

                    // Check for leading/trailing spaces
                    if (ingredientName != ingredientName.Trim())
                    {
                        result.errors.Add($"Ingredient '{ingredientName}' has leading or trailing spaces!");
                    }

                    // Check for double spaces
                    if (ingredientName.Contains("  "))
                    {
                        result.warnings.Add($"Ingredient '{ingredientName}' contains double spaces");
                    }
                }

                if (ingredient.quantity <= 0)
                {
                    result.errors.Add($"Ingredient '{ingredient.ingredientName}' has invalid quantity: {ingredient.quantity}");
                }
            }
        }

        // Check 4: Missing dish sprite
        if (recipe.dishSprite == null)
        {
            result.warnings.Add("Recipe has no dish sprite assigned");
        }
    }
}
