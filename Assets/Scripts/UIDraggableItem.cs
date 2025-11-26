using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        // YENİ: Sesi, sürükleme olayı başlamadan hemen önce çal!
        CalSesiOynat();

        // If this is the original ingredient (not a clone), create a clone to drag
        if (!isClone)
        {
            // Create a clone
            draggedClone = Instantiate(gameObject, transform.parent);
            draggedClone.name = gameObject.name; // Keep same name (remove "(Clone)" suffix)

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

        // Check if dropped on plate
        bool droppedOnPlate = false;

        if (eventData.pointerEnter != null)
        {
            UIPlateController plate = eventData.pointerEnter.GetComponent<UIPlateController>();
            if (plate != null)
            {
                droppedOnPlate = true;
                // The plate's OnDrop will handle this
            }
        }

        // If not dropped on plate, destroy the clone
        if (!droppedOnPlate)
        {
            Destroy(gameObject);
        }
    }

    // Mark this clone as being placed on the plate
    public void MarkAsPlacedOnPlate()
    {
        // Disable dragging
        enabled = false;

        // Disable raycast target so it doesn't interfere with future drags
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.raycastTarget = false;
        }
    }
}