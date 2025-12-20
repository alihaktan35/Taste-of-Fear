using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton CursorManager - Manages custom cursors throughout the game
/// Uses default cursor in most scenes, pointer cursor in table01 scene
/// </summary>
public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;

    [Header("Cursor Textures")]
    [Tooltip("Default cursor texture (cursor.png) - used in all scenes except table01")]
    public Texture2D defaultCursorTexture;

    [Tooltip("Pointer cursor texture (pointer.png) - used in table01 scene")]
    public Texture2D pointerCursorTexture;

    [Header("Cursor Hotspots")]
    [Tooltip("Default cursor hotspot - the 'click point' of the cursor")]
    public Vector2 defaultCursorHotspot = GameConstants.CURSOR_HOTSPOT;

    [Tooltip("Pointer cursor hotspot - the 'click point' for pointer")]
    public Vector2 pointerCursorHotspot = GameConstants.POINTER_HOTSPOT;

    private string currentSceneName;

    /// <summary>
    /// Gets the singleton instance of CursorManager
    /// </summary>
    public static CursorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CursorManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("CursorManager");
                    _instance = obj.AddComponent<CursorManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        // Singleton pattern - destroy duplicates
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Set initial cursor
        UpdateCursorForCurrentScene();
    }

    void OnDestroy()
    {
        // Unsubscribe from scene load events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is loaded
    /// </summary>
    /// <param name="scene">The loaded scene</param>
    /// <param name="mode">The load mode</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        UpdateCursorForCurrentScene();
    }

    /// <summary>
    /// Updates cursor based on current scene
    /// Uses pointer cursor in table01, default cursor in all other scenes
    /// </summary>
    private void UpdateCursorForCurrentScene()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        if (string.Equals(currentSceneName, GameConstants.SCENE_TABLE01, System.StringComparison.OrdinalIgnoreCase))
        {
            SetPointerCursor();
        }
        else
        {
            SetDefaultCursor();
        }
    }

    /// <summary>
    /// Sets the default cursor (cursor.png)
    /// Used in all scenes except table01
    /// </summary>
    public void SetDefaultCursor()
    {
        if (defaultCursorTexture != null)
        {
            Cursor.SetCursor(defaultCursorTexture, defaultCursorHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("[CursorManager] Default cursor texture not assigned! Using system cursor.");
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    /// <summary>
    /// Sets the pointer cursor (pointer.png)
    /// Used in table01 scene for ingredient interaction
    /// </summary>
    public void SetPointerCursor()
    {
        if (pointerCursorTexture != null)
        {
            Cursor.SetCursor(pointerCursorTexture, pointerCursorHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("[CursorManager] Pointer cursor texture not assigned! Using default cursor.");
            SetDefaultCursor();
        }
    }

    /// <summary>
    /// Resets cursor to system default (useful for debugging)
    /// </summary>
    public void ResetToSystemCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Sets cursor visibility
    /// </summary>
    /// <param name="visible">True to show cursor, false to hide</param>
    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }

    /// <summary>
    /// Locks/unlocks the cursor
    /// </summary>
    /// <param name="locked">True to lock cursor to center, false to unlock</param>
    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
