using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public RawImage displayImage; // UI RawImage to display video
    public RenderTexture renderTexture;

    [Header("Chroma Key Settings")]
    public Material chromaKeyMaterial; // Assign the material with ChromaKey shader
    public Color keyColor = new Color(0, 1, 0, 1); // Green by default
    [Range(0, 1)] public float threshold = 0.4f;
    [Range(0, 1)] public float smoothness = 0.1f;
    [Range(0, 1)] public float despill = 0.5f; // Green spill removal strength

    [Header("Playback Settings")]
    public bool playOnStart = false;
    public bool loopVideo = false;

    private void Start()
    {
        SetupVideoPlayer();
        UpdateChromaKeySettings();

        if (playOnStart)
        {
            PlayVideo();
        }
    }

    private void SetupVideoPlayer()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned!");
            return;
        }

        // Setup render texture
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1920, 1080, 0);
        }

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.isLooping = loopVideo;
        videoPlayer.playOnAwake = false;

        // Apply render texture to RawImage
        if (displayImage != null)
        {
            displayImage.texture = renderTexture;

            // Only apply chroma key material if assigned (optional)
            if (chromaKeyMaterial != null)
            {
                displayImage.material = chromaKeyMaterial;
                Debug.Log("[VideoPlayerController] Chroma key material applied");
            }
            else
            {
                Debug.Log("[VideoPlayerController] No chroma key material - video will show with green background");
            }
        }
    }

    public void UpdateChromaKeySettings()
    {
        // Update the material INSTANCE used by displayImage, not the original material
        if (displayImage != null && displayImage.material != null)
        {
            displayImage.material.SetColor("_KeyColor", keyColor);
            displayImage.material.SetFloat("_Threshold", threshold);
            displayImage.material.SetFloat("_Smoothness", smoothness);
            displayImage.material.SetFloat("_Despill", despill);

            Debug.Log($"[VideoPlayerController] Chroma key updated - KeyColor: {keyColor}, Threshold: {threshold}, Smoothness: {smoothness}, Despill: {despill}");
        }
        else
        {
            Debug.LogWarning("[VideoPlayerController] Cannot update chroma key - displayImage or material is null!");
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            Debug.Log($"[VideoPlayerController] Video playing with chroma key... Clip: {videoPlayer.clip?.name}, IsPlaying: {videoPlayer.isPlaying}");

            if (displayImage != null)
            {
                Debug.Log($"[VideoPlayerController] DisplayImage active: {displayImage.gameObject.activeSelf}, Color: {displayImage.color}");
            }

            if (chromaKeyMaterial != null)
            {
                Debug.Log($"[VideoPlayerController] ChromaKey Material assigned. Threshold: {threshold}, Smoothness: {smoothness}");
            }
            else
            {
                Debug.LogWarning("[VideoPlayerController] ChromaKey Material is NULL!");
            }
        }
        else
        {
            Debug.LogError("[VideoPlayerController] VideoPlayer is NULL!");
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

    public void PauseVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Pause();
        }
    }

    public bool IsPlaying()
    {
        return videoPlayer != null && videoPlayer.isPlaying;
    }

    public bool HasFinished()
    {
        if (videoPlayer == null) return true;
        return !videoPlayer.isLooping && videoPlayer.frame >= (long)videoPlayer.frameCount - 1;
    }

    // Update chroma key settings in real-time (useful for testing in Inspector)
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateChromaKeySettings();
        }
    }
}
