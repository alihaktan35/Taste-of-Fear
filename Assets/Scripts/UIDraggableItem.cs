using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Text;

/// <summary>
/// Makes UI ingredient items draggable with clone-based drag system
/// Includes hover tooltip functionality with 1 second delay
/// </summary>
public class UIDraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 pointerOffset;
    private GameObject draggedClone;
    private Vector2 originalPosition;
    private bool isClone = false;
    private AudioSource audioSource;

    // Tooltip variables
    private Coroutine showTooltipCoroutine;
    private bool isHovering = false;

    public string ingredientName;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        audioSource = GetComponent<AudioSource>();

        // Set ingredient name from GameObject name if not set
        if (string.IsNullOrEmpty(ingredientName))
        {
            ingredientName = gameObject.name;
        }
    }

    /// <summary>
    /// Plays the ingredient sound effect
    /// </summary>
    private void PlaySound()
    {
        if (audioSource != null)
        {
            // Stop and restart sound for rapid clicks
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Play sound when picking up ingredient
        PlaySound();

        // If this is the original ingredient (not a clone), create a clone to drag
        if (!isClone)
        {
            // Create a clone
            draggedClone = Instantiate(gameObject, transform.parent);
            draggedClone.name = gameObject.name + "_CLONE";

            // Set up the clone
            UIDraggableItem cloneDraggable = draggedClone.GetComponent<UIDraggableItem>();
            if (cloneDraggable != null)
            {
                cloneDraggable.isClone = true;
                cloneDraggable.ingredientName = this.ingredientName;
                cloneDraggable.rectTransform = draggedClone.GetComponent<RectTransform>();
                cloneDraggable.canvas = this.canvas;

                // Position clone at current mouse position
                Vector2 localPointerPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPointerPosition))
                {
                    cloneDraggable.rectTransform.localPosition = localPointerPosition;
                    cloneDraggable.originalPosition = localPointerPosition;
                }

                // Start dragging the clone
                cloneDraggable.transform.SetAsLastSibling();

                // Calculate offset for the clone
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    cloneDraggable.rectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out cloneDraggable.pointerOffset
                );
            }
        }
        else
        {
            // This is a clone being dragged
            originalPosition = rectTransform.localPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out pointerOffset
            );
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Only clones can be dragged
        if (!isClone)
        {
            // Forward drag event to the clone
            if (draggedClone != null)
            {
                UIDraggableItem cloneDraggable = draggedClone.GetComponent<UIDraggableItem>();
                if (cloneDraggable != null)
                {
                    cloneDraggable.OnDrag(eventData);
                }
            }
            return;
        }

        // Drag the clone
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            rectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Only process for clones
        if (!isClone)
        {
            // Forward to clone
            if (draggedClone != null)
            {
                UIDraggableItem cloneDraggable = draggedClone.GetComponent<UIDraggableItem>();
                if (cloneDraggable != null)
                {
                    cloneDraggable.OnPointerUp(eventData);
                }
            }
            draggedClone = null;
            return;
        }

        // Check if dropped on plate by raycasting at pointer position
        bool droppedOnPlate = false;
        UIPlateController foundPlate = null;

        // Perform a raycast at the pointer position to find all UI elements underneath
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = eventData.position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // Check all raycasted objects for a UIPlateController
        foreach (RaycastResult result in raycastResults)
        {
            UIPlateController plate = result.gameObject.GetComponent<UIPlateController>();
            if (plate != null)
            {
                foundPlate = plate;
                droppedOnPlate = true;
                break;
            }
        }

        // If plate found, drop the ingredient on it
        if (droppedOnPlate && foundPlate != null)
        {
            // CRITICAL FIX: eventData.pointerDrag points to the ORIGINAL, not the clone!
            // We need to replace it with the clone (this.gameObject) before calling OnDrop
            eventData.pointerDrag = this.gameObject;
            foundPlate.OnDrop(eventData);
        }

        // If not dropped on plate, destroy the clone
        if (!droppedOnPlate)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Marks this ingredient as placed on the plate, disabling raycasts to prevent interference
    /// </summary>
    public void MarkAsPlacedOnPlate()
    {
        // Disable dragging
        enabled = false;

        // Disable ALL Graphic components' raycast targets so they don't interfere with future drags
        UnityEngine.UI.Graphic[] graphics = GetComponentsInChildren<UnityEngine.UI.Graphic>(true);
        foreach (UnityEngine.UI.Graphic graphic in graphics)
        {
            graphic.raycastTarget = false;
        }
    }

    // ========== LOCALIZED TOOLTIP SYSTEM ==========

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isClone) return;
        isHovering = true;
        if (showTooltipCoroutine != null) StopCoroutine(showTooltipCoroutine);
        showTooltipCoroutine = StartCoroutine(ShowTooltipAfterDelay(eventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
            showTooltipCoroutine = null;
        }
        if (IngredientTooltip.Instance != null)
        {
            IngredientTooltip.Instance.HideTooltip();
        }
    }
    
    private string GenerateSafeKey(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        
        StringBuilder sb = new StringBuilder(text.ToLowerInvariant().Trim());
        sb.Replace('ç', 'c'); sb.Replace('ğ', 'g'); sb.Replace('ı', 'i');
        sb.Replace('ö', 'o'); sb.Replace('ş', 's'); sb.Replace('ü', 'u');
        sb.Replace('Ç', 'c'); sb.Replace('Ğ', 'g'); sb.Replace('İ', 'i');
        sb.Replace('Ö', 'o'); sb.Replace('Ş', 's'); sb.Replace('Ü', 'u');
        sb.Replace(' ', '_');
        return sb.ToString();
    }

    private System.Collections.IEnumerator ShowTooltipAfterDelay(PointerEventData eventData)
    {
        yield return new WaitForSeconds(1f);
        if (!isHovering) yield break;

        // DEBUG: Log which ingredient is being hovered (helps identify problematic items)
        Debug.Log($"[IngredientTooltip] Hovering over GameObject: '{gameObject.name}', ingredientName: '{ingredientName}'");

        // SAFEGUARD: Check if ingredientName is valid
        if (string.IsNullOrEmpty(ingredientName))
        {
            Debug.LogError($"[IngredientTooltip] ERROR: ingredientName is null or empty for GameObject: '{gameObject.name}'");
            if (IngredientTooltip.Instance != null && isHovering)
            {
                IngredientTooltip.Instance.ShowTooltip("Unknown Item", eventData.position);
            }
            yield break;
        }

        // Generate a localization key for the ingredient
        string localizationKey = "ingredient_" + GenerateSafeKey(ingredientName);
        Debug.Log($"[IngredientTooltip] Looking for localization key: '{localizationKey}'");

        string tooltipText = ingredientName; // Fallback to original name

        // Get localized string asynchronously
        var localizedStringOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Texts", localizationKey);
        yield return localizedStringOperation;

        // Check result after yield (outside of try-catch since yield cannot be in try block)
        bool localizationSucceeded = false;
        if (localizedStringOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string result = localizedStringOperation.Result;

            // SAFEGUARD: Check if result is valid and not an error message
            if (!string.IsNullOrEmpty(result) &&
                result != localizationKey &&
                !result.Contains("Exception") &&
                !result.Contains("Error") &&
                result.Length < 100) // Reasonable length check
            {
                tooltipText = result;
                localizationSucceeded = true;
                Debug.Log($"[IngredientTooltip] Localization SUCCESS: '{localizationKey}' -> '{tooltipText}'");
            }
            else
            {
                Debug.LogWarning($"[IngredientTooltip] Localization returned invalid result for key: '{localizationKey}'. Result: '{result}'. Using fallback: '{ingredientName}'");
            }
        }
        else
        {
            Debug.LogWarning($"[IngredientTooltip] Localization operation failed for key: '{localizationKey}'. Using fallback: '{ingredientName}'");
        }

        if (!localizationSucceeded)
        {
            Debug.LogWarning($"[IngredientTooltip] No valid translation found for: '{localizationKey}'. Displaying fallback: '{tooltipText}'");
        }

        // FINAL SAFEGUARD: If tooltipText is still invalid, use "Unknown Item"
        if (string.IsNullOrEmpty(tooltipText) || tooltipText.Length > 100)
        {
            tooltipText = "Unknown Item";
            Debug.LogError($"[IngredientTooltip] FINAL FALLBACK triggered for GameObject: '{gameObject.name}'");
        }

        if (IngredientTooltip.Instance != null && isHovering)
        {
            IngredientTooltip.Instance.ShowTooltip(tooltipText, eventData.position);
            while (isHovering)
            {
                IngredientTooltip.Instance.UpdateTooltipPosition(Input.mousePosition);
                yield return null;
            }
        }
    }
}