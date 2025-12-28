using UnityEngine;
using TMPro; // TextMeshPro kullandiginiz icin bu zorunludur
using UnityEngine.UI; // UI elemanlari icin
using UnityEngine.Localization.Settings;

public class RecipeBookManager : MonoBehaviour
{
    // === UI Referanslari ===
    public GameObject recipeBookPanel;
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI ingredientsListText;
    public Button nextPageButton;
    public Button previousPageButton;

    // Tarif Veri Yapisi (Yemek Adi ve Malzemeler)
    [System.Serializable]
    public struct Recipe
    {
        [Header("Turkish")]
        public string recipeName;
        [TextArea(3, 10)]
        public string ingredients;

        [Header("English")]
        public string recipeNameEn;
        [TextArea(3, 10)]
        public string ingredientsEn;
    }

    // Tum tariflerin listesi
    public Recipe[] allRecipes;

    // Su anda goruntulenentarifin indeksi
    private int currentPageIndex = 0;

    void Start()
    {
        // Panel baslangicta kapali olmali
        recipeBookPanel.SetActive(false);

        // Buton dinleyicilerini atama (Inspector'dan da yapilabilir)
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);

        // Dil degisikligini dinle
        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;

        // Ilk tarifi yukle
        if (allRecipes.Length > 0)
        {
            UpdatePageContent();
        }
        else
        {
            Debug.LogError("Tarif listesi bos! Lutfen tarifleri girin.");
        }
    }

    void OnDestroy()
    {
        // Event listener'i temizle
        LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;
    }

    // Dil degistiginde cagirilir
    private void OnLanguageChanged(UnityEngine.Localization.Locale locale)
    {
        if (allRecipes.Length > 0)
        {
            UpdatePageContent();
        }
    }

    // Mevcut dilin Turkce olup olmadigini kontrol eder
    private bool IsTurkish()
    {
        // LocalizationSettings uzerinden aktif dili kontrol et (daha guvenilir)
        var currentLocale = LocalizationSettings.SelectedLocale;

        if (currentLocale != null)
        {
            // Locale kodu "tr" ise Turkce, degilse Ingilizce
            bool isTr = currentLocale.Identifier.Code == "tr";
            Debug.Log($"[RecipeBook] Language Check - Locale: {currentLocale.Identifier.Code}, IsTurkish: {isTr}");
            return isTr;
        }

        // Fallback: PlayerPrefs kullan (LocalizationSettings yuklenmemisse)
        int langValue = PlayerPrefs.GetInt("Language", 1);
        bool isTurkishFallback = langValue == 1;
        Debug.Log($"[RecipeBook] Language Check (Fallback) - PlayerPrefs Value: {langValue}, IsTurkish: {isTurkishFallback}");
        return isTurkishFallback;
    }

    // Tarif Kitabini Acma/Kapama fonksiyonu
    public void ToggleRecipeBook()
    {
        // Panel aciksa kapat, kapaliysa ac
        bool isCurrentlyActive = recipeBookPanel.activeSelf;
        recipeBookPanel.SetActive(!isCurrentlyActive);

        // Panel acildiginda en uste cikar (tum diger UI elementlerinin onunde gozuksun)
        if (recipeBookPanel.activeSelf)
        {
            recipeBookPanel.transform.SetAsLastSibling();

            // KRITIK: Kitap acildiginda icerigi guncelle (dil degisikligi icin)
            if (allRecipes.Length > 0)
            {
                UpdatePageContent();
            }
        }

        // Opsiyonel: Kitap acildiginda oyunu duraklatabilirsiniz.
        // if (recipeBookPanel.activeSelf) Time.timeScale = 0f;
        // else Time.timeScale = 1f;
    }

    // Sonraki sayfaya gecme
    private void NextPage()
    {
        currentPageIndex++;
        // Dongusel gecis (son sayfadan sonra ilk sayfaya doner)
        if (currentPageIndex >= allRecipes.Length)
        {
            currentPageIndex = 0;
        }
        UpdatePageContent();
    }

    // Onceki sayfaya gecme
    private void PreviousPage()
    {
        currentPageIndex--;
        // Dongusel gecis (ilk sayfadan once son sayfaya doner)
        if (currentPageIndex < 0)
        {
            currentPageIndex = allRecipes.Length - 1;
        }
        UpdatePageContent();
    }

    // Sayfa icerigini guncelleme
    private void UpdatePageContent()
    {
        Recipe currentRecipe = allRecipes[currentPageIndex];
        bool isTurkish = IsTurkish();

        // Debug: Mevcut tarif verilerini logla
        Debug.Log($"[RecipeBook] Recipe #{currentPageIndex + 1}");
        Debug.Log($"[RecipeBook] TR Name: '{currentRecipe.recipeName}' | EN Name: '{currentRecipe.recipeNameEn}'");
        Debug.Log($"[RecipeBook] TR Ingredients: '{currentRecipe.ingredients}' | EN Ingredients: '{currentRecipe.ingredientsEn}'");

        // Dile gore tarif adini ve malzemeleri sec
        string displayName;
        string displayIngredients;

        if (isTurkish)
        {
            // Turkce secili - Turkce verileri kullan
            displayName = currentRecipe.recipeName;
            displayIngredients = currentRecipe.ingredients;
            Debug.Log("[RecipeBook] Using TURKISH data");
        }
        else
        {
            // Ingilizce secili - Ingilizce verileri kullan, bossa Turkce'ye fallback
            displayName = !string.IsNullOrEmpty(currentRecipe.recipeNameEn)
                ? currentRecipe.recipeNameEn
                : currentRecipe.recipeName;

            displayIngredients = !string.IsNullOrEmpty(currentRecipe.ingredientsEn)
                ? currentRecipe.ingredientsEn
                : currentRecipe.ingredients;

            Debug.Log($"[RecipeBook] Using ENGLISH data - Name empty: {string.IsNullOrEmpty(currentRecipe.recipeNameEn)}, Ingredients empty: {string.IsNullOrEmpty(currentRecipe.ingredientsEn)}");
        }

        // Sayfa numarasi etiketi
        string recipeLabel = isTurkish ? "Tarif" : "Recipe";

        // Sayfa numarasini ve tarif adini guncelle - Alt satira alindi
        recipeNameText.text = $"{recipeLabel} #{currentPageIndex + 1}\n{displayName}";

        // Malzeme listesini guncelle - Buyuk X'leri kucuk x'e cevir
        string formattedIngredients = displayIngredients.Replace("X ", "x ");
        ingredientsListText.text = formattedIngredients;

        Debug.Log($"[RecipeBook] Final Display - Label: {recipeLabel}, Name: {displayName}");
    }
}