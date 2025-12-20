using UnityEngine;

/// <summary>
/// CursorManager for table01 scene - Sets custom pointer cursor for ingredient interaction
/// Automatically resets to system cursor when scene is exited
/// </summary>
public class CursorManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [Tooltip("Pointer cursor texture (pointer16x16.png) - used in table01 scene")]
    public Texture2D pointerCursorTexture;

    [Tooltip("Pointer cursor hotspot - the 'click point' (finger tip position)")]
    public Vector2 pointerCursorHotspot = GameConstants.POINTER_HOTSPOT;

    void Start()
    {
        // Set pointer cursor when scene loads
        SetPointerCursor();
    }

    void OnDestroy()
    {
        // Reset to system cursor when leaving scene
        ResetToSystemCursor();
    }

    /// <summary>
    /// Sets the pointer cursor (pointer.png)
    /// Used in table01 scene for ingredient interaction
    /// </summary>
    private void SetPointerCursor()
    {
        if (pointerCursorTexture != null)
        {
            Cursor.SetCursor(pointerCursorTexture, pointerCursorHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("[CursorManager] Pointer cursor texture not assigned! Using system cursor.");
            ResetToSystemCursor();
        }
    }

    /// <summary>
    /// Resets cursor to system default
    /// </summary>
    private void ResetToSystemCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Sets cursor visibility (public utility method)
    /// </summary>
    /// <param name="visible">True to show cursor, false to hide</param>
    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }
}
