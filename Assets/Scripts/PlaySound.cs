using UnityEngine;
using UnityEngine.EventSystems; // !!! Bu satırı ekleyin

[RequireComponent(typeof(AudioSource))] 
public class PlaySound : MonoBehaviour, IPointerClickHandler // !!! Bu arayüzü ekleyin
{
    private AudioSource sesKaynagi;

    void Start()
    {
        sesKaynagi = GetComponent<AudioSource>();
        // Debug.Log("Script başladı."); // Kontrol amaçlı
    }

    // ARTIK OnMouseDown() Kullanmıyoruz. Yerine IPointerClickHandler arayüzünden gelen bu fonksiyonu kullanıyoruz:
    public void OnPointerClick(PointerEventData eventData)
    {
        // Tıklandığında buraya düşüp düşmediğini kontrol et
        Debug.Log(gameObject.name + " TIKLANDI (UI Olayı)."); 
        
        CalSesiOynat();
    }

    private void CalSesiOynat() 
    {
        if (sesKaynagi != null)
        {
            if (sesKaynagi.isPlaying)
            {
                sesKaynagi.Stop();
            }
            sesKaynagi.Play(); 
        }
    }
}