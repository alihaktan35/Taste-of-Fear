using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // Changed from TMPro

public class SceneChanger : MonoBehaviour
{
    public Text warningUsernameText; // Changed from TextMeshProUGUI
    private Coroutine showWarningCoroutine;

    void Start()
    {
        if (warningUsernameText != null)
        {
            warningUsernameText.gameObject.SetActive(false);
        }
    }

    // Bir sonraki sahneye geçiş (Build Settings sırasına göre)
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Toplam sahne sayısını kontrol et
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Son sahnedesiniz!");
        }
    }

    // İsme göre sahne yükleme
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // YENİ: How to Play butonuna basıldığında Tutorial sahnesine gider
    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    // Order01 sahnesine direkt geçiş
    public void LoadOrder01()
    {
        if (UsernameManager.Instance.HasUsername())
        {
            // Yeni oyun başlarken skoru ve oyun verilerini sıfırla
            GameFlowManager.Instance.ResetGameData();
            SceneManager.LoadScene("order01");
        }
        else
        {
            if (showWarningCoroutine == null)
            {
                showWarningCoroutine = StartCoroutine(ShowWarning(3f));
            }
        }
    }

    private IEnumerator ShowWarning(float duration)
    {
        warningUsernameText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        warningUsernameText.gameObject.SetActive(false);
        showWarningCoroutine = null;
    }

    // OptionsMenu (Settings) sahnesini aç
    public void OpenSettings()
    {
        SceneManager.LoadScene("optionsMenu");
    }

    // Ana menüye geri dön
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Oyundan çık
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}