using UnityEngine;
using UnityEngine.UI; // UI öðeleri için
using System.Collections; // Coroutine'ler için
using TMPro; // TextMeshPro kullanýyorsanýz

public class ClownDialogueManager : MonoBehaviour
{
    public GameObject dialogueBubble; // Konuþma balonu GameObject'i (Image)
    public TMP_Text dialogueText;    // Konuþma metni TextMeshPro nesnesi
    public float moveSpeed = 2f;     // Karakterin hareket hýzý
    public float stopPositionX = 0f; // Karakterin duracaðý X koordinatý
    public float startPositionX = -5f; // Karakterin baþlayacaðý X koordinatý
    public float endPositionX = 5f;    // Karakterin geri döneceði X koordinatý
    public float dialogueDisplayTime = 3f; // Konuþma balonunun ekranda kalma süresi

    private Vector3 initialPosition; // Karakterin baþlangýç pozisyonu
    private bool isMoving = false;
    private bool dialogueActive = false;

    void Start()
    {

        initialPosition = transform.position;
        dialogueBubble.SetActive(false);
        transform.position = new Vector3(startPositionX, initialPosition.y, initialPosition.z);
        StartScenario(); // Oyun baþladýðýnda otomatik baþlat

        initialPosition = transform.position; // Mevcut pozisyonu baþlangýç olarak kaydet
        dialogueBubble.SetActive(false); // Baþlangýçta konuþma balonunu gizle
        transform.position = new Vector3(startPositionX, initialPosition.y, initialPosition.z); // Baþlangýç pozisyonuna ayarla
    }

    public void StartScenario()
    {
        if (!isMoving && !dialogueActive)
        {
            StartCoroutine(ScenarioRoutine());
        }
    }

    IEnumerator ScenarioRoutine()
    {
        isMoving = true;
        // Palyaçoyu durma pozisyonuna hareket ettir
        Vector3 targetPos = new Vector3(stopPositionX, transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;

        // Diyalogu göster
        dialogueBubble.SetActive(true);
        dialogueText.text = "Merhaba! Ne güzel bir gün deðil mi?"; // Diyalog metnini buraya yaz
        dialogueActive = true;
        yield return new WaitForSeconds(dialogueDisplayTime);
        dialogueBubble.SetActive(false);
        dialogueActive = false;

        // Palyaçoyu geri hareket ettir
        isMoving = true;
        Vector3 returnTargetPos = new Vector3(endPositionX, transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, returnTargetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnTargetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
        // Ýstersen baþlangýç pozisyonuna geri dönmesini saðlayabilirsin
        // transform.position = initialPosition;
    }
}