using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Plays a sound effect when the UI element is clicked
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour, IPointerClickHandler
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Handles UI click event and plays sound
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudioClip();
    }

    /// <summary>
    /// Plays the audio clip (stops and restarts if already playing)
    /// </summary>
    private void PlayAudioClip()
    {
        if (audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }
}