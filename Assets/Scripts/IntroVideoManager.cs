using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "startMenu";
    public float fadeDuration = 1f;

    private CanvasGroup fadePanel;

    void Awake()
    {
        // Set sound to ON by default every time the game starts
        PlayerPrefs.SetInt("Sound", 1); // 1 for ON
        AudioListener.volume = 1f;
        PlayerPrefs.Save();

        GameObject fadeObj = GameObject.Find("FadePanel");
        if (fadeObj == null)
        {
            Debug.LogError("FadePanel not found! Check the name.");
            return;
        }

        fadePanel = fadeObj.GetComponent<CanvasGroup>();
        if (fadePanel == null)
        {
            Debug.LogError("CanvasGroup not found on FadePanel!");
        }
    }

    void Start()
    {
        // Start with a black screen
        fadePanel.alpha = 1f;

        // Adjust aspect ratio to fit the screen
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

        videoPlayer.loopPointReached += OnVideoEnd;

        // Fade-in effect when the scene opens
        StartCoroutine(FadeIn());
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeAndLoad());
    }

    public void SkipVideo()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 0f;
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}