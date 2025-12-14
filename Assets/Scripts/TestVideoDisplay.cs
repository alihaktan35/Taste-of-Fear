using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Quick test script to verify video display without chroma key
/// Attach this to an empty GameObject and assign references in Inspector
/// </summary>
public class TestVideoDisplay : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UnityEngine.UI.RawImage rawImage;
    public RenderTexture renderTexture;

    void Start()
    {
        // Simple test without chroma key
        if (videoPlayer != null && rawImage != null)
        {
            // Make sure the RawImage is visible
            rawImage.gameObject.SetActive(true);
            rawImage.color = Color.white;

            // Setup video player
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = renderTexture;

            // Assign texture to RawImage (without material for testing)
            rawImage.texture = renderTexture;
            rawImage.material = null; // Temporarily remove material to test

            // Play video
            videoPlayer.Play();

            Debug.Log("[TestVideoDisplay] Video should now be visible without chroma key");
            Debug.Log($"[TestVideoDisplay] Video clip: {videoPlayer.clip?.name}");
            Debug.Log($"[TestVideoDisplay] RawImage active: {rawImage.gameObject.activeSelf}");
            Debug.Log($"[TestVideoDisplay] RenderTexture: {renderTexture?.name}");
        }
        else
        {
            Debug.LogError("[TestVideoDisplay] Missing references!");
        }
    }
}
