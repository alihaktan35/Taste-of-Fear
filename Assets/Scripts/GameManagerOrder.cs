using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerOrder : MonoBehaviour
{
    public static GameManagerOrder Instance; // Singleton pattern
    
    [Header("Clown Animation Objects")]
    public GameObject speechBubble;
    public GameObject goToTableButton;
    public GameObject quitGameButton;
    public Animator clownAnimator;
    
    private bool isWaitingForPlayer = false;
    
    void Awake()
    {
        Instance = this; // Statik referans
    }
    
    void Start()
    {
        // Başlangıçta gizle
        if (speechBubble != null) speechBubble.SetActive(false);
        if (goToTableButton != null) goToTableButton.SetActive(false);
        if (quitGameButton != null) quitGameButton.SetActive(false);
    }
    
    public void OnClownArrived()
    {
        Debug.Log("Clown arrived!");

        if (speechBubble != null) speechBubble.SetActive(true);
        if (goToTableButton != null) goToTableButton.SetActive(true);
        if (quitGameButton != null) quitGameButton.SetActive(true);

        if (clownAnimator != null)
        {
            clownAnimator.speed = 0;
        }

        isWaitingForPlayer = true;
    }
    
    public void OnGoToTableClicked()
    {
        Debug.Log("Going to table...");

        if (speechBubble != null) speechBubble.SetActive(false);
        if (goToTableButton != null) goToTableButton.SetActive(false);
        if (quitGameButton != null) quitGameButton.SetActive(false);

        if (clownAnimator != null)
        {
            clownAnimator.speed = 1;
        }

        isWaitingForPlayer = false;
    }

    public void OnQuitGameClicked()
    {
        Debug.Log("Quitting to start menu...");

        // startMenu sahnesine dön
        SceneManager.LoadScene("startMenu");
    }
    
    public void OnClownExit()
    {
        Debug.Log("Clown exited!");
        LoadScene("order01");
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}