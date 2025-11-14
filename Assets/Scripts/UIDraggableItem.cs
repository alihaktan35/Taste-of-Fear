using UnityEngine;
using UnityEngine.EventSystems; // UI olaylarý için bu namespace'i ekleyin

public class UIDraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform; // Sürüklenen UI elementinin RectTransform'u
    private Canvas canvas; // Üzerinde bulunduðu Canvas
    private Vector2 pointerOffset; // Týklanan yer ile nesne pivot noktasý arasýndaki fark

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Ebeveynindeki Canvas'ý bul
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Týklanan fare konumu ile nesnenin pivot noktasý arasýndaki farký hesapla
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );
        // Nesneyi en üste getir (diðer UI elemanlarýnýn önüne)
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nesneyi fare ile sürükle
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
        {
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Sürükleme bittiðinde yapýlacak bir þey varsa buraya yazýlýr
        // Örneðin, tabaða düþürüldüðünü kontrol etmek için
    }
}