using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    [Header("Otomatik Geçiş Ayarları")]
    [Tooltip("Kaç saniye sonra bir sonraki sahneye geçilsin")]
    public float waitTime = 10f;
    
    void Start()
    {
        // waitTime saniye sonra LoadNextScene fonksiyonunu çağır
        Invoke("LoadNextScene", waitTime);
    }
    
    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Son sahnedesiniz!");
            // İsterseniz ilk sahneye dön: SceneManager.LoadScene(0);
        }
    }
}