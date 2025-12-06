using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class JumpscareController : MonoBehaviour
{
    // Public variables for Inspector assignment
    public Image vampire1Image;
    public GameObject vampire2Object;
    public AudioSource jumpscareAudio;
    public string mainMenuSceneName = "startMenu";

    // TextMeshProUGUI variable
    public TextMeshProUGUI failedText;

    // Adjustable timers
    public float fadeInTime = 2f;
    public float waitBeforeScareTime = 0.5f;
    public float scareDisplayTime = 6.0f;
    public float textDisplayTime = 1.0f;

    void Start()
    {
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

        StartCoroutine(JumpscareRoutine());
    }

    IEnumerator JumpscareRoutine()
    {
        float timer = 0f;

        // 1. VISUAL 1: Calm Vampire Slowly Appears (Fade In)
        if (vampire1Image == null)
        {
            Debug.LogError("Vampire 1 Image not assigned! Jumpscare cancelled.");
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

        // 2. SHORT WAIT
        yield return new WaitForSeconds(waitBeforeScareTime);

        // 3. JUMPSCARE MOMENT
        vampire1Image.gameObject.SetActive(false);

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

        // 4.5: Show "YOU FAILED" Text

        if (failedText != null)
        {
            // NEW ADDITION: Close Vampire 2 and show text
            if (vampire2Object != null)
            {
                vampire2Object.SetActive(false); // <--- VAMPIRE 2 CLOSED!
            }

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

        // Scene will stay here. (Or you can add scene transition from here if you want)
    }
}