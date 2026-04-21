# SWUtility 개발 TODO 리스트

앞으로 `SWUtility` 프레임워크에 추가하고 개선할 기능들의 목록입니다.

## 🛠️ 신규 매니저 개발
- [x] **LocalizationManager (다국어 매니저) 개발**
  - CSV 또는 JSON 기반의 다국어 데이터 로드 및 파싱 기능.
  - 현재 언어 설정에 맞춰 텍스트(TextMeshPro 등) 및 이미지를 동적으로 변경하는 기능.
  - 에디터 상에서 쉽게 번역 키를 적용할 수 있는 헬퍼 컴포넌트 제공.

- [x] **SettingManager (환경 설정 매니저) 개발**
  - 해상도, 그래픽 품질, 게임 플레이 옵션 등을 관리하고 저장/불러오기(Save/Load) 하는 기능.
  - JSON 직렬화(Serialization)를 통한 로컬 데이터 저장 방식 적용.
  - `SoundManager` 및 `UIManager` 등 타 매니저와의 연동 구조 마련.

## 🚀 기존 매니저 개선
- [x] **SoundManager 업그레이드 (Audio Mixer 연동)**
  - 기존의 단순 `AudioSource.volume` 제어 방식에서 벗어나, Unity의 **Audio Mixer**를 통한 그룹별(Master, BGM, SFX) 볼륨 제어 기능 추가.
  - `SettingManager`와 연동하여 옵션 창의 슬라이더 조작 시 실시간으로 Audio Mixer 볼륨이 조절되도록 구현.
  - 음소거(Mute) 기능 및 볼륨 저장 기능 연동.
