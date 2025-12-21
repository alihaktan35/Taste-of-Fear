using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardButtonHandler : MonoBehaviour
{
    public void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("leaderboard");
    }
}
