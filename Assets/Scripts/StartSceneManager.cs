using UnityEngine;

/// <summary>
/// Manages the Start Scene (main menu)
/// Resets the score when the player returns to the start menu
/// This happens when:
/// - Starting a new game
/// - Quitting from pause menu
/// - Getting a jumpscare (game over)
/// </summary>
public class StartSceneManager : MonoBehaviour
{
    void Start()
    {
        // Reset score to zero whenever start scene is loaded
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            Debug.Log("[StartSceneManager] Score reset to 0");
        }
    }
}
