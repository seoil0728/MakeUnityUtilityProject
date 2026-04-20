using UnityEngine;

public abstract class ScreenUIBase : ScreenSpaceUIBase
{
    [Header("Update Settings")]
    [Tooltip("체크 시 매니저를 통해 매 프레임 MoveFrame()이 호출됩니다.")]
    [SerializeField] protected bool useMoveFrame = false;

    // 매니저에서 접근하기 위한 프로퍼티
    public bool UseMoveFrame => useMoveFrame;

    public override void Show()
    {
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.PushScreen(this);
        }
        else
        {
            SetVisible(true); 
        }
    }

    public override void Hide()
    {
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.PopScreen(this);
        }
        else
        {
            SetVisible(false); 
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (isVisible) base.Show();
        else base.Hide();
    }

    /// <summary>
    /// useMoveFrame이 true일 때, 매니저의 Update 루프에서 매 프레임 호출됩니다.
    /// 애니메이션, 타이머, 실시간 데이터 갱신 등의 로직을 구현하세요.
    /// </summary>
    public virtual void MoveFrame()
    {
        // 하위 클래스에서 필요 시 오버라이드하여 구현
    }
}