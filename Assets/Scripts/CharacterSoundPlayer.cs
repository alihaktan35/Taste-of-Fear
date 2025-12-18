using UnityEngine;

/// <summary>
/// Plays character-specific greeting sounds when characters enter the order scene
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class CharacterSoundPlayer : MonoBehaviour
{
    [Header("Character Greeting Sounds")]
    [Tooltip("Golge (Shadow) greeting sound")]
    public AudioClip golgeGreeting;

    [Tooltip("Hayalet (Ghost) greeting sound")]
    public AudioClip hayaletGreeting;

    [Tooltip("Palyaco (Clown) greeting sound")]
    public AudioClip palyacoGreeting;

    [Tooltip("Vampir (Vampire) greeting sound")]
    public AudioClip vampirGreeting;

    [Tooltip("Zombie greeting sound")]
    public AudioClip zombieGreeting;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the greeting sound for the current character
    /// </summary>
    public void PlayCharacterGreeting()
    {
        if (GameFlowManager.Instance == null)
        {
            Debug.LogError("[CharacterSoundPlayer] GameFlowManager.Instance is null!");
            return;
        }

        CharacterData currentCharacter = GameFlowManager.Instance.currentCharacter;

        if (currentCharacter == null)
        {
            Debug.LogError("[CharacterSoundPlayer] currentCharacter is null!");
            return;
        }

        AudioClip soundToPlay = GetCharacterGreetingSound(currentCharacter.characterName);

        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
            Debug.Log($"[CharacterSoundPlayer] Playing greeting sound for: {currentCharacter.characterName}");
        }
        else
        {
            Debug.LogWarning($"[CharacterSoundPlayer] No greeting sound assigned for: {currentCharacter.characterName}");
        }
    }

    /// <summary>
    /// Returns the appropriate greeting sound based on character name
    /// </summary>
    private AudioClip GetCharacterGreetingSound(string characterName)
    {
        switch (characterName.ToLower())
        {
            case "golge":
                return golgeGreeting;

            case "hayalet":
                return hayaletGreeting;

            case "palyaco":
                return palyacoGreeting;

            case "vampir":
                return vampirGreeting;

            case "zombie":
                return zombieGreeting;

            default:
                Debug.LogWarning($"[CharacterSoundPlayer] Unknown character name: {characterName}");
                return null;
        }
    }
}
