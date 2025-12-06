using UnityEngine;
using TMPro; // TextMeshPro kullandiginiz icin bu zorunludur
using UnityEngine.UI; // UI elemanlari icin

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
        public string recipeName;
        [TextArea(3, 10)] // Unity Inspector'da daha genis bir metin alani saglar
        public string ingredients;
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

    // Tarif Kitabini Acma/Kapama fonksiyonu
    public void ToggleRecipeBook()
    {
        // Panel aciksa kapat, kapaliysa ac
        bool isCurrentlyActive = recipeBookPanel.activeSelf;
        recipeBookPanel.SetActive(!isCurrentlyActive);

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

        // Sayfa numarasini ve tarif adini guncelle - Alt satira alindi
        recipeNameText.text = $"Tarif #{currentPageIndex + 1}\n{currentRecipe.recipeName}";

        // Malzeme listesini guncelle - Buyuk X'leri kucuk x'e cevir
        string formattedIngredients = currentRecipe.ingredients.Replace("X ", "x ");
        ingredientsListText.text = formattedIngredients;

        // Butonlari devre disi birakip/etkinlestirmeyi de ekleyebilirsiniz (dongusel yapmazsaniz)
    }
}