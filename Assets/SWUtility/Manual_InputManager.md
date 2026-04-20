# Manual: InputManager

`InputManager`는 Unity의 **New Input System 패키지**를 레거시(구형) `Input` 클래스처럼 직관적이고 쉽게 사용할 수 있게 감싸주는(Wrapper) 정적(Static) 기반 프레임워크입니다.

## 🎯 주요 기능
- **정적 호출 (Polling):** `SWInput.GetButtonDown("Jump")` 와 같이 Update문 안에서 매우 직관적으로 입력 상태를 체크할 수 있습니다.
- **자동 에셋 캐싱:** C# 코드를 수정할 필요 없이, 유니티 에디터 상에서 `.inputactions` 파일에 액션을 추가하기만 하면 곧바로 코드에서 문자열 키워드로 호출 가능합니다.
- **액션 맵(Action Map) 스위칭:** `게임 모드`와 `UI 팝업 모드`를 완벽히 분리하여, UI 조작 중에 캐릭터가 엉뚱하게 움직이는 현상을 막아줍니다.

## 🛠️ 사전 세팅 (중요!)
1. 유니티 패키지 매니저에서 **Input System**이 설치되어 있어야 합니다.
2. `Resources` 폴더 안에 **`PlayerControls.inputactions`** 라는 이름으로 Input Action 에셋을 만드세요.
3. 이 에셋 안에 **`Player`** 라는 이름의 Action Map(게임용)과 **`UI`** 라는 이름의 Action Map(메뉴 조작용)을 생성하고 원하는 키를 바인딩하세요.

## 🛠️ 사용 방법

### 1. 코드에서 입력 받기 (`SWInput`)
`Update` 함수 내부에서 레거시 방식과 똑같이 사용하면 됩니다.

```csharp
using SWUtility.InputManager;
using UnityEngine;

void Update()
{
    // 단발성 클릭 (키보드 띄어쓰기 등)
    if (SWInput.GetButtonDown("Jump")) { Jump(); }
    
    // 누르고 있는 동안 지속 (마우스 클릭 유지 등)
    if (SWInput.GetButton("Fire")) { Shoot(); }
    
    // 손가락을 뗐을 때
    if (SWInput.GetButtonUp("Fire")) { StopShooting(); }

    // 방향 이동 (WASD 조합, 조이스틱 등) -> Vector2 반환
    Vector2 moveDir = SWInput.GetVector2("Move");
    transform.Translate(moveDir * Time.deltaTime);
}
```

### 2. 모드 전환 (입력 차단)
UI 팝업이 열릴 때 캐릭터 입력을 막고, 닫힐 때 다시 켜주는 데 사용합니다.

```csharp
// 인벤토리를 열었을 때 호출
InputManager.Instance.SwitchToUIMode(); 
// -> 이 순간부터 SWInput.GetButtonDown("Jump")는 무조건 false를 반환합니다.

// 인벤토리를 닫았을 때 호출
InputManager.Instance.SwitchToGameMode(); 
// -> 다시 캐릭터 조작이 가능해집니다.
```

### 3. 컷씬 모드 (전체 차단)
모든 입력을 완전히 무시해야 할 때 사용합니다.

```csharp
InputManager.Instance.DisableInput(); // 조작 완전 불가
InputManager.Instance.EnableInput();  // 기본 게임 모드로 복귀
```
