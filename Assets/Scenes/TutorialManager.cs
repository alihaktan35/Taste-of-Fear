using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep // Her bir adýmýn neler içereceðini tanýmlýyoruz
    {
        public Sprite background;     // Sahne resmi
        public string message;        // Yazýlacak açýklama
        public Vector2 arrowPos;      // Okun konumu
        public Vector2 bubblePos;     // Balonun konumu
        public float arrowRotation;   // Okun dönüþ açýsý
    }

    public TutorialStep[] steps;      // Tüm adýmlarý tutan liste
    private int currentIndex = 0;

    [Header("UI Baðlantýlarý")]
    public Image bgDisplay;
    public TextMeshProUGUI textDisplay;
    public RectTransform arrowRect;
    public RectTransform bubbleRect;

    void Start() { UpdateUI(); } // Oyun açýlýnca ilk adýmý göster

    public void Next()
    {
        if (currentIndex < steps.Length - 1) { currentIndex++; UpdateUI(); }
        else { Debug.Log("Tutorial Bitti!"); } // Buraya ana sahneye geçiþ kodu gelecek
    }

    public void Back()
    {
        if (currentIndex > 0) { currentIndex--; UpdateUI(); }
    }

    void UpdateUI()
    {
        TutorialStep current = steps[currentIndex];
        bgDisplay.sprite = current.background;
        textDisplay.text = current.message;
        arrowRect.anchoredPosition = current.arrowPos;
        bubbleRect.anchoredPosition = current.bubblePos;
        arrowRect.rotation = Quaternion.Euler(0, 0, current.arrowRotation);
    }
}