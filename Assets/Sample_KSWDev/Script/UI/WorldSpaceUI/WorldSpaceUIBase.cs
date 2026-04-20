using UnityEngine;

public abstract class WorldSpaceUIBase : MonoBehaviour
{
    protected Canvas uiCanvas;

    protected virtual void Awake()
    {
        uiCanvas = GetComponentInChildren<Canvas>();
    }

    protected virtual void Start()
    {
        // 씬에 배치된 3D UI들이 시작될 때 매니저에 스스로를 등록
        if (WorldSpaceUIManager.Instance != null)
        {
            WorldSpaceUIManager.Instance.RegisterUI(this);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] WorldSpaceUIManager 인스턴스가 존재하지 않아 등록할 수 없습니다.");
        }
        
        Initialize();
    }

    protected virtual void OnDestroy()
    {
        if (WorldSpaceUIManager.Instance != null)
        {
            WorldSpaceUIManager.Instance.UnregisterUI(this);
        }
    }

    // 하위 클래스에서 반드시 구현해야 하는 초기화 로직
    public abstract void Initialize();

    // 기본 표시/숨김 로직 (Canvas 컴포넌트를 켜고 끄는 방식 권장)
    public virtual void Show()
    {
        if (uiCanvas != null) uiCanvas.enabled = true;
    }

    public virtual void Hide()
    {
        if (uiCanvas != null) uiCanvas.enabled = false;
    }
}