using UnityEngine;

/// <summary>
/// Bir karakterin tum gorsellerini tutan ScriptableObject
/// Her karakter icin: siparis verme, bekleme, 50% sinirli, 100% sinirli halleri
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    [Tooltip("Karakter adi (Golge, Hayalet, Palyaco, Vampir, Zombie)")]
    public string characterName;

    [Header("Character Sprites")]
    [Tooltip("Siparis verme hali (order01 sahnesi icin)")]
    public Sprite orderSprite;

    [Tooltip("Siparis bekleme hali (successful sahnesi icin)")]
    public Sprite waitingSprite;

    [Tooltip("50% sinirli hali (jumpscare sahnesi icin)")]
    public Sprite angry50Sprite;

    [Tooltip("100% sinirli hali (jumpscare sahnesi icin)")]
    public Sprite angry100Sprite;
}
