using UnityEngine;

public class TargetButton : InteractableObject
{
    [Header("Visual Feedback")]
    [SerializeField] private GameObject selectionOutline; // 선택 시 활성화될 테두리
    [SerializeField] private Transform buttonModel;      // 애니메이션 효과를 줄 모델 트랜스폼
    
    private Vector3 idlePos;
    private Vector3 activePos;
    [SerializeField] private float activeElevation = -0.2f; // 선택 시 높이 변화 값

    private void Start()
    {
        if (buttonModel == null) buttonModel = transform;
        
        idlePos = buttonModel.localPosition;
        activePos = idlePos + Vector3.up * activeElevation;

        OnDeselect();
    }

    public override void OnHoverEnter() { }
    public override void OnHoverExit() { }

    public override void OnClick()
    {
        // SelectionManager를 통해 이 마커를 선택 상태로 전환
        SelectionManager.Instance.SelectTargetButton(this);
        
        base.OnClick();
    }

    public override void OnLongClick() { }

    public override void OnSelect()
    {
        base.OnSelect();
        if (selectionOutline != null) selectionOutline.SetActive(true);
        buttonModel.localPosition = activePos;
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        if (selectionOutline != null) selectionOutline.SetActive(false);
        buttonModel.localPosition = idlePos;
    }
}
