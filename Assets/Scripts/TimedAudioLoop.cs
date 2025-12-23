using UnityEngine;
using System.Collections;

public class TimedAudioLoop : MonoBehaviour
{
    public AudioSource audioSource; // Ses kaynaðý
    public float waitTime = 3f;     // Bekleme süresi (3 saniye)

    void Start()
    {
        // Eðer AudioSource atanmadýysa bu objedekini bulmaya çalýþ
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Döngüyü baþlat
        StartCoroutine(PlayAudioWithDelay());
    }

    IEnumerator PlayAudioWithDelay()
    {
        while (true) // Sonsuz döngü
        {
            audioSource.Play(); // Sesi çal (1 saniye sürecek)

            // Sesin bitmesini bekle (1 saniye) + senin istediðin 3 saniye bekle
            // Toplamda sesin baþlangýcýndan diðer baþlangýca 4 saniye olur.
            // Eðer "ses bittikten sonra tam 3 saniye beklesin" dersen:
            yield return new WaitForSeconds(audioSource.clip.length + waitTime);
        }
    }
}