using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GlobalSingleton을 상속받아 씬에 없어도 Resources 폴더에서 알아서 프리팹을 찾아 스폰합니다.
public class SoundManager : GlobalSingleton<SoundManager>
{
    [Header("2D Sound Settings (Enum)")]
    [SerializeField] private float masterBgmVolume = 0.5f;
    [SerializeField] private float masterUIVolume = 1.0f;

    [Serializable] public struct BGMSetup { public BGMType type; public AudioClip clip; }
    [Serializable] public struct UISFXSetup { public UISFXType type; public AudioClip clip; }

    [SerializeField] private BGMSetup[] bgmSetups;
    [SerializeField] private UISFXSetup[] uiSfxSetups;

    private Dictionary<BGMType, AudioClip> bgmDict = new Dictionary<BGMType, AudioClip>();
    private Dictionary<UISFXType, AudioClip> uiSfxDict = new Dictionary<UISFXType, AudioClip>();

    // 내부 2D AudioSource 채널
    private AudioSource[] bgmSources = new AudioSource[2];
    private int activeBgmIndex = 0;
    private Coroutine bgmFadeCoroutine;
    private AudioSource uiSfxSource;

    [Header("3D Sound Settings (Object Pool)")]
    [Tooltip("3D 사운드를 재생할 때 원본으로 복사될 오디오 소스 프리팹입니다.")]
    [SerializeField] private AudioSource audioSourcePrefab;


    public override void Initialize()
    {
        if (IsInitialized) return;
        
        // 1. 2D 딕셔너리 세팅
        foreach (var b in bgmSetups) if (!bgmDict.ContainsKey(b.type)) bgmDict.Add(b.type, b.clip);
        foreach (var u in uiSfxSetups) if (!uiSfxDict.ContainsKey(u.type)) uiSfxDict.Add(u.type, u.clip);

        // 2. 2D BGM 소스 동적 생성 (핑퐁 교차재생용 2개)
        for (int i = 0; i < 2; i++)
        {
            bgmSources[i] = gameObject.AddComponent<AudioSource>();
            bgmSources[i].loop = true;
            bgmSources[i].playOnAwake = false;
            bgmSources[i].spatialBlend = 0f; // 완벽한 2D
            bgmSources[i].volume = 0f;
        }

        // 3. 2D UI 소스 동적 생성
        uiSfxSource = gameObject.AddComponent<AudioSource>();
        uiSfxSource.playOnAwake = false;
        uiSfxSource.spatialBlend = 0f;
        
        base.Initialize();
    }

    // ==========================================
    // --- 외부 호출 API : 2D BGM ---
    // ==========================================
    public void PlayBGM(BGMType type, float fadeDuration = 1.0f)
    {
        if (type == BGMType.None || !bgmDict.TryGetValue(type, out AudioClip clip)) return;
        if (bgmSources[activeBgmIndex].clip == clip) return; // 이미 재생 중이면 무시

        if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);
        
        int nextBgmIndex = 1 - activeBgmIndex;
        bgmFadeCoroutine = StartCoroutine(CrossFadeBGM(clip, bgmSources[activeBgmIndex], bgmSources[nextBgmIndex], fadeDuration));
        activeBgmIndex = nextBgmIndex;
    }

    private IEnumerator CrossFadeBGM(AudioClip newClip, AudioSource fadeOutSrc, AudioSource fadeInSrc, float duration)
    {
        fadeInSrc.clip = newClip;
        fadeInSrc.volume = 0f;
        fadeInSrc.Play();

        float currentTime = 0f;
        float startFadeOutVol = fadeOutSrc.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeOutSrc.volume = Mathf.Lerp(startFadeOutVol, 0f, currentTime / duration);
            fadeInSrc.volume = Mathf.Lerp(0f, masterBgmVolume, currentTime / duration);
            yield return null;
        }

        fadeOutSrc.Stop();
        fadeInSrc.volume = masterBgmVolume;
        bgmFadeCoroutine = null;
    }

    // ==========================================
    // --- 외부 호출 API : 2D UI SFX ---
    // ==========================================
    public void PlayUI(UISFXType type)
    {
        if (type == UISFXType.None || !uiSfxDict.TryGetValue(type, out AudioClip clip)) return;
        
        uiSfxSource.PlayOneShot(clip, masterUIVolume);
    }

    // ==========================================
    // --- 외부 호출 API : 3D 인게임 사운드 (SO + 풀링) ---
    // ==========================================
    public void Play3D(SoundDataSO soundData, Vector3 position)
    {
        if (soundData == null || audioSourcePrefab == null) return;
        
        // 이전 단계에서 수정한 GetClip() 호출 (랜덤 혹은 순차 재생)
        AudioClip clip = soundData.GetClip(); 
        if (clip == null) return;

        // 1. ObjectPoolManager를 통해 스폰! ("Sounds" 카테고리 폴더 밑으로 들어갑니다)
        AudioSource source = ObjectPoolManager.Instance.Spawn<AudioSource>(
            audioSourcePrefab, 
            position, 
            Quaternion.identity, 
            "Sounds"
        );

        // 2. SO 데이터를 바탕으로 AudioSource 설정 세팅
        source.clip = clip;
        source.volume = soundData.volume;
        source.pitch = soundData.GetRandomPitch();
        source.spatialBlend = 1.0f; // 3D 사운드
        source.maxDistance = soundData.maxDistance;
        source.rolloffMode = AudioRolloffMode.Logarithmic;

        // 3. 재생
        source.Play();

        // 4. 피치를 고려하여 실제 재생 길이를 계산한 뒤, ObjectPoolManager에게 자동 반환(Despawn)을 예약합니다.
        float duration = clip.length / Mathf.Max(0.1f, Mathf.Abs(source.pitch));
        ObjectPoolManager.Instance.Despawn(source.gameObject, duration + 0.1f);
    }
}