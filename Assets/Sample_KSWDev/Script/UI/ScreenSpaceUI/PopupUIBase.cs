using UnityEngine;

public abstract class PopupUIBase : ScreenSpaceUIBase
{
    public override void Show()
    {
        base.Show();
        
        // 팝업 스택에 자신을 추가
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.PushPopup(this);
        }
    }

    public override void Hide()
    {
        base.Hide();
        
        // 팝업 스택에서 자신을 제거
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.PopPopup(this);
        }
    }

    // ESC 키 등으로 팝업을 닫을 때 호출될 함수 (필요 시 오버라이드)
    public virtual void ClosePopup()
    {
        Hide();
    }
}