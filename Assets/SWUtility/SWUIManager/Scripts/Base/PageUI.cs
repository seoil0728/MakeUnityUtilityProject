using UnityEngine;

namespace SWUtility.UIManager
{
    public abstract class PageUI : UIBase
    {
        [Header("Update Settings")]
        [Tooltip("체크 시 매니저를 통해 매 프레임 MoveFrame()이 호출됩니다.")]
        [SerializeField] protected bool useMoveFrame = false;

        public bool UseMoveFrame => useMoveFrame;

        public override void Show()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PushPage(this);
            }
            else
            {
                SetVisible(true); 
            }
        }

        public override void Hide()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PopPage(this);
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
        }
    }
}
