using UnityEngine;

namespace SWUtility.UIManager
{
    public abstract class HUDUI : UIBase
    {
        // HUD는 Page나 Popup처럼 스택(Stack) 기반으로 관리되지 않고
        // 독립적으로 활성화/비활성화되는 상시 노출 UI에 사용됩니다.
    }
}
