using UnityEngine;

namespace SWUtility.UIManager
{
    public abstract class PopupUI : UIBase
    {
        public override void Show()
        {
            base.Show();
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PushPopup(this);
            }
        }

        public override void Hide()
        {
            base.Hide();
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PopPopup(this);
            }
        }

        public virtual void ClosePopup()
        {
            Hide();
        }
    }
}
