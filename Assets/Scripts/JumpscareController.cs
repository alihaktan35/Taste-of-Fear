using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JumpscareController : MonoBehaviour
{
    // Inspector'da atama yapmak için public deðiþkenler
    public Image vampire1Image; // Eskiden sakinPalyaçoImage
    public GameObject vampire2Object; // Eskiden sinirliPalyaçoObject
    public AudioSource jumpscareAudio;
    public string mainMenuSceneName = "MainMenu"; // Ana menü sahnenizin adý

    // Ayarlanabilir zamanlayýcýlar
    public float fadeInTime = 2f;         // Sakin vampirin görünme süresi (Fade-in)
    public float waitBeforeScareTime = 0.5f; // Gerginlik için beklenen kýsa süre
    public float scareDisplayTime = 1.5f;   // Jumpscare'in ekranda kalma süresi

    void Start()
    {
        // Baþlangýçta Jumpscare Rutinini baþlat
        StartCoroutine(JumpscareRoutine());
    }

    IEnumerator JumpscareRoutine()
    {
        // 1. GÖRSEL 1: Sakin Vampirin Yavaþça Görünmesi (Fade In)
        float timer = 0f;
        Color color = vampire1Image.color;

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            // Alpha deðerini 0'dan 1'e doðru yavaþça artýr
            color.a = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            vampire1Image.color = color;
            yield return null; // Bir sonraki frame'e geç
        }

        // Alpha deðerinin tam 1 olduðundan emin ol
        color.a = 1f;
        vampire1Image.color = color;

        // 2. KISA BEKLEME: Gerginliði artýrma
        yield return new WaitForSeconds(waitBeforeScareTime);

        // 3. JUMPSCARE ANI

        // A. Görsel 1'i kapat ve Görsel 2'yi aç
        vampire1Image.gameObject.SetActive(false);
        vampire2Object.SetActive(true);

        // B. Sesi çal
        jumpscareAudio.Play();

        // C. Ekraný dondurma (Opsiyonel)
        // float originalTimeScale = Time.timeScale;
        // Time.timeScale = 0f;
        // yield return new WaitForSecondsRealtime(0.05f); 
        // Time.timeScale = originalTimeScale;

        // 4. JUMPSCARE SONU: Belirtilen süre kadar ekranda tut
        yield return new WaitForSeconds(scareDisplayTime);

        // 5. SAHNE GEÇÝÞÝ: Ana Menü'ye dön
        SceneManager.LoadScene(mainMenuSceneName);
    }
}