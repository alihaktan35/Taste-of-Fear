using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Text;

public class TableSceneManager : MonoBehaviour
{
    public static string RequestedRecipeName = ""; // Static variable to pass recipe name between scenes

    [Header("Recipe Database")]
    public RecipeDatabase recipeDatabase;

    [Header("UI References")]
    public Text instructionText; // Unity UI Text component
    public UIPlateController plateController;

    [Header("Localization")]
    public LocalizedString instructionLocalizedString;

    [Header("Debug Info")]
    [SerializeField] private string currentRecipeName; // Inspector'da görünür hali

    private RecipeData currentRecipe;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void Start()
    {
        // Ensure the LocalizedString is configured to prevent errors.
        if (instructionLocalizedString.IsEmpty)
        {
            instructionLocalizedString.TableReference = "UI_Texts";
            instructionLocalizedString.TableEntryReference = "instruction_text_1";
        }
        
        // Get the requested recipe
        if (string.IsNullOrEmpty(RequestedRecipeName))
        {
            Debug.LogWarning("No recipe requested! Using default for testing.");
            if (recipeDatabase != null && recipeDatabase.allRecipes.Count > 0)
            {
                currentRecipe = recipeDatabase.allRecipes[0];
            }
        }
        else
        {
            currentRecipe = recipeDatabase.GetRecipeByName(RequestedRecipeName);
            if (currentRecipe == null)
            {
                Debug.LogError($"Recipe '{RequestedRecipeName}' not found in database!");
                return;
            }
        }

        SetupRecipe();
    }

    void SetupRecipe()
    {
        if (currentRecipe == null)
        {
            Debug.LogError("No recipe to set up!");
            return;
        }

        currentRecipeName = currentRecipe.recipeName;
        UpdateInstructionText();

        if (plateController != null)
        {
            plateController.SetRecipe(currentRecipe);
        }
    }
    
    private void OnLocaleChanged(Locale obj)
    {
        UpdateInstructionText();
    }

    private void UpdateInstructionText()
    {
        if (instructionText == null || currentRecipe == null) return;
        StartCoroutine(UpdateInstructionTextCoroutine());
    }

    private IEnumerator UpdateInstructionTextCoroutine()
    {
        // --- Resiliently find Food Name ---
        string localizedFoodName = null;
        string originalRecipeName = currentRecipe.recipeName;

        StringBuilder sb = new StringBuilder(originalRecipeName.ToLowerInvariant().Trim());
        sb.Replace('ç', 'c'); sb.Replace('ğ', 'g'); sb.Replace('ı', 'i'); sb.Replace('ö', 'o'); sb.Replace('ş', 's'); sb.Replace('ü', 'u');
        sb.Replace('Ç', 'c'); sb.Replace('Ğ', 'g'); sb.Replace('İ', 'i'); sb.Replace('Ö', 'o'); sb.Replace('Ş', 's'); sb.Replace('Ü', 'u');
        sb.Replace(' ', '_');
        
        string[] possibleKeys = new string[]
        {
            "food_" + originalRecipeName.Trim().ToLower().Replace(" ", "_"),
            "food_" + originalRecipeName.Trim().ToLowerInvariant().Replace(" ", "_"),
            "food_" + sb.ToString(),
            "food_" + originalRecipeName.Trim().ToLower().Replace(" ", "_") + "\t\t\t"
        };
        
        foreach (var key in possibleKeys)
        {
            var foodNameOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Texts", key);
            yield return foodNameOp;

            if (foodNameOp.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && foodNameOp.Result != key)
            {
                localizedFoodName = foodNameOp.Result;
                Debug.Log($"SUCCESS: Found translation for '{originalRecipeName}' using key: '{key}'");
                break;
            }
        }
        
        if (localizedFoodName == null)
        {
            localizedFoodName = $"[No Translation: {originalRecipeName}]";
            Debug.LogError($"FAILURE: Could not find any translation for recipe '{originalRecipeName}'. Checked multiple key variations.");
        }

        // Set arguments and get the final string
        instructionLocalizedString.Arguments = new object[] { localizedFoodName.ToUpper() }; // The original string was uppercase
        var instructionOp = instructionLocalizedString.GetLocalizedStringAsync();
        yield return instructionOp;
        
        instructionText.text = instructionOp.Result ?? "Loading...";
    }

    public static void LoadTableSceneWithRecipe(string recipeName)
    {
        RequestedRecipeName = recipeName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("table01");
    }
}
