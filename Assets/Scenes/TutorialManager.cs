using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public Sprite background;
        [TextArea(3, 5)] // Yazý alanýný editörde geniþletir
        public string message;
        public Vector2 arrowPos;
        public Vector2 bubblePos;
        public Vector2 bubbleSize;    // YENÝ: Balonun geniþlik ve yüksekliði
        public float arrowRotation;
    }

    public TutorialStep[] steps;
    private int currentIndex = 0;

    [Header("UI Baðlantýlarý")]
    public Image bgDisplay;
    public TextMeshProUGUI textDisplay;
    public RectTransform arrowRect;
    public RectTransform bubbleRect;

    void Start() { UpdateUI(); }

    public void Next()
    {
        if (currentIndex < steps.Length - 1) { currentIndex++; UpdateUI(); }
        else { Debug.Log("Tutorial Bitti!"); }
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

        // Konumlarý güncelle
        arrowRect.anchoredPosition = current.arrowPos;
        bubbleRect.anchoredPosition = current.bubblePos;

        // YENÝ: Boyutu güncelle (Width ve Height)
        bubbleRect.sizeDelta = current.bubbleSize;

        // Rotasyonu güncelle
        arrowRect.rotation = Quaternion.Euler(0, 0, current.arrowRotation);
    }
}