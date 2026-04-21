# Manual: SettingManager

`SettingManager`는 게임의 각종 옵션(사운드, 그래픽, 시스템 설정 등)을 통합 관리하고 기기에 영구적으로 저장하는 시스템입니다. **키-값(Key-Value) 기반의 이벤트 브로드캐스팅 아키텍처**를 사용하여 다른 매니저들과의 결합도를 극도로 낮추었습니다.

## 🎯 주요 기능
- **동적 키-값 저장소:** 하드코딩된 변수 없이 문자열 키(String Key)를 통해 런타임에 동적으로 설정을 추가하고 관리할 수 있습니다.
- **자동 직렬화:** 설정 데이터는 `LitJson`을 통해 JSON 형태로 `Application.persistentDataPath`에 자동 저장 및 로드됩니다.
- **이벤트 브로드캐스팅:** 특정 키의 설정값이 변경되면, 해당 키를 구독(Subscribe)하고 있는 모든 객체에 즉시 이벤트가 전달됩니다.
- **디커플링(Decoupling):** UI 시스템이나 타 매니저(`SoundManager`, `LocalizationManager`)를 직접 참조하지 않아, 사용자가 원하는 매니저만 선택적으로 프로젝트에 포함시킬 수 있습니다.

## 🛠️ 사용 방법

### 1. 설정값 저장 및 적용 (UI 등에서 호출)
UI 슬라이더, 토글, 드롭다운 등의 값이 변경되었을 때 `SettingManager`의 함수를 호출하여 값을 업데이트합니다. 값이 실제로 변경되었을 때만 이벤트가 발생합니다.

```csharp
// float 값 변경 (예: BGM 볼륨 슬라이더)
SettingManager.Instance.SetFloat("BGM_Volume", 0.5f);

// int 값 변경 (예: 언어 설정 드롭다운)
SettingManager.Instance.SetInt("Lang_Index", 1);

// bool 값 변경 (예: 전체화면 토글)
SettingManager.Instance.SetBool(SettingManager.KEY_FULLSCREEN, true);
```

### 2. 설정값 변경 이벤트 구독 (타 매니저에서 수신)
설정값의 변화에 반응해야 하는 시스템은 시작 시점(`Start`)에 이벤트를 구독하고, 현재 값을 가져와 초기화합니다.

```csharp
// SoundManager.cs 내부 예시
private void Start()
{
    if (SettingManager.Instance != null)
    {
        // 1. 이벤트 구독 (값이 바뀔 때마다 UpdateBgmVolume 호출)
        SettingManager.Instance.AddFloatListener("BGM_Volume", UpdateBgmVolume);
        
        // 2. 초기값 로드 및 적용
        float currentVol = SettingManager.Instance.GetFloat("BGM_Volume", 1.0f);
        UpdateBgmVolume(currentVol);
    }
}

private void UpdateBgmVolume(float volume)
{
    // AudioMixer 볼륨 조절 등 실제 로직 수행
}
```

### 3. 유니티 내장 시스템 제어
자주 사용되는 유니티 기본 설정(전체화면, 그래픽 품질, VSync)은 `SettingManager` 내부에 키가 상수로 정의되어 있으며, 매니저가 자체적으로 이벤트를 구독하여 엔진 설정을 제어합니다.

- `SettingManager.KEY_FULLSCREEN`: `Screen.fullScreen` 제어
- `SettingManager.KEY_QUALITY`: `QualitySettings.SetQualityLevel` 제어
- `SettingManager.KEY_VSYNC`: `QualitySettings.vSyncCount` 제어

이 값들은 다른 설정과 동일하게 `SetBool`이나 `SetInt`로 변경하면 엔진에 즉시 반영됩니다.

## ⚠️ 데이터 저장 시점
데이터는 성능을 위해 매번 값이 바뀔 때마다 파일에 쓰지 않고, 애플리케이션이 종료되거나(`OnApplicationQuit`) 백그라운드로 전환될 때(`OnApplicationPause`) 일괄적으로 저장(`SaveSettings`)됩니다. 강제로 저장하고 싶다면 언제든 `SaveSettings()` 메서드를 직접 호출할 수 있습니다.
