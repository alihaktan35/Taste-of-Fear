using UnityEngine;
using UnityEngine.UI; // Image bileþenini kullanmak için gerekli!
using System.Collections; // Coroutine (zamanla iþlem) için gerekli

public class KarakterBelirme : MonoBehaviour
{
    // Inspector'da ayarlanacak deðiþken:
    public float belirmeSuresi = 2.0f; // Belirmenin ne kadar süreceði (saniye)

    private Image karakterImage;
    private Color renk;

    void Start()
    {
        // 1. Karakterin Image bileþenini al
        karakterImage = GetComponent<Image>();

        // 2. Baþlangýç rengini kaydet
        renk = karakterImage.color;

        // 3. Karakteri baþlangýçta tamamen görünmez yap (Alfa = 0)
        renk.a = 0f;
        karakterImage.color = renk;

        // 4. Belirme iþlemini baþlat
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float gecenZaman = 0f;

        // Geçen zaman, belirme süresinden az olduðu sürece devam et
        while (gecenZaman < belirmeSuresi)
        {
            // Oraný hesapla (0.0'dan 1.0'a doðru artacak)
            float alfaOrani = gecenZaman / belirmeSuresi;

            // Yeni alfa deðerini ata
            renk.a = alfaOrani;
            karakterImage.color = renk;

            // Zamaný ilerlet
            gecenZaman += Time.deltaTime;

            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Belirme bittiðinde, tam görünür olduðundan emin ol (alfa=1)
        renk.a = 1f;
        karakterImage.color = renk;

        Debug.Log("Karakter tamamen belirdi.");
    }

    
}