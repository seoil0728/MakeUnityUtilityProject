using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class ScreenSpaceUIBase : MonoBehaviour
{
    protected Canvas uiCanvas;

    protected virtual void Awake()
    {
        uiCanvas = GetComponentInChildren<Canvas>();
    }

    protected virtual void Start()
    {
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.RegisterUI(this);
        }
        
        Initialize();
    }

    protected virtual void OnDestroy()
    {
        if (ScreenSpaceUIManager.Instance != null)
        {
            ScreenSpaceUIManager.Instance.UnregisterUI(this);
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