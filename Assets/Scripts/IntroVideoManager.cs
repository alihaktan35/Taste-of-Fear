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
        GameObject fadeObj = GameObject.Find("FadePanel");
        if (fadeObj == null)
        {
            Debug.LogError("FadePanel bulunamadı! İsmini kontrol et.");
            return;
        }

        fadePanel = fadeObj.GetComponent<CanvasGroup>();
        if (fadePanel == null)
        {
            Debug.LogError("FadePanel üzerinde CanvasGroup yok!");
        }
    }

    void Start()
    {
        // Başlangıçta ekran siyah olsun
        fadePanel.alpha = 1f;

        // Videoyu ekrana tam oturması için aspect ratio'yu ayarla
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

        videoPlayer.loopPointReached += OnVideoEnd;

        // Sahne açılırken fade-in efekti
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
