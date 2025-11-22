using UnityEngine;
using TMPro; // TextMeshPro kullandýðýnýz için bu zorunludur
using UnityEngine.UI; // UI elemanlarý için

public class RecipeBookManager : MonoBehaviour
{
    // === UI Referanslarý ===
    public GameObject recipeBookPanel;
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI ingredientsListText;
    public Button nextPageButton;
    public Button previousPageButton;

    // Tarif Veri Yapýsý (Yemek Adý ve Malzemeler)
    [System.Serializable]
    public struct Recipe
    {
        public string recipeName;
        [TextArea(3, 10)] // Unity Inspector'da daha geniþ bir metin alaný saðlar
        public string ingredients;
    }

    // Tüm tariflerin listesi
    public Recipe[] allRecipes;

    // Þu anda görüntülenen tarifin indeksi
    private int currentPageIndex = 0;

    void Start()
    {
        // Panel baþlangýçta kapalý olmalý
        recipeBookPanel.SetActive(false);

        // Buton dinleyicilerini atama (Inspector'dan da yapýlabilir)
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);

        // Ýlk tarifi yükle
        if (allRecipes.Length > 0)
        {
            UpdatePageContent();
        }
        else
        {
            Debug.LogError("Tarif listesi boþ! Lütfen tarifleri girin.");
        }
    }

    // Tarif Kitabýný Açma/Kapama fonksiyonu
    public void ToggleRecipeBook()
    {
        // Panel açýksa kapat, kapalýysa aç
        bool isCurrentlyActive = recipeBookPanel.activeSelf;
        recipeBookPanel.SetActive(!isCurrentlyActive);

        // Opsiyonel: Kitap açýldýðýnda oyunu duraklatabilirsiniz.
        // if (recipeBookPanel.activeSelf) Time.timeScale = 0f;
        // else Time.timeScale = 1f;
    }

    // Sonraki sayfaya geçme
    private void NextPage()
    {
        currentPageIndex++;
        // Döngüsel geçiþ (son sayfadan sonra ilk sayfaya döner)
        if (currentPageIndex >= allRecipes.Length)
        {
            currentPageIndex = 0;
        }
        UpdatePageContent();
    }

    // Önceki sayfaya geçme
    private void PreviousPage()
    {
        currentPageIndex--;
        // Döngüsel geçiþ (ilk sayfadan önce son sayfaya döner)
        if (currentPageIndex < 0)
        {
            currentPageIndex = allRecipes.Length - 1;
        }
        UpdatePageContent();
    }

    // Sayfa içeriðini güncelleme
    private void UpdatePageContent()
    {
        Recipe currentRecipe = allRecipes[currentPageIndex];

        // Sayfa numarasýný ve tarif adýný güncelle
        recipeNameText.text = $"Tarif #{currentPageIndex + 1} - {currentRecipe.recipeName}";

        // Malzeme listesini güncelle
        ingredientsListText.text = currentRecipe.ingredients;

        // Butonlarý devre dýþý býrakýp/etkinleþtirmeyi de ekleyebilirsiniz (döngüsel yapmazsanýz)
    }
}