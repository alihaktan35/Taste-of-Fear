using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class KarakterBelirme : MonoBehaviour
{
    // Inspector'da ayarlanacak deðiþkenler:
    public float belirmeSuresi = 3.0f; // Karakterin görünme süresi

    // Konuþma Balonu Deðiþkenleri
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

        // Baþlangýçta tamamen görünmez yap
        renk.a = 0f;
        karakterImage.color = renk;

        // Belirme iþlemini baþlat
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

        // Belirme bittiðinde, tam görünür olduðundan emin ol
        renk.a = 1f;
        karakterImage.color = renk;

        // Konuþma Balonunu göster fonksiyonunu çaðýr
        KonusmaBalonuGoster();
    }

    void KonusmaBalonuGoster()
    {
        // ÖNEMLÝ: Baðlantýlarýn Kontrolü (Hala atanmamýþsa Log Error verir)
        if (konusmaBalonuObjesi == null || metinAlani == null)
        {
            Debug.LogError("Konuþma balonu veya metin alaný Inspector'da atanmadý!");
            return;
        }

        string secilenMetin = "";

        if (rastgeleMetinler.Length == 0)
        {
            // Metin dizisi boþsa, sadece uyarý ver ve metni boþ býrak
            Debug.LogWarning("Rastgele metin dizisi Inspector'da boþ býrakýldý. Konuþma balonu boþ gösteriliyor.");
            secilenMetin = ""; // <<-- BURASI DEÐÝÞTÝRÝLDÝ ("Selam..." yerine boþ string)
        }
        else
        {
            // Dizi doluysa, rastgele metin seç
            int rastgeleIndex = Random.Range(0, rastgeleMetinler.Length);
            secilenMetin = rastgeleMetinler[rastgeleIndex];
        }

        // Balonu görünür yap ve metni ata
        konusmaBalonuObjesi.SetActive(true);
        metinAlani.text = secilenMetin;
    }
}