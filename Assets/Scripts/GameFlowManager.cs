using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton - Main game flow manager
/// Handles character/recipe selection, order tracking, and high score management
/// Persists across scenes with DontDestroyOnLoad
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    private static GameFlowManager _instance;

    /// <summary>
    /// Gets the singleton instance of GameFlowManager
    /// </summary>
    public static GameFlowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameFlowManager>();
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("GameFlowManager");
                    _instance = managerObject.AddComponent<GameFlowManager>();
                }
            }
            return _instance;
        }
    }

    [Header("Databases")]
    public CharacterDatabase characterDatabase;
    public RecipeDatabase recipeDatabase;

    [Header("Current Selection")]
    public CharacterData currentCharacter;
    public RecipeData currentRecipe;

    [Header("Order Statistics")]
    [SerializeField] private int successfulOrders = 0;
    [SerializeField] private int failedOrders = 0;
    [SerializeField] private int totalOrders = 0;

    [Header("High Score")]
    [SerializeField] private int highScore = 0;

    // Kullanilan yemeklerin takibi (tekrar onleme icin)
    private List<string> usedRecipeNames = new List<string>();

    // PlayerPrefs keys
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string TOTAL_ORDERS_KEY = "TotalOrders";
    private const string SUCCESSFUL_ORDERS_KEY = "SuccessfulOrders";
    private const string FAILED_ORDERS_KEY = "FailedOrders";

    void Awake()
    {
        // Singleton pattern - destroy duplicates
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGameData();
    }

    /// <summary>
    /// Prepares a new order by randomly selecting a character and recipe
    /// </summary>
    public void PrepareNewOrder()
    {
        // Select random character
        if (characterDatabase != null)
        {
            currentCharacter = characterDatabase.GetRandomCharacter();
            if (currentCharacter == null)
            {
                Debug.LogError("[GameFlowManager] Failed to select character!");
                return;
            }
        }
        else
        {
            Debug.LogError("[GameFlowManager] CharacterDatabase not assigned!");
            return;
        }

        // Select random recipe (from unused ones)
        if (recipeDatabase != null)
        {
            currentRecipe = SelectRandomRecipe();
            if (currentRecipe == null)
            {
                Debug.LogError("[GameFlowManager] Failed to select recipe!");
                return;
            }
        }
        else
        {
            Debug.LogError("[GameFlowManager] RecipeDatabase not assigned!");
            return;
        }
    }

    /// <summary>
    /// Selects a random recipe from unused ones
    /// If all recipes are used, clears the list and starts over
    /// </summary>
    /// <returns>Selected recipe or null if database is empty</returns>
    private RecipeData SelectRandomRecipe()
    {
        if (recipeDatabase.allRecipes.Count == 0)
        {
            Debug.LogError("[GameFlowManager] RecipeDatabase is empty!");
            return null;
        }

        // Reset used recipes list if all have been used
        if (usedRecipeNames.Count >= recipeDatabase.allRecipes.Count)
        {
            usedRecipeNames.Clear();
        }

        // Build list of unused recipes
        List<RecipeData> availableRecipes = new List<RecipeData>();
        foreach (RecipeData recipe in recipeDatabase.allRecipes)
        {
            // Turkish character support - manual comparison
            bool alreadyUsed = false;
            foreach (string usedName in usedRecipeNames)
            {
                if (string.Equals(usedName, recipe.recipeName, System.StringComparison.OrdinalIgnoreCase))
                {
                    alreadyUsed = true;
                    break;
                }
            }

            if (!alreadyUsed)
            {
                availableRecipes.Add(recipe);
            }
        }

        // Fallback: if no unused recipes found (error case), reset list
        if (availableRecipes.Count == 0)
        {
            usedRecipeNames.Clear();
            availableRecipes = new List<RecipeData>(recipeDatabase.allRecipes);
        }

        // Select random recipe
        int randomIndex = UnityEngine.Random.Range(0, availableRecipes.Count);
        RecipeData selectedRecipe = availableRecipes[randomIndex];

        // Mark as used
        usedRecipeNames.Add(selectedRecipe.recipeName);

        return selectedRecipe;
    }

    /// <summary>
    /// Called when an order is completed successfully
    /// </summary>
    public void OnOrderSuccess()
    {
        successfulOrders++;
        totalOrders++;
        UpdateHighScore();
        SaveGameData();
    }

    /// <summary>
    /// Called when an order fails (jumpscare)
    /// Resets the score!
    /// </summary>
    public void OnOrderFailed()
    {
        failedOrders++;
        totalOrders++;

        // Reset score on jumpscare
        ScoreManager.Instance.ResetScore();
        UpdateHighScore();
        SaveGameData();
    }

    /// <summary>
    /// Compares current score with high score and updates if necessary
    /// </summary>
    private void UpdateHighScore()
    {
        int currentScore = ScoreManager.Instance.GetScore();

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Loads game data from PlayerPrefs
    /// </summary>
    private void LoadGameData()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        totalOrders = PlayerPrefs.GetInt(TOTAL_ORDERS_KEY, 0);
        successfulOrders = PlayerPrefs.GetInt(SUCCESSFUL_ORDERS_KEY, 0);
        failedOrders = PlayerPrefs.GetInt(FAILED_ORDERS_KEY, 0);
    }

    /// <summary>
    /// Saves game data to PlayerPrefs
    /// </summary>
    private void SaveGameData()
    {
        PlayerPrefs.SetInt(TOTAL_ORDERS_KEY, totalOrders);
        PlayerPrefs.SetInt(SUCCESSFUL_ORDERS_KEY, successfulOrders);
        PlayerPrefs.SetInt(FAILED_ORDERS_KEY, failedOrders);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Gets the current high score
    /// </summary>
    /// <returns>High score value</returns>
    public int GetHighScore()
    {
        return highScore;
    }

    /// <summary>
    /// Gets order statistics
    /// </summary>
    /// <returns>Tuple containing successful, failed, and total order counts</returns>
    public (int successful, int failed, int total) GetOrderStatistics()
    {
        return (successfulOrders, failedOrders, totalOrders);
    }

    /// <summary>
    /// Resets all game data (called when starting new game)
    /// Note: High score is preserved
    /// </summary>
    public void ResetGameData()
    {
        successfulOrders = 0;
        failedOrders = 0;
        totalOrders = 0;
        usedRecipeNames.Clear();
        ScoreManager.Instance.ResetScore();
        SaveGameData();
    }
}
