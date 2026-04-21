# Manual: SoundManager

`SoundManager`는 게임 내의 2D BGM 및 UI 효과음과 3D 인게임 효과음을 효율적으로 관리하고 재생해 주는 통합 시스템입니다.

## 🎯 구조 설명
- **2D 사운드 (BGM, UI):** 카메라의 위치와 무관하게 일정한 볼륨으로 들리는 소리입니다. `SoundEnums.cs`에 정의된 Enum 타입을 키워드로 사용합니다.
- **3D 사운드 (InGame):** 폭발음, 발소리 등 발생 위치에 따라 볼륨과 좌우 패닝이 달라지는 소리입니다. `SoundDataSO` (ScriptableObject)를 에셋으로 만들어 관리하며, `ObjectPoolManager`와 연계되어 동적으로 생성/반환됩니다.

## 🛠️ 사용 방법

### 1. Audio Mixer 연동 (선택적)
`SoundManager`는 **Audio Mixer** 연동을 선택적으로 지원합니다.
1. 인스펙터의 `Audio Mixer (Optional)` 섹션에 믹서 그룹(`BGM`, `UI SFX`, `SFX`)을 할당합니다.
2. 각 믹서에서 볼륨 파라미터를 노출(Expose)시키고, 노출된 파라미터 이름(예: `BGM_Vol`)을 인스펙터에 입력합니다.
3. 믹서 그룹을 할당하지 않고 비워두면 기존의 `AudioSource.volume` 제어 방식으로 자동으로 폴백(Fallback)되어 동작합니다.

### 2. 2D 사운드 세팅 및 재생
1. `Assets/SWUtility/SoundManager/Scripts/SoundEnums.cs` 파일에 재생할 BGM 및 UI 사운드 이름을 추가합니다.
2. `Resources` 폴더에 생성해 둔 `SoundManager` 프리팹의 인스펙터 창에서, 각 Enum 타입에 맞는 `AudioClip`을 할당해 줍니다.
3. 코드에서 다음과 같이 호출합니다.

```csharp
// 1초 동안 부드럽게 크로스페이드(Cross-fade)되며 BGM 재생
SoundManager.Instance.PlayBGM(BGMType.InGame_Normal, 1.0f);

// UI 버튼 클릭음 즉시 재생
SoundManager.Instance.PlayUI(UISFXType.ButtonClick);
```

### 3. 볼륨 조절 API 및 SettingManager 연동
`SoundManager`는 볼륨(0.0 ~ 1.0)을 동적으로 조절하는 공용 함수를 제공합니다. **결합도를 낮추기 위해 `SettingManager` 코드를 직접 참조하지 않으므로**, 프로젝트에 `SettingManager`가 있다면 별도의 연동 스크립트를 작성하여 다음과 같이 연결해 주어야 합니다.

```csharp
// 예시: GameRoot 또는 GameLogic 초기화 스크립트에서 연동
private void Start()
{
    if (SettingManager.Instance != null && SoundManager.Instance != null)
    {
        // SettingManager의 볼륨 변경 이벤트를 SoundManager의 적용 함수와 연결
        SettingManager.Instance.AddFloatListener("BGM_Volume", SoundManager.Instance.SetBgmVolume);
        SettingManager.Instance.AddFloatListener("SFX_Volume", SoundManager.Instance.SetSfxVolume);
        
        // 시작 시 초기 설정값 적용
        SoundManager.Instance.SetBgmVolume(SettingManager.Instance.GetFloat("BGM_Volume", 1.0f));
        SoundManager.Instance.SetSfxVolume(SettingManager.Instance.GetFloat("SFX_Volume", 1.0f));
    }
}
```

### 4. 3D 사운드 세팅 및 재생
1. 프로젝트 창에서 우클릭 > `Create` > `Audio` > `Sound Data (3D)`를 선택하여 에셋을 생성합니다.
2. 해당 에셋 인스펙터에서 사운드 클립(여러 개 넣으면 랜덤 재생 가능), 볼륨, 피치 변화폭, 최대 거리(Max Distance)를 설정합니다.
3. 코드에서 재생할 위치와 함께 호출합니다. 매니저가 오디오 소스를 풀링 스폰하여 재생 후, 클립 길이를 계산해 자동으로 반환(Despawn) 시켜줍니다.

```csharp
public SoundDataSO explosionSound; // 인스펙터에서 할당

void Explode()
{
    // 지정된 위치에서 3D 사운드 재생
    SoundManager.Instance.Play3D(explosionSound, transform.position);
}
```

## ⚠️ 필수 요구 사항
1. **ObjectPoolManager 연동:** 3D 사운드는 동적으로 오디오 소스를 생성하고 반환하기 위해 `SWUtility.ObjectPool` 매니저를 필수로 사용합니다. 씬에 `ObjectPoolManager`가 존재해야 정상 작동합니다.
2. **프리팹 할당:** 3D 사운드를 사용하려면 `SoundManager` 프리팹의 `Audio Source Prefab` 슬롯에, `AudioSource` 컴포넌트가 달린 기본 프리팹을 연결해 두어야 합니다.
