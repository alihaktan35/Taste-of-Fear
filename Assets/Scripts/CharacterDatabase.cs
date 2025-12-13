using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tum karakterlerin listesini tutan veritabani
/// </summary>
[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Game/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [Header("All Characters")]
    [Tooltip("Oyundaki tum karakterler (Golge, Hayalet, Palyaco, Vampir, Zombie)")]
    public List<CharacterData> allCharacters = new List<CharacterData>();

    /// <summary>
    /// Rastgele bir karakter secer
    /// </summary>
    public CharacterData GetRandomCharacter()
    {
        if (allCharacters == null || allCharacters.Count == 0)
        {
            Debug.LogError("[CharacterDatabase] Karakter listesi bos! Karakterleri ekleyin.");
            return null;
        }

        int randomIndex = Random.Range(0, allCharacters.Count);
        CharacterData selectedCharacter = allCharacters[randomIndex];

        Debug.Log($"[CharacterDatabase] Random karakter secildi: {selectedCharacter.characterName}");

        return selectedCharacter;
    }

    /// <summary>
    /// Isme gore karakter bulur
    /// </summary>
    public CharacterData GetCharacterByName(string characterName)
    {
        return allCharacters.Find(character => character.characterName == characterName);
    }
}
