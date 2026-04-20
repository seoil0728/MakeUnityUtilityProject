using UnityEngine;

public enum SoundPlayMode
{
    Random,
    Sequential
}

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Audio/Sound Data (3D)")]
public class SoundDataSO : ScriptableObject
{
    [Header("Audio Clips")]
    public AudioClip[] clips;
    
    [Tooltip("Random: 무작위 재생 / Sequential: 배열 순서대로 재생")]
    public SoundPlayMode playMode = SoundPlayMode.Random;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1.0f;
    [Range(0.5f, 1.5f)] public float minPitch = 0.95f;
    [Range(0.5f, 1.5f)] public float maxPitch = 1.05f;

    [Header("3D Settings")]
    public float maxDistance = 20f;

    // 순차 재생을 위한 내부 인덱스 (직렬화하지 않음)
    [System.NonSerialized] private int currentIndex = 0;

    // 게임 시작 시 (또는 에디터에서 플레이 버튼 누를 때) 인덱스 초기화
    private void OnEnable()
    {
        currentIndex = 0;
    }

    // 외부에서 클립을 가져갈 때 호출하는 함수
    public AudioClip GetClip()
    {
        if (clips == null || clips.Length == 0) return null;

        if (playMode == SoundPlayMode.Random)
        {
            return clips[Random.Range(0, clips.Length)];
        }
        else // Sequential
        {
            AudioClip selectedClip = clips[currentIndex];
            // 인덱스를 증가시키고, 배열 끝에 도달하면 다시 0으로 순환
            currentIndex = (currentIndex + 1) % clips.Length; 
            return selectedClip;
        }
    }

    public float GetRandomPitch() => Random.Range(minPitch, maxPitch);
}