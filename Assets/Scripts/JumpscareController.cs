using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class JumpscareController : MonoBehaviour
{
    // Public variables for Inspector assignment
    public Image vampire1Image; // 50% sinirli karakter (sakin)
    public GameObject vampire2Object; // 100% sinirli karakter (jumpscare)
    public AudioSource jumpscareAudio;
    public string nextSceneName = "order01"; // Sonraki sahne (order01)

    [Header("Video Jumpscare (Optional)")]
    public bool useVideoJumpscare = false; // Toggle between sprite and video
    public VideoPlayerController videoPlayerController; // Reference to video player

    // TextMeshProUGUI variable
    public TextMeshProUGUI failedText;

    // Adjustable timers
    public float fadeInTime = 2f; // Sakin karakter belirme suresi
    public float waitBeforeScareTime = 0.5f; // Jumpscare oncesi kisa bekleme
    public float scareDisplayTime = 9.5f; // Jumpscare ekranda kalma suresi (sesler icin uzatildi)
    public float textDisplayTime = 1.5f; // "YOU FAILED" text gosterme suresi
    public float waitBeforeNextScene = 0.5f; // order01'e gecmeden once bekleme suresi
    // TOPLAM SURE: 2 + 0.5 + 9.5 + 1.5 + 0.5 = 14 saniye

    void Start()
    {
        // Karakter gorsellerini guncelle (dinamik olarak)
        UpdateCharacterSprites();

        // Hide Text object at start using Alpha value (not using SetActive)
        if (failedText != null)
        {
            Color c = failedText.color;
            c.a = 0f; // Hide by setting Alpha to 0
            failedText.color = c;
        }
        else
        {
            Debug.LogError("ERROR: Failed Text not assigned in Inspector!");
        }

        // Basarisiz siparis olarak kaydet
        GameFlowManager.Instance.OnOrderFailed();

        StartCoroutine(JumpscareRoutine());
    }

    /// <summary>
    /// Karakter gorsellerini ve video klibini gunceller (50% ve 100% sinirli haller + jumpscare video)
    /// </summary>
    private void UpdateCharacterSprites()
    {
        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;

        // TEST: Eger currentCharacter null ise, veritabanindan rastgele karakter sec
        if (currentCharacter == null)
        {
            Debug.LogWarning("[JumpscareController] currentCharacter null! Rastgele karakter seciliyor (TEST MODE)");

            if (GameFlowManager.Instance.characterDatabase != null)
            {
                currentCharacter = GameFlowManager.Instance.characterDatabase.GetRandomCharacter();
                GameFlowManager.Instance.currentCharacter = currentCharacter;
            }

            if (currentCharacter == null)
            {
                Debug.LogError("[JumpscareController] Hala currentCharacter null! Iptal ediliyor.");
                return;
            }
        }

        // 50% sinirli hali (vampire1Image)
        if (vampire1Image != null)
        {
            vampire1Image.sprite = currentCharacter.angry50Sprite;
            Debug.Log($"[JumpscareController] 50% sinirli gorsel yuklendi: {currentCharacter.characterName}");
        }
        else
        {
            Debug.LogError("[JumpscareController] vampire1Image null!");
        }

        // 100% sinirli hali (vampire2Object)
        if (vampire2Object != null)
        {
            Image vampire2Image = vampire2Object.GetComponent<Image>();
            if (vampire2Image != null)
            {
                vampire2Image.sprite = currentCharacter.angry100Sprite;
                Debug.Log($"[JumpscareController] 100% sinirli gorsel yuklendi: {currentCharacter.characterName}");
            }
            else
            {
                Debug.LogError("[JumpscareController] vampire2Object'te Image component bulunamadi!");
            }
        }
        else
        {
            Debug.LogError("[JumpscareController] vampire2Object null!");
        }

        // Jumpscare video klibini ve chroma key ayarlarini ayarla
        if (useVideoJumpscare && videoPlayerController != null && currentCharacter.jumpscareVideo != null)
        {
            // Video klibini yukle
            videoPlayerController.videoPlayer.clip = currentCharacter.jumpscareVideo;

            // Karaktere ozel chroma key ayarlarini uygula
            videoPlayerController.keyColor = currentCharacter.greenScreenColor;
            videoPlayerController.threshold = currentCharacter.chromaThreshold;
            videoPlayerController.smoothness = currentCharacter.chromaSmoothness;
            videoPlayerController.despill = currentCharacter.chromaDespill;
            videoPlayerController.UpdateChromaKeySettings();

            Debug.Log($"[JumpscareController] Jumpscare video yuklendi: {currentCharacter.characterName} - {currentCharacter.jumpscareVideo.name}");
            Debug.Log($"[JumpscareController] Chroma key ayarlari: Threshold={currentCharacter.chromaThreshold}, Smoothness={currentCharacter.chromaSmoothness}, Despill={currentCharacter.chromaDespill}");
        }
        else if (useVideoJumpscare && videoPlayerController != null && currentCharacter.jumpscareVideo == null)
        {
            Debug.LogWarning($"[JumpscareController] {currentCharacter.characterName} icin jumpscare video atanmamis!");
        }
    }

    IEnumerator JumpscareRoutine()
    {
        float timer = 0f;

        // 1. VISUAL 1: Calm Vampire Slowly Appears (Fade In) - Skip if using video only
        if (!useVideoJumpscare || vampire1Image != null)
        {
            if (vampire1Image == null && !useVideoJumpscare)
            {
                Debug.LogError("Vampire 1 Image not assigned! Jumpscare cancelled.");
                yield break;
            }

            if (vampire1Image != null)
            {
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

                // 2. SHORT WAIT
                yield return new WaitForSeconds(waitBeforeScareTime);
            }
        }
        else
        {
            Debug.Log("[JumpscareController] Skipping vampire1Image fade-in, using video jumpscare only.");
        }

        // 3. JUMPSCARE MOMENT
        if (vampire1Image != null)
        {
            vampire1Image.gameObject.SetActive(false);
        }

        // Use video or sprite based on setting
        if (useVideoJumpscare && videoPlayerController != null)
        {
            Debug.Log("[JumpscareController] Video jumpscare baslatiyor...");

            // Play video jumpscare
            videoPlayerController.gameObject.SetActive(true);
            Debug.Log($"[JumpscareController] VideoDisplay aktif edildi. Active: {videoPlayerController.gameObject.activeSelf}");

            videoPlayerController.PlayVideo();
            Debug.Log("[JumpscareController] PlayVideo() cagirildi");

            if (jumpscareAudio != null)
            {
                jumpscareAudio.Play();
            }

            // Wait for video to finish or use the specified time
            float videoWaitTime = scareDisplayTime;
            Debug.Log($"[JumpscareController] {videoWaitTime} saniye bekleniyor...");
            yield return new WaitForSeconds(videoWaitTime);

            // Stop and hide video
            videoPlayerController.StopVideo();
            videoPlayerController.gameObject.SetActive(false);
            Debug.Log("[JumpscareController] Video durduruldu ve gizlendi");
        }
        else
        {
            // Use original sprite jumpscare
            if (vampire2Object != null)
            {
                vampire2Object.SetActive(true);
            }

            if (jumpscareAudio != null)
            {
                jumpscareAudio.Play();
            }

            // 4. JUMPSCARE END: Keep on screen for specified time
            yield return new WaitForSeconds(scareDisplayTime);

            // Hide sprite jumpscare
            if (vampire2Object != null)
            {
                vampire2Object.SetActive(false);
            }
        }

        // 4.5: Show "YOU FAILED" Text

        if (failedText != null)
        {
            failedText.text = "YOU FAILED";

            // Make text visible by setting Alpha to 1
            Color c = failedText.color;
            c.a = 1f;
            failedText.color = c;

            Debug.Log("Text and close commands applied.");

            yield return new WaitForSeconds(textDisplayTime);
        }
        else
        {
            Debug.LogError("FAILEDTEXT NULL! Inspector assignment is wrong!");
        }

        // 5. RETURN TO ORDER SCENE
        Debug.Log($"[JumpscareController] {waitBeforeNextScene} saniye sonra {nextSceneName} sahnesine donuluyor...");
        yield return new WaitForSeconds(waitBeforeNextScene);

        SceneManager.LoadScene(nextSceneName);
    }
}