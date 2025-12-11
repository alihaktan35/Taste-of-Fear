using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

// Bu script'i kullandığınız nesnede AudioSource bileşeninin olmasını sağlar (zorunlu kılınabilir, ancak bu kodda manuel kontrol ediyoruz)
public class UIDraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 pointerOffset;
    private GameObject draggedClone; // Clone that will be dragged
    private Vector2 originalPosition; // Original position to return if not dropped on plate
    private bool isClone = false; // Is this object a clone?

    // YENİ: Ses kaynağını tutacak değişken
    private AudioSource sesKaynagi; 

    public string ingredientName; // Name of the ingredient (e.g., "asit", "göz")

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        // YENİ: Objeye eklenmiş AudioSource bileşenini al
        // Sesin çalabilmesi için her bir malzeme objesinde (balçık, asit, vb.) Audio Source bileşeni olmalıdır.
        sesKaynagi = GetComponent<AudioSource>(); 

        // Set ingredient name from GameObject name if not set
        if (string.IsNullOrEmpty(ingredientName))
        {
            ingredientName = gameObject.name;
        }
    }
    
    // YENİ: Sesi çalma fonksiyonu
    private void CalSesiOynat() 
    {
        if (sesKaynagi != null)
        {
            // Oynayan sesi durdurup baştan başlatır (hızlı tıklamalar için ideal)
            if (sesKaynagi.isPlaying)
            {
                sesKaynagi.Stop();
            }
            sesKaynagi.Play();
            Debug.Log(gameObject.name + " sesi çalındı."); // Sesin tetiklendiğini kontrol etmek için
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"[Drag] OnPointerDown on '{ingredientName}', isClone={isClone}");

        if (!isClone)
        {
            Debug.Log($"[ORIGINAL] Before clone - active={gameObject.activeSelf}, position={rectTransform.localPosition}, parent={transform.parent.name}");
        }

        // YENİ: Sesi, sürükleme olayı başlamadan hemen önce çal!
        CalSesiOynat();

        // If this is the original ingredient (not a clone), create a clone to drag
        if (!isClone)
        {
            Debug.Log($"[Drag] Creating clone of '{ingredientName}'...");

            // Create a clone
            draggedClone = Instantiate(gameObject, transform.parent);
            draggedClone.name = gameObject.name + "_CLONE"; // Mark as clone with different name

            Debug.Log($"[Drag] Clone created! Clone ID={draggedClone.GetInstanceID()}, Original ID={gameObject.GetInstanceID()}");
            Debug.Log($"[Drag] Original stays at position {rectTransform.localPosition}");
            Debug.Log($"[ORIGINAL] After clone - active={gameObject.activeSelf}, position={rectTransform.localPosition}, parent={transform.parent.name}");

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
            Debug.Log($"[ORIGINAL] OnPointerUp - active={gameObject.activeSelf}, position={rectTransform.localPosition}, parent={transform.parent.name}");

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

            Debug.Log($"[ORIGINAL] After forwarding to clone - active={gameObject.activeSelf}, position={rectTransform.localPosition}");
            return;
        }

        Debug.Log($"[CLONE] OnPointerUp called for '{ingredientName}' (ID: {gameObject.GetInstanceID()})");

        // Check if dropped on plate by raycasting at pointer position
        bool droppedOnPlate = false;
        UIPlateController foundPlate = null;

        // Perform a raycast at the pointer position to find all UI elements underneath
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = eventData.position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        Debug.Log($"[Drag] Raycast found {raycastResults.Count} objects at drop position");

        // Check all raycasted objects for a UIPlateController
        foreach (RaycastResult result in raycastResults)
        {
            Debug.Log($"[Drag]   - Raycast hit: {result.gameObject.name} (ID: {result.gameObject.GetInstanceID()})");

            UIPlateController plate = result.gameObject.GetComponent<UIPlateController>();
            if (plate != null)
            {
                Debug.Log($"[Drag] ✓ Found plate on '{result.gameObject.name}'!");
                foundPlate = plate;
                droppedOnPlate = true;
                break;
            }
        }

        // If plate found, drop the ingredient on it
        if (droppedOnPlate && foundPlate != null)
        {
            Debug.Log($"[Drag] Calling plate.OnDrop for ingredient '{ingredientName}'");

            // CRITICAL FIX: eventData.pointerDrag points to the ORIGINAL, not the clone!
            // We need to replace it with the clone (this.gameObject) before calling OnDrop
            GameObject originalPointerDrag = eventData.pointerDrag;
            eventData.pointerDrag = this.gameObject; // Set to clone

            Debug.Log($"[Drag] Fixed pointerDrag: was '{originalPointerDrag.name}' (ID:{originalPointerDrag.GetInstanceID()}), now '{eventData.pointerDrag.name}' (ID:{eventData.pointerDrag.GetInstanceID()})");

            foundPlate.OnDrop(eventData);
        }
        else
        {
            Debug.LogWarning($"[Drag] No plate found at drop position for '{ingredientName}'");
        }

        // If not dropped on plate, destroy the clone
        if (!droppedOnPlate)
        {
            Debug.Log($"[Drag] Ingredient '{ingredientName}' not dropped on plate - destroying clone");
            Destroy(gameObject);
        }
    }

    // Mark this clone as being placed on the plate
    public void MarkAsPlacedOnPlate()
    {
        Debug.Log($"[Drag] MarkAsPlacedOnPlate called for '{ingredientName}' - disabling raycasts");

        // Disable dragging
        enabled = false;

        // Disable ALL Graphic components' raycast targets so they don't interfere with future drags
        UnityEngine.UI.Graphic[] graphics = GetComponentsInChildren<UnityEngine.UI.Graphic>(true);
        foreach (UnityEngine.UI.Graphic graphic in graphics)
        {
            graphic.raycastTarget = false;
            Debug.Log($"[Drag]   - Disabled raycast on {graphic.gameObject.name} ({graphic.GetType().Name})");
        }
    }
}