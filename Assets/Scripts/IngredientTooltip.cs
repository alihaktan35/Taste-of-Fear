using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton tooltip manager for displaying ingredient names on hover
/// Shows tooltip after 1 second delay when hovering over ingredients
/// </summary>
public class IngredientTooltip : MonoBehaviour
{
    private static IngredientTooltip _instance;

    [Header("Tooltip UI References")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private Text tooltipText;

    [Header("Tooltip Settings")]
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f); // Offset from cursor
    [SerializeField] private float screenEdgePadding = 20f; // Padding from screen edges

    private RectTransform tooltipRectTransform;
    private Canvas canvas;

    /// <summary>
    /// Gets the singleton instance of IngredientTooltip
    /// </summary>
    public static IngredientTooltip Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<IngredientTooltip>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        // Get components
        if (tooltipPanel != null)
        {
            tooltipRectTransform = tooltipPanel.GetComponent<RectTransform>();
        }

        canvas = GetComponentInParent<Canvas>();

        // Hide tooltip initially
        HideTooltip();
    }

    /// <summary>
    /// Shows the tooltip with the given ingredient name at the cursor position
    /// Automatically adjusts position to stay within screen bounds
    /// </summary>
    /// <param name="ingredientName">Name of the ingredient to display</param>
    /// <param name="cursorPosition">Screen position of the cursor</param>
    public void ShowTooltip(string ingredientName, Vector2 cursorPosition)
    {
        if (tooltipPanel == null || tooltipText == null)
        {
            Debug.LogWarning("[IngredientTooltip] Tooltip UI references not set!");
            return;
        }

        // Set text
        tooltipText.text = ingredientName;

        // Show tooltip (needed to get accurate rect size)
        tooltipPanel.SetActive(true);

        // Force canvas update to get correct tooltip size
        Canvas.ForceUpdateCanvases();

        // Position tooltip with smart positioning
        PositionTooltip(cursorPosition);
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Updates tooltip position to follow cursor
    /// Automatically adjusts position to stay within screen bounds
    /// </summary>
    /// <param name="cursorPosition">Current screen position of cursor</param>
    public void UpdateTooltipPosition(Vector2 cursorPosition)
    {
        if (tooltipPanel == null || !tooltipPanel.activeSelf)
            return;

        PositionTooltip(cursorPosition);
    }

    /// <summary>
    /// Positions tooltip near cursor with smart edge detection
    /// Tooltip appears on left/top if cursor is near right/bottom edges
    /// </summary>
    /// <param name="cursorPosition">Screen position of cursor</param>
    private void PositionTooltip(Vector2 cursorPosition)
    {
        // Convert cursor position to local canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            cursorPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        // Calculate dynamic offset based on cursor position
        Vector2 dynamicOffset = offset;

        // Get tooltip size
        float tooltipWidth = tooltipRectTransform.rect.width;
        float tooltipHeight = tooltipRectTransform.rect.height;

        // Check if cursor is in right half of screen
        if (cursorPosition.x > Screen.width * 0.6f)
        {
            // Show tooltip on the LEFT side of cursor
            dynamicOffset.x = -tooltipWidth - Mathf.Abs(offset.x);
        }

        // Check if cursor is in bottom half of screen
        if (cursorPosition.y < Screen.height * 0.4f)
        {
            // Show tooltip ABOVE cursor
            dynamicOffset.y = tooltipHeight + Mathf.Abs(offset.y);
        }

        // Apply position with dynamic offset
        Vector2 targetPosition = localPoint + dynamicOffset;

        // Clamp to screen bounds (extra safety)
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float halfWidth = tooltipWidth / 2f;
        float halfHeight = tooltipHeight / 2f;

        targetPosition.x = Mathf.Clamp(targetPosition.x,
            -canvasRect.rect.width / 2f + halfWidth + screenEdgePadding,
            canvasRect.rect.width / 2f - halfWidth - screenEdgePadding);

        targetPosition.y = Mathf.Clamp(targetPosition.y,
            -canvasRect.rect.height / 2f + halfHeight + screenEdgePadding,
            canvasRect.rect.height / 2f - halfHeight - screenEdgePadding);

        tooltipRectTransform.localPosition = targetPosition;
    }
}
