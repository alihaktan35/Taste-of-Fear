using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization; // 1. Localization kütüphanesini ekledik

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public Sprite background;
        // 2. 'string' yerine 'LocalizedString' kullanarak Inspector'dan seçim yapýlmasýný saðladýk
        public LocalizedString message;
        public Vector2 arrowPos;
        public Vector2 bubblePos;
        public Vector2 bubbleSize;
        public float arrowRotation;
    }

    public TutorialStep[] steps;
    private int currentIndex = 0;

    [Header("UI References")]
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

        // 3. Metni yerelleþtirilmiþ olarak ekrana yazdýrýyoruz
        if (current.message != null)
        {
            textDisplay.text = current.message.GetLocalizedString();
        }

        arrowRect.anchoredPosition = current.arrowPos;
        bubbleRect.anchoredPosition = current.bubblePos;
        bubbleRect.sizeDelta = current.bubbleSize;
        arrowRect.rotation = Quaternion.Euler(0, 0, current.arrowRotation);

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