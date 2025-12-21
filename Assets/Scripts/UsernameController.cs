using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls username input in Options menu
/// Uses Legacy UI (InputField, Text)
/// </summary>
public class UsernameController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private InputField usernameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject usernamePanel;

    private void Start()
    {
        // Load existing username
        if (UsernameManager.Instance.HasUsername())
        {
            usernameInputField.text = UsernameManager.Instance.GetUsername();
        }

        // Setup button
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (saveButton != null)
        {
            saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        }
    }

    private void OnSaveButtonClicked()
    {
        string username = usernameInputField.text.Trim();

        if (UsernameManager.Instance.IsValidUsername(username))
        {
            UsernameManager.Instance.SetUsername(username);
            Debug.Log($"Username saved: {username}");

            // Optional: Show confirmation feedback
        }
        else
        {
            Debug.LogWarning("Invalid username");
            // Optional: Show error message
        }
    }
}
