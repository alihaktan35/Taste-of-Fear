using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class JumpscareController : MonoBehaviour
{
    // Inspector'da atama yapmak için public deðiþkenler
    public Image vampire1Image;
    public GameObject vampire2Object;
    public AudioSource jumpscareAudio;
    public string mainMenuSceneName = "startMenu";

    // TextMeshProUGUI tipinde deðiþken
    public TextMeshProUGUI failedText;

    // Ayarlanabilir zamanlayýcýlar
    public float fadeInTime = 2f;
    public float waitBeforeScareTime = 0.5f;
    public float scareDisplayTime = 6.0f;
    public float textDisplayTime = 1.0f;

    void Start()
    {
        // Baþlangýçta Text nesnesini Alpha deðeri ile gizle (SetActive kullanmýyoruz)
        if (failedText != null)
        {
            Color c = failedText.color;
            c.a = 0f; // Alpha'yý 0 yaparak gizle
            failedText.color = c;
        }
        else
        {
            Debug.LogError("HATA: Failed Text Inspector'da atanmamýþ!");
        }

        StartCoroutine(JumpscareRoutine());
    }

    IEnumerator JumpscareRoutine()
    {
        float timer = 0f;

        // 1. GÖRSEL 1: Sakin Vampirin Yavaþça Görünmesi (Fade In)
        if (vampire1Image == null)
        {
            Debug.LogError("Vampire 1 Image atanmadý! Jumpscare iptal ediliyor.");
            yield break;
        }

        Color color = vampire1Image.color;

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            vampire1Image.color = color;
            yield return null;
        }

        color.a = 1f;
        vampire1Image.color = color;

        // 2. KISA BEKLEME
        yield return new WaitForSeconds(waitBeforeScareTime);

        // 3. JUMPSCARE ANI
        vampire1Image.gameObject.SetActive(false);

        if (vampire2Object != null)
        {
            vampire2Object.SetActive(true);
        }

        if (jumpscareAudio != null)
        {
            jumpscareAudio.Play();
        }

        // 4. JUMPSCARE SONU: Belirtilen süre kadar ekranda tut
        yield return new WaitForSeconds(scareDisplayTime);

        // 4.5: "YOU FAILED" Yazýsýný Göster

        if (failedText != null)
        {
            // YENÝ EKLEME: Vampire 2'yi kapat ve metni göster
            if (vampire2Object != null)
            {
                vampire2Object.SetActive(false); // <--- VAMPÝRE 2 KAPATILDI!
            }

            failedText.text = "YOU FAILED";

            // Text'i Alpha'yý 1 yaparak görünür yap
            Color c = failedText.color;
            c.a = 1f;
            failedText.color = c;

            Debug.Log("Yazý ve kapanýþ komutlarý uygulandý.");

            yield return new WaitForSeconds(textDisplayTime);
        }
        else
        {
            Debug.LogError("FAILEDTEXT NULL! Inspector atamasý yanlýþ!");
        }

        // Sahne burada kalacak. (Veya isterseniz buradan sahne geçiþi yapabilirsiniz)
    }
}