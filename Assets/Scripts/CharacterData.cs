using UnityEngine;
using UnityEngine.Video;

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

    [Header("Jumpscare Video")]
    [Tooltip("Karaktere ozel jumpscare video klibi (vampir.webm, palyaco.webm, etc.)")]
    public VideoClip jumpscareVideo;

    [Header("Chroma Key Settings (Her video icin farkli yesil ton)")]
    [Tooltip("Video'daki yesil ekranin rengi (eyedropper ile secin)")]
    public Color greenScreenColor = new Color(0, 1, 0, 1);

    [Tooltip("Ne kadar yesil kaldirilacak (0-1)")]
    [Range(0, 1)] public float chromaThreshold = 0.4f;

    [Tooltip("Kenar yumusakligi (0-1)")]
    [Range(0, 1)] public float chromaSmoothness = 0.1f;

    [Tooltip("Yesil renk dokulmesini kaldirma gucu (0-1)")]
    [Range(0, 1)] public float chromaDespill = 0.8f;
}
