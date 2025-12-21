using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("startMenu");
    }
}
