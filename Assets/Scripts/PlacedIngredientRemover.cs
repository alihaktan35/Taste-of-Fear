using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Allows clicking on placed ingredients to remove them from the plate
/// Costs time penalty when used
/// </summary>
public class PlacedIngredientRemover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Settings")]
    [Tooltip("Time penalty in seconds when removing an ingredient")]
    public float timePenalty = 10f;

    [Header("Visual Feedback")]
    [Tooltip("Color when hovering (red tint for removal indication)")]
    public Color hoverColor = new Color(1f, 0.3f, 0.3f, 1f); // Light red

    private Image ingredientImage;
    private Color originalColor;
    private bool isHovering = false;

    // References set by UIPlateController
    [HideInInspector] public UIPlateController plateController;
    [HideInInspector] public string ingredientName;

    void Awake()
    {
        ingredientImage = GetComponent<Image>();
        if (ingredientImage != null)
        {
            originalColor = ingredientImage.color;
        }
    }

    /// <summary>
    /// Called when mouse enters the ingredient area
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ingredientImage != null)
        {
            isHovering = true;
            ingredientImage.color = hoverColor;
        }
    }

    /// <summary>
    /// Called when mouse exits the ingredient area
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (ingredientImage != null)
        {
            isHovering = false;
            ingredientImage.color = originalColor;
        }
    }

    /// <summary>
    /// Called when ingredient is clicked
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (plateController != null)
        {
            // Remove this ingredient from the plate
            plateController.RemoveIngredient(gameObject, ingredientName, timePenalty);
        }
        else
        {
            Debug.LogError("[PlacedIngredientRemover] plateController reference is missing!");
        }
    }
}
