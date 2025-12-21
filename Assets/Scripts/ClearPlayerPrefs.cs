using UnityEngine;

/// <summary>
/// TEMPORARY SCRIPT - Clears all PlayerPrefs data
/// USAGE: Attach to any GameObject, run Play mode, then DELETE this script
/// </summary>
public class ClearPlayerPrefs : MonoBehaviour
{
    private void Start()
    {
        Debug.LogWarning("🗑️ CLEARING ALL PLAYERPREFS...");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.LogWarning("✅ ALL PLAYERPREFS CLEARED!");
        Debug.LogWarning("⚠️ NOW DELETE THIS SCRIPT!");
    }
}
