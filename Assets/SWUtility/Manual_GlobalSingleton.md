# Manual: GlobalSingleton

`GlobalSingleton<T>`는 SWUtility 프레임워크의 모든 전역 매니저들의 기반이 되는 추상 클래스입니다.

## 🎯 주요 기능
- **DontDestroyOnLoad 보장:** 씬(Scene)이 변경되어도 파괴되지 않고 유지됩니다.
- **자동 인스턴스화:** 씬에 해당 매니저가 없으면, `Resources` 폴더에서 동명의 프리팹을 찾아 스폰하거나 빈 오브젝트를 생성하여 자동으로 부착합니다.
- **명시적 초기화:** `IsInitialized` 플래그와 `Initialize()` 가상 함수를 통해 매니저의 초기화 시점을 명확하게 제어할 수 있습니다.

## 🛠️ 사용 방법 (새로운 매니저 만들기)

새로운 전역 매니저를 만들려면 `GlobalSingleton<T>`를 상속받기만 하면 됩니다.

```csharp
using UnityEngine;
using SWUtility.Singleton;

public class MyDataManager : GlobalSingleton<MyDataManager>
{
    public int playerScore = 0;

    // (선택) 초기화 로직이 필요할 경우 오버라이드
    public override void Initialize()
    {
        if (IsInitialized) return;
        
        Debug.Log("데이터 매니저 초기화 됨!");
        playerScore = 100;
        
        base.Initialize(); // IsInitialized를 true로 만들어줌
    }
}
```

### 호출 방법
다른 스크립트에서 언제든지 `Instance`를 통해 접근할 수 있습니다.
```csharp
int score = MyDataManager.Instance.playerScore;
```

## ⚠️ 주의 사항
- 프리팹으로 미리 세팅해두고 싶다면, 매니저 클래스 이름과 정확히 일치하는 이름(예: `MyDataManager.prefab`)으로 프리팹을 만들어 `Resources` 폴더 안에 넣어두세요.
- 가급적 게임이 시작되는 첫 씬(Boot 씬 등)에서 각 매니저들의 `Initialize()`를 명시적으로 한 번씩 호출해 주는 것이 흐름 제어에 좋습니다.
