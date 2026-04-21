# SWUtility 프레임워크 메뉴얼

`SWUtility`는 범용적으로 재사용 가능하도록 설계된 Unity 유틸리티 매니저 모음입니다. 각 매니저들은 결합도를 낮추고 모듈화되어 있으며, `GlobalSingleton` 기반으로 동작합니다.

각 매니저에 대한 상세한 작동 원리와 사용법(예제 코드)은 아래의 개별 메뉴얼 문서를 참고해 주세요.

---

## 1. Global Singleton (`SWUtility.Singleton`)
모든 전역 매니저의 기반(Base)이 되는 클래스입니다. 씬(Scene) 로드 시 파괴되지 않는 영구적인(DontDestroyOnLoad) 싱글톤 인스턴스를 보장합니다.
👉 **상세 메뉴얼:** [Manual_GlobalSingleton.md](Manual_GlobalSingleton.md)

## 2. UIManager (`SWUtility.UIManager`)
게임 내 UI 상태(HUD, Page, Popup)를 체계적으로 관리하는 스택(Stack) 기반 UI 프레임워크입니다.
👉 **상세 메뉴얼:** [Manual_UIManager.md](Manual_UIManager.md)

## 3. ObjectPoolManager (`SWUtility.ObjectPool`)
메모리 단편화 및 프레임 드랍을 방지하기 위해 생성/파괴가 빈번한 오브젝트를 미리 생성하고 재사용하는 풀링 시스템입니다.
👉 **상세 메뉴얼:** [Manual_ObjectPoolManager.md](Manual_ObjectPoolManager.md)

## 4. SoundManager (`SWUtility.SoundManager`)
2D 사운드(BGM, UI 클릭음)와 3D 인게임 사운드를 통합 관리하는 시스템입니다.
👉 **상세 메뉴얼:** [Manual_SoundManager.md](Manual_SoundManager.md)

## 5. SceneLoadManager (`SWUtility.SceneLoadManager`)
비동기 씬 로딩 및 초기화 타이밍(코루틴)을 정밀하게 제어하는 씬 생명주기 관리 시스템입니다.
👉 **상세 메뉴얼:** [Manual_SceneLoadManager.md](Manual_SceneLoadManager.md)

## 6. InputManager (`SWUtility.InputManager`)
New Input System을 사용하면서도 레거시 Input과 100% 동일한 사용감을 제공하는 직관적인 래퍼(Wrapper) 시스템입니다. 액션 맵 스위칭(모드 전환)을 강력하게 지원합니다.
👉 **상세 메뉴얼:** [Manual_InputManager.md](Manual_InputManager.md)

## 7. LocalizationManager (`SWUtility.Localization`)
CSV 기반의 다국어 데이터 관리 및 실시간 언어 변경 기능을 제공하는 시스템입니다. TextMeshPro와 연동하여 코드 없이 다국어 UI를 구축할 수 있습니다.
👉 **상세 메뉴얼:** [Manual_LocalizationManager.md](Manual_LocalizationManager.md)
