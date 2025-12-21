using UnityEngine;

/// <summary>
/// Manages player username storage and retrieval
/// Username is stored locally using PlayerPrefs
/// </summary>
public class UsernameManager : MonoBehaviour
{
    private static UsernameManager instance;
    public static UsernameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("UsernameManager");
                instance = go.AddComponent<UsernameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private const string USERNAME_KEY = "PlayerUsername";
    private const string USERNAME_SET_KEY = "UsernameSet";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Gets the current username. Returns default if not set.
    /// </summary>
    public string GetUsername()
    {
        return PlayerPrefs.GetString(USERNAME_KEY, GameConstants.DEFAULT_USERNAME);
    }

    /// <summary>
    /// Sets the username and marks it as configured
    /// </summary>
    /// <param name="username">The username to set</param>
    public void SetUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("Attempted to set empty username");
            return;
        }

        // Trim and validate length
        username = username.Trim();
        if (username.Length < GameConstants.MIN_USERNAME_LENGTH)
        {
            Debug.LogWarning($"Username too short (min {GameConstants.MIN_USERNAME_LENGTH} chars)");
            return;
        }

        if (username.Length > GameConstants.MAX_USERNAME_LENGTH)
        {
            username = username.Substring(0, GameConstants.MAX_USERNAME_LENGTH);
        }

        PlayerPrefs.SetString(USERNAME_KEY, username);
        PlayerPrefs.SetInt(USERNAME_SET_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log($"Username set to: {username}");
    }

    /// <summary>
    /// Checks if the user has already set a username
    /// </summary>
    /// <returns>True if username has been set before</returns>
    public bool HasUsername()
    {
        return PlayerPrefs.GetInt(USERNAME_SET_KEY, 0) == 1;
    }

    /// <summary>
    /// Clears the username (for testing or reset)
    /// </summary>
    public void ClearUsername()
    {
        PlayerPrefs.DeleteKey(USERNAME_KEY);
        PlayerPrefs.DeleteKey(USERNAME_SET_KEY);
        PlayerPrefs.Save();
        Debug.Log("Username cleared");
    }

    /// <summary>
    /// Validates if a username is acceptable
    /// </summary>
    /// <param name="username">Username to validate</param>
    /// <returns>True if valid</returns>
    public bool IsValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return false;

        username = username.Trim();

        if (username.Length < GameConstants.MIN_USERNAME_LENGTH) return false;
        if (username.Length > GameConstants.MAX_USERNAME_LENGTH) return false;

        // Check for invalid characters (optional - you can customize)
        // For now, allow any characters
        return true;
    }
}
