using System;

/// <summary>
/// Represents a single entry in the leaderboard
/// Serializable for Firebase Realtime Database
/// </summary>
[Serializable]
public class LeaderboardEntry
{
    /// <summary>
    /// Player's username
    /// </summary>
    public string username;

    /// <summary>
    /// Player's high score
    /// </summary>
    public int score;

    /// <summary>
    /// Timestamp when score was achieved (Unix timestamp)
    /// </summary>
    public long timestamp;

    /// <summary>
    /// Creates a new leaderboard entry
    /// </summary>
    /// <param name="username">Player username</param>
    /// <param name="score">Score achieved</param>
    public LeaderboardEntry(string username, int score)
    {
        this.username = username;
        this.score = score;
        this.timestamp = 0; // Timestamp will be set by server
    }

    /// <summary>
    /// Default constructor for Firebase deserialization
    /// </summary>
    public LeaderboardEntry()
    {
        username = GameConstants.DEFAULT_USERNAME;
        score = 0;
        timestamp = 0;
    }

    /// <summary>
    /// Gets a human-readable date from timestamp
    /// </summary>
    public string GetFormattedDate()
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        return dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
    }

    public override string ToString()
    {
        return $"{username}: {score} pts ({GetFormattedDate()})";
    }
}
