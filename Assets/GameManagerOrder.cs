using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Clown Animation Objects")]
    public GameObject speechBubble;
    public GameObject goToTableButton;
    public Animator clownAnimator;
    
    private bool isWaitingForPlayer = false;
    
    void Start()
    {
        // Başlangıçta gizle
        if (speechBubble != null) speechBubble.SetActive(false);
        if (goToTableButton != null) goToTableButton.SetActive(false);
    }
    
    // Animation Event: Palyaço ekrana gelip durduğunda çağrılacak
    public void OnClownArrived()
    {
        Debug.Log("Clown arrived!");
        
        // Konuşma balonu ve butonu göster
        if (speechBubble != null) speechBubble.SetActive(true);
        if (goToTableButton != null) goToTableButton.SetActive(true);
        
        // Animasyonu durdur
        if (clownAnimator != null)
        {
            clownAnimator.speed = 0; // Animasyonu dondur
        }
        
        isWaitingForPlayer = true;
    }
    
    // Butona basıldığında çağrılacak
    public void OnGoToTableClicked()
    {
        Debug.Log("Going to table...");
        
        // Konuşma balonu ve butonu gizle
        if (speechBubble != null) speechBubble.SetActive(false);
        if (goToTableButton != null) goToTableButton.SetActive(false);
        
        // Animasyonu devam ettir (palyaço çıksın)
        if (clownAnimator != null)
        {
            clownAnimator.speed = 1; // Animasyonu devam ettir
        }
        
        isWaitingForPlayer = false;
        
        // Animasyon bitince sahne değişecek (aşağıda ayarlayacağız)
    }
    
    // Animation Event: Palyaço ekrandan çıktığında çağrılacak
    public void OnClownExit()
    {
        Debug.Log("Clown exited!");
        LoadScene("order01"); // veya gitmek istediğiniz sahne
    }
    
    // İsme göre sahne yükleme
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}