using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public Sprite background;
        [TextArea(3, 5)]
        public string message;
        public Vector2 arrowPos;
        public Vector2 bubblePos;
        public Vector2 bubbleSize;
        public float arrowRotation;
    }

    public TutorialStep[] steps;
    private int currentIndex = 0;

    [Header("UI Bağlantıları")]
    public Image bgDisplay;
    public TextMeshProUGUI textDisplay;
    public RectTransform arrowRect;
    public RectTransform bubbleRect;
    public GameObject nextButton;
    public GameObject backButton;

    void Start() { UpdateUI(); }

    public void Next()
    {
        if (currentIndex < steps.Length - 1) { currentIndex++; UpdateUI(); }
        else { Skip(); } // Son adımdaysa ana menüye gitsin
    }

    public void Back()
    {
        if (currentIndex > 0) { currentIndex--; UpdateUI(); }
    }

    public void Skip()
    {
        SceneManager.LoadScene("StartMenu");
    }

    void UpdateUI()
    {
        TutorialStep current = steps[currentIndex];

        bgDisplay.sprite = current.background;
        textDisplay.text = current.message;

        arrowRect.anchoredPosition = current.arrowPos;
        bubbleRect.anchoredPosition = current.bubblePos;
        bubbleRect.sizeDelta = current.bubbleSize;
        arrowRect.rotation = Quaternion.Euler(0, 0, current.arrowRotation);

        // Show/hide buttons
        if (backButton != null)
        {
            backButton.SetActive(currentIndex > 0);
        }

        if (nextButton != null)
        {
            nextButton.SetActive(currentIndex < steps.Length - 1);
        }
    }
}
