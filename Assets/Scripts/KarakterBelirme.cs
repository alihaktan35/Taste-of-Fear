using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class KarakterBelirme : MonoBehaviour
{
    // Inspector'da ayarlanacak değişkenler:
    public float belirmeSuresi = 3.0f; // Karakterin görünme süresi

    // Konuşma Balonu Değişkenleri
    public GameObject konusmaBalonuObjesi;
    public TMP_Text metinAlani;

    [TextArea]
    public string[] rastgeleMetinler; // Inspector'da buraya metinlerinizi gireceksiniz


    private Image karakterImage;
    private Color renk;

    void Start()
    {
        karakterImage = GetComponent<Image>();

        renk = karakterImage.color;

        // Başlangıçta tamamen görünmez yap
        renk.a = 0f;
        karakterImage.color = renk;

        // Belirme işlemini başlat
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float gecenZaman = 0f;

        while (gecenZaman < belirmeSuresi)
        {
            float alfaOrani = gecenZaman / belirmeSuresi;

            renk.a = alfaOrani;
            karakterImage.color = renk;

            gecenZaman += Time.deltaTime;

            yield return null;
        }

        // Belirme bittiğinde, tam görünür olduğundan emin ol
        renk.a = 1f;
        karakterImage.color = renk;

        // Konuşma Balonunu göster fonksiyonunu çağır
        KonusmaBalonuGoster();
    }

    void KonusmaBalonuGoster()
    {
        // Bağlantıların Kontrolü
        if (konusmaBalonuObjesi == null || metinAlani == null)
        {
            Debug.LogError("Konuşma balonu veya metin alanı Inspector'da atanmadı! Lütfen bağlantıları kontrol edin.");
            return;
        }

        string secilenMetin = "";

        if (rastgeleMetinler.Length == 0)
        {
            // Metin dizisi boşsa, uyarı ver ve metni boş bırak
            Debug.LogWarning("Rastgele metin dizisi Inspector'da boş bırakıldı! Konuşma balonu boş gösteriliyor.");
            secilenMetin = "";
        }
        else
        {
            // Dizi doluysa, rastgele metin seç
            int rastgeleIndex = Random.Range(0, rastgeleMetinler.Length);
            secilenMetin = rastgeleMetinler[rastgeleIndex];
        }

        // Balonu görünür yap ve metni ata
        konusmaBalonuObjesi.SetActive(true); // <<-- BURASI ONU AÇIK TUTAN YER
        metinAlani.text = secilenMetin;

        // Bu fonksiyonda, konuşma balonunu kapatan (SetActive(false)) hiçbir kod yoktur.
        // Başka bir script kapatmıyorsa, açık kalacaktır.
    }
}