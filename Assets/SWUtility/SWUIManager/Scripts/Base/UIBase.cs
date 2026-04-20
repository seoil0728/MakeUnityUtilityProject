using UnityEngine;

namespace SWUtility.UIManager
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIBase : MonoBehaviour
    {
        protected Canvas uiCanvas;

        protected virtual void Awake()
        {
            uiCanvas = GetComponent<Canvas>();
            if (uiCanvas == null) uiCanvas = GetComponentInChildren<Canvas>();
        }

        protected virtual void Start()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.RegisterUI(this);
            }
            
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UnregisterUI(this);
            }
        }

        public abstract void Initialize();

        public virtual void Show()
        {
            if (uiCanvas != null) uiCanvas.enabled = true;
        }

        public virtual void Hide()
        {
            if (uiCanvas != null) uiCanvas.enabled = false;
        }
    }
}
