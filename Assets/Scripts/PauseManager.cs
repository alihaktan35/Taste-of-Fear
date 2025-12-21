using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

/// <summary>
/// Manages pause functionality for the table01 cooking scene.
/// Handles ESC key toggle, time control, audio pause, and UI animations.
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject blurOverlay;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;

    private bool isPaused = false;
    private bool isTransitioning = false;

    private void Start()
    {
        // Ensure pause UI is hidden at start
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        // Toggle pause with ESC key
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Pauses the game, stops time and audio, shows pause UI
    /// </summary>
    public void PauseGame()
    {
        if (isPaused || isTransitioning) return;

        isPaused = true;
        StartCoroutine(PauseTransition());
    }

    /// <summary>
    /// Resumes the game, restores time and audio, hides pause UI
    /// </summary>
    public void ResumeGame()
    {
        if (!isPaused || isTransitioning) return;

        isPaused = false;
        StartCoroutine(ResumeTransition());
    }

    /// <summary>
    /// Returns to main menu (no confirmation dialog)
    /// </summary>
    public void QuitToMainMenu()
    {
        if (isTransitioning) return;

        StartCoroutine(QuitTransition());
    }

    private IEnumerator PauseTransition()
    {
        isTransitioning = true;

        // Show pause UI
        pausePanel.SetActive(true);
        canvasGroup.blocksRaycasts = true;

        // Fade in animation
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time since Time.timeScale will be 0
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Pause game
        Time.timeScale = 0f;
        AudioListener.pause = true;

        isTransitioning = false;
    }

    private IEnumerator ResumeTransition()
    {
        isTransitioning = true;

        // Resume game immediately
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Fade out animation
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        // Hide pause UI
        pausePanel.SetActive(false);

        isTransitioning = false;
    }

    private IEnumerator QuitTransition()
    {
        isTransitioning = true;

        // Fade out faster
        float quitFadeDuration = fadeDuration * 0.5f;
        float elapsed = 0f;
        while (elapsed < quitFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / quitFadeDuration);
            yield return null;
        }

        // Restore time before scene change
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Load main menu
        SceneManager.LoadScene("startMenu");
    }

    private void OnDestroy()
    {
        // Safety: ensure time scale is restored when scene unloads
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
