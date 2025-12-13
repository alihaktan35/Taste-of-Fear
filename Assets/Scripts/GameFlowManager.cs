using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton - Oyunun akisini yoneten ana manager
/// Karakter ve yemek secimini, siparis takibini, high score yonetimini yapar
/// DontDestroyOnLoad ile sahneler arasi korunur
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    // Singleton instance
    private static GameFlowManager _instance;
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
        // Singleton kontrolu
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Kaydedilmis verileri yukle
        LoadGameData();

        Debug.Log($"[GameFlowManager] Initialized - High Score: {highScore}, Total Orders: {totalOrders}");
    }

    /// <summary>
    /// Yeni bir tur icin rastgele karakter ve yemek secer
    /// </summary>
    public void PrepareNewOrder()
    {
        // Rastgele karakter sec
        if (characterDatabase != null)
        {
            currentCharacter = characterDatabase.GetRandomCharacter();
            if (currentCharacter == null)
            {
                Debug.LogError("[GameFlowManager] Karakter secilemedi! CharacterDatabase kontrol edin.");
                return;
            }
        }
        else
        {
            Debug.LogError("[GameFlowManager] CharacterDatabase atanmamis!");
            return;
        }

        // Rastgele yemek sec (kullanilmamis olanlardan)
        if (recipeDatabase != null)
        {
            currentRecipe = SelectRandomRecipe();
            if (currentRecipe == null)
            {
                Debug.LogError("[GameFlowManager] Yemek secilemedi! RecipeDatabase kontrol edin.");
                return;
            }
        }
        else
        {
            Debug.LogError("[GameFlowManager] RecipeDatabase atanmamis!");
            return;
        }

        Debug.Log($"[GameFlowManager] Yeni siparis hazirlandi: {currentCharacter.characterName} - {currentRecipe.recipeName}");
    }

    /// <summary>
    /// Kullanilmamis yemeklerden rastgele birini secer
    /// Tum yemekler kullanildiysa listeyi sifirlar ve tekrar baslar
    /// </summary>
    private RecipeData SelectRandomRecipe()
    {
        if (recipeDatabase.allRecipes.Count == 0)
        {
            Debug.LogError("[GameFlowManager] RecipeDatabase'de yemek yok!");
            return null;
        }

        // Tum yemekler kullanildiysa listeyi sifirla
        if (usedRecipeNames.Count >= recipeDatabase.allRecipes.Count)
        {
            Debug.Log($"[GameFlowManager] Tum {recipeDatabase.allRecipes.Count} yemek kullanildi! Liste sifirlaniyor...");
            usedRecipeNames.Clear();
        }

        // Kullanilmamis yemeklerin listesini olustur
        List<RecipeData> availableRecipes = new List<RecipeData>();
        foreach (RecipeData recipe in recipeDatabase.allRecipes)
        {
            // Türkçe karakter desteği için manuel karşılaştırma
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

        // Eger kullanilmamis yemek yoksa (hata durumu), listeyi sifirla
        if (availableRecipes.Count == 0)
        {
            Debug.LogWarning("[GameFlowManager] Kullanilmamis yemek bulunamadi, liste sifirlaniyor...");
            usedRecipeNames.Clear();
            availableRecipes = new List<RecipeData>(recipeDatabase.allRecipes);
        }

        // Rastgele bir yemek sec
        int randomIndex = Random.Range(0, availableRecipes.Count);
        RecipeData selectedRecipe = availableRecipes[randomIndex];

        // Kullanilan yemeklere ekle
        usedRecipeNames.Add(selectedRecipe.recipeName);

        Debug.Log($"[GameFlowManager] Yemek secildi: {selectedRecipe.recipeName} (Kullanilan: {usedRecipeNames.Count}/{recipeDatabase.allRecipes.Count})");

        return selectedRecipe;
    }

    /// <summary>
    /// Siparis basarili oldugundan cagrilir (successful sahnesinden)
    /// </summary>
    public void OnOrderSuccess()
    {
        successfulOrders++;
        totalOrders++;

        Debug.Log($"[GameFlowManager] Siparis BASARILI! (Basarili: {successfulOrders}, Basarisiz: {failedOrders}, Toplam: {totalOrders})");

        // High score kontrolu
        UpdateHighScore();

        // Verileri kaydet
        SaveGameData();
    }

    /// <summary>
    /// Siparis basarisiz oldugundan cagrilir (jumpscare sahnesinden)
    /// Skoru sifiirlar!
    /// </summary>
    public void OnOrderFailed()
    {
        failedOrders++;
        totalOrders++;

        Debug.Log($"[GameFlowManager] Siparis BASARISIZ! (Basarili: {successfulOrders}, Basarisiz: {failedOrders}, Toplam: {totalOrders})");

        // SKOR SIFIRLAMA (jumpscare durumunda)
        int currentScore = ScoreManager.Instance.GetScore();
        Debug.Log($"[GameFlowManager] Skor sifirlaniyor! (Onceki skor: {currentScore})");
        ScoreManager.Instance.ResetScore();

        // High score kontrolu (sifirlanmadan once)
        UpdateHighScore();

        // Verileri kaydet
        SaveGameData();
    }

    /// <summary>
    /// Guncel skoru high score ile karsilastirir ve gerekirse gunceller
    /// </summary>
    private void UpdateHighScore()
    {
        int currentScore = ScoreManager.Instance.GetScore();

        if (currentScore > highScore)
        {
            highScore = currentScore;
            Debug.Log($"[GameFlowManager] *** YENi HIGH SCORE: {highScore} ***");
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Oyun verilerini yukler (PlayerPrefs)
    /// </summary>
    private void LoadGameData()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        totalOrders = PlayerPrefs.GetInt(TOTAL_ORDERS_KEY, 0);
        successfulOrders = PlayerPrefs.GetInt(SUCCESSFUL_ORDERS_KEY, 0);
        failedOrders = PlayerPrefs.GetInt(FAILED_ORDERS_KEY, 0);

        Debug.Log($"[GameFlowManager] Veriler yuklendi - High Score: {highScore}, Total: {totalOrders}, Success: {successfulOrders}, Failed: {failedOrders}");
    }

    /// <summary>
    /// Oyun verilerini kaydeder (PlayerPrefs)
    /// </summary>
    private void SaveGameData()
    {
        PlayerPrefs.SetInt(TOTAL_ORDERS_KEY, totalOrders);
        PlayerPrefs.SetInt(SUCCESSFUL_ORDERS_KEY, successfulOrders);
        PlayerPrefs.SetInt(FAILED_ORDERS_KEY, failedOrders);
        PlayerPrefs.Save();

        Debug.Log($"[GameFlowManager] Veriler kaydedildi.");
    }

    /// <summary>
    /// High score'u dondurur
    /// </summary>
    public int GetHighScore()
    {
        return highScore;
    }

    /// <summary>
    /// Istatistikleri dondurur
    /// </summary>
    public (int successful, int failed, int total) GetOrderStatistics()
    {
        return (successfulOrders, failedOrders, totalOrders);
    }

    /// <summary>
    /// Oyun verilerini tamamen sifirlar (yeni oyun baslatildiginda)
    /// </summary>
    public void ResetGameData()
    {
        successfulOrders = 0;
        failedOrders = 0;
        totalOrders = 0;
        usedRecipeNames.Clear();
        ScoreManager.Instance.ResetScore();

        SaveGameData();

        Debug.Log("[GameFlowManager] Oyun verileri sifirlandi (High score korundu).");
    }
}
