using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Localization; // Localization kütüphanesini ekledik

public class KarakterBelirme : MonoBehaviour
{
    // Inspector'da ayarlanacak değişkenler:
    public float belirmeSuresi = 3.0f;

    // Konuşma Balonu Değişkenleri
    public GameObject konusmaBalonuObjesi;
    public TMP_Text metinAlani;

    // string[] yerine LocalizedString[] kullanarak yerelleştirmeyi aktif ediyoruz.
    // Inspector'da Size kısmını 10 yaparak 10 farklı metin atayabilirsin.
    public LocalizedString[] rastgeleMetinler;

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

        renk.a = 1f;
        karakterImage.color = renk;

        // Konuşma Balonunu göster fonksiyonunu çağır
        KonusmaBalonuGoster();
    }

    void KonusmaBalonuGoster()
    {
        if (konusmaBalonuObjesi == null || metinAlani == null)
        {
            Debug.LogError("Konuşma balonu veya metin alanı Inspector'da atanmadı!");
            return;
        }

        string secilenMetin = "";

        // Eğer Inspector'da 10 tane LocalizedString atadıysan Length otomatik 10 olacaktır.
        if (rastgeleMetinler == null || rastgeleMetinler.Length == 0)
        {
            Debug.LogWarning("Rastgele metin dizisi boş!");
        }
        else
        {
            // 0 ile rastgeleMetinler.Length (yani 10) arasında bir sayı seçer
            int rastgeleIndex = Random.Range(0, rastgeleMetinler.Length);

            // Seçilen index'teki metni o anki sistem dilinde getirir
            secilenMetin = rastgeleMetinler[rastgeleIndex].GetLocalizedString();
        }

        konusmaBalonuObjesi.SetActive(true);
        metinAlani.text = secilenMetin;
    }
}