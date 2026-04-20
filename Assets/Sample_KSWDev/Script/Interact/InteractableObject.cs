using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler 
{
    [Header("Audio Settings")]
    [Tooltip("이 오브젝트를 클릭할 때 재생할 사운드입니다. 인스펙터에서 개별 변경이 가능합니다.")]
    [SerializeField] protected UISFXType clickSound = UISFXType.ButtonClick;
    
    public bool IsSelected { get; protected set; }
    protected bool isPointerDown = false;
    private float downTime;
    private const float LongClickThreshold = 2.0f;

    public virtual void OnHoverEnter()
    {
        
    }

    public virtual void OnHoverExit()
    {
        
    }

    public virtual void OnClick()
    {
        if (clickSound == UISFXType.None)
            return;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUI(clickSound);
        }
    }

    public virtual void OnLongClick()
    {
        
    }

    public virtual void OnSelect() { IsSelected = true; }
    public virtual void OnDeselect() { IsSelected = false; }

    public void OnPointerEnter(PointerEventData eventData) => OnHoverEnter();
    public void OnPointerExit(PointerEventData eventData) { isPointerDown = false; OnHoverExit(); }
    public void OnPointerDown(PointerEventData eventData) 
    { 
        if (eventData.button != PointerEventData.InputButton.Left) return;
        isPointerDown = true; 
        downTime = Time.time; 
    }
    public void OnPointerUp(PointerEventData eventData) 
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (isPointerDown && Time.time - downTime < LongClickThreshold) OnClick();
        isPointerDown = false;
    }

    protected virtual void Update() 
    {
        if (isPointerDown && Time.time - downTime >= LongClickThreshold) 
        { 
            OnLongClick(); 
            isPointerDown = false; 
        }
    }
}