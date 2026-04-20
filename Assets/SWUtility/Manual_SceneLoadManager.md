# Manual: SceneLoadManager

`SceneLoadManager`는 유니티의 씬(Scene) 전환을 코루틴(Coroutine)을 통해 정밀하게 제어하고, 메인 씬과 서브 씬 간의 종속성을 관리해 주는 시스템입니다.

## 🎯 주요 기능
- **비동기 생명주기 관리:** 씬이 바뀔 때, 에셋 로드나 서버 통신 등 시간이 걸리는 작업이 끝날 때까지 씬 전환을 안전하게 대기할 수 있습니다. (`InitRoutine`, `UnInitRoutine`)
- **Main 씬 vs Additive 씬:** 완전히 새로운 스테이지로 넘어가는 Single 모드(Main)와, 기존 씬 위에 인벤토리나 배경을 덧씌우는 Additive 모드를 명확히 구분하여 로드합니다.
- **씬 간 데이터 전달:** `LoadMainScene` 호출 시 파라미터를 넘겨주어, 다음 씬에서 초기화 데이터(Payload)로 사용할 수 있습니다.
- **상태 제어 (Pause/Resume):** 팝업형 Additive 씬이 띄워졌을 때, 밑에 깔린 Main 씬의 시간을 멈추거나 입력을 차단하는 콜백을 자동으로 쏴줍니다.

## 🛠️ 사용 방법

### 1. Main 씬 구축
메인 씬으로 사용할 씬의 최상단 빈 게임오브젝트에 `MainSceneRoot`를 상속받은 스크립트를 붙입니다.

```csharp
using SWUtility.SceneLoadManager;

public class GameSceneRoot : MainSceneRoot
{
    public override void SetupContext(object payload)
    {
        base.SetupContext(payload);
        // 이전 씬에서 넘긴 데이터 파싱
    }

    public override IEnumerator InitRoutine()
    {
        // 맵 생성, 에셋 로딩 대기 등
        yield return new WaitForSeconds(1f);
    }

    public override void OnSceneReady()
    {
        // 화면 페이드 인, 게임 진짜 시작!
    }
}
```

### 2. 씬 호출
어디서든 매니저를 통해 씬을 비동기로 로드합니다. (로딩 스크린과 연동하기 좋습니다.)

```csharp
// "GameScene" 씬을 단독(Single) 로드하고 100이라는 데이터 전달
SceneLoadManager.Instance.LoadMainScene("GameScene", 100);

// "InventoryScene" 씬을 현재 화면 위에 겹쳐서(Additive) 로드
SceneLoadManager.Instance.LoadAdditiveScene("InventoryScene");

// 덧씌워진 씬 내리기
SceneLoadManager.Instance.UnloadAdditiveScene("InventoryScene");
```

### 3. Additive 씬 구축 및 상태 제어
배경에 깔릴 씬에는 `AdditiveSceneRoot`를 상속받은 스크립트를 붙입니다.

```csharp
public class InventorySceneRoot : AdditiveSceneRoot
{
    // 이 씬이 열려있으면 메인 씬의 OnPause() 콜백을 트리거합니다.
    public override bool ShouldPauseMainScene => true;

    protected override void OnMainSceneInjected()
    {
        // 부모 씬 참조 완료 (ParentMainScene 사용 가능)
    }

    public override IEnumerator InitRoutine() { yield return null; }
}
```
