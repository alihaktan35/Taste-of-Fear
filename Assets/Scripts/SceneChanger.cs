using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
            // İsterseniz ilk sahneye dönebilir: SceneManager.LoadScene(0);
        }
    }
    
    // İsme göre sahne yükleme
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // Order01 sahnesine direkt geçiş
    public void LoadOrder01()
    {
        SceneManager.LoadScene("order01");
    }
    
    // OptionsMenu (Settings) sahnesini aç
    public void OpenSettings()
    {
        SceneManager.LoadScene("optionsMenu");
    }
    
    // Ana menüye geri dön
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Ana sahnenizin adını buraya yazın
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