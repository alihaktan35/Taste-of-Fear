using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Manages character dialogue and movement for the clown character
/// </summary>
public class ClownDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueBubble;
    public TMP_Text dialogueText;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float stopPositionX = GameConstants.CHARACTER_CENTER_POSITION;
    public float startPositionX = GameConstants.CHARACTER_LEFT_POSITION;
    public float endPositionX = GameConstants.CHARACTER_RIGHT_POSITION;

    [Header("Dialogue Settings")]
    public float dialogueDisplayTime = 3f;

    private Vector3 initialPosition;
    private bool isMoving = false;
    private bool dialogueActive = false;

    void Start()
    {
        initialPosition = transform.position;
        dialogueBubble.SetActive(false);
        transform.position = new Vector3(startPositionX, initialPosition.y, initialPosition.z);
        StartScenario();
    }

    /// <summary>
    /// Starts the character movement and dialogue scenario
    /// </summary>
    public void StartScenario()
    {
        if (!isMoving && !dialogueActive)
        {
            StartCoroutine(ScenarioRoutine());
        }
    }

    /// <summary>
    /// Coroutine that handles character movement and dialogue display
    /// </summary>
    IEnumerator ScenarioRoutine()
    {
        // Move character to center position
        isMoving = true;
        Vector3 targetPos = new Vector3(stopPositionX, transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;

        // Show dialogue
        dialogueBubble.SetActive(true);
        dialogueText.text = "Merhaba! Ne güzel bir gün değil mi?";
        dialogueActive = true;
        yield return new WaitForSeconds(dialogueDisplayTime);
        dialogueBubble.SetActive(false);
        dialogueActive = false;

        // Move character back to end position
        isMoving = true;
        Vector3 returnTargetPos = new Vector3(endPositionX, transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, returnTargetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnTargetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}