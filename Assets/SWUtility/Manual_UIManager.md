# Manual: UIManager

`UIManager`는 게임 내 UI를 Page, Popup, HUD 세 가지 타입으로 분류하여 스택(Stack) 기반으로 관리하는 강력한 프레임워크입니다.

## 🎯 UI 타입 설명
1. **PageUI:** 전체 화면을 덮는 네비게이션 단위입니다. (예: 메인 화면, 로비 화면) 스택으로 관리되어 이전 페이지로 돌아가는 처리가 쉽습니다.
2. **PopupUI:** 기존 화면 위에 겹쳐서 뜨는 모달 창입니다. (예: 확인 창, 설정 창) 중첩이 가능하며 최상단 팝업부터 닫힙니다.
3. **HUDUI:** 체력바, 미니맵 등 스택의 영향을 받지 않고 항상 떠있어야 하는 UI입니다.

## 🛠️ 사용 방법

### 1. UI 스크립트 작성
UI 프리팹의 최상단(Canvas가 있는 곳)에 붙일 스크립트를 작성합니다. 목적에 맞게 `PageUI`, `PopupUI`, `HUDUI` 중 하나를 상속받습니다.

```csharp
using SWUtility.UIManager;
using UnityEngine;

public class SettingsPopup : PopupUI
{
    public override void Initialize()
    {
        // UI 내부 버튼 리스너 등록 등의 초기화 작업
    }

    public void OnClickClose()
    {
        ClosePopup(); // 팝업 닫기
    }
}
```

### 2. UI 띄우고 닫기
`UIManager.Instance.ShowUI<T>()`를 사용하면 매니저가 자동으로 스택을 계산하여 화면에 띄워줍니다. (해당 씬에 해당 프리팹이 미리 배치되어 있거나 Instantiate 되어 있어야 합니다.)

```csharp
// 특정 팝업 띄우기
UIManager.Instance.ShowUI<SettingsPopup>();

// 특정 팝업 숨기기
UIManager.Instance.HideUI<SettingsPopup>();

// [유용한 기능] 현재 켜져있는 가장 위의 팝업 닫기 (ESC 키 구현에 적합)
UIManager.Instance.CloseTopPopup();
```

## 💡 고급 기능 (MoveFrame)
`PageUI`를 상속받은 스크립트는 인스펙터에서 `Use Move Frame`을 체크할 수 있습니다. 이를 체크하고 `MoveFrame()` 함수를 오버라이드하면, 해당 페이지가 최상단에 있을 때만 매니저가 매 프레임 업데이트를 호출해 줍니다. (성능 최적화에 유리함)
