using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component for a single leaderboard entry row
/// Displays rank, username, and score
/// </summary>
public class LeaderboardEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text rankText;
    [SerializeField] private Text usernameText;
    [SerializeField] private Text scoreText;

    /// <summary>
    /// Updates the entry with leaderboard data
    /// </summary>
    /// <param name="rank">Position in leaderboard (1-10)</param>
    /// <param name="entry">Leaderboard entry data</param>
    public void SetData(int rank, LeaderboardEntry entry)
    {
        if (rankText != null)
        {
            rankText.text = $"#{rank}";
        }

        if (usernameText != null)
        {
            usernameText.text = entry.username;
        }

        if (scoreText != null)
        {
            scoreText.text = entry.score.ToString("D" + GameConstants.SCORE_DISPLAY_DIGITS);
        }
    }

    /// <summary>
    /// Highlights this entry (e.g., for current player)
    /// </summary>
    /// <param name="highlight">True to highlight</param>
    public void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            // Change color to highlight (e.g., gold/yellow)
            if (usernameText != null)
            {
                usernameText.color = new Color(1f, 0.843f, 0f); // Gold
            }
        }
        else
        {
            // Default color (white)
            if (usernameText != null)
            {
                usernameText.color = Color.white;
            }
        }
    }
}
