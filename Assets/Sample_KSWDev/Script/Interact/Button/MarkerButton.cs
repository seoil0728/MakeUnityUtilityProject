using UnityEngine;

public class MarkerButton : InteractableObject
{
    [Header("Marker Settings")]
    [SerializeField] private TileType markerType; // 이 버튼이 담당하는 마커 종류 (L, R, T, O)

    [Header("Visual Feedback")]
    [SerializeField] private GameObject selectionOutline; // 선택 시 활성화될 테두리
    [SerializeField] private Transform markerModel;      // 애니메이션 효과를 줄 모델 트랜스폼
    
    private Vector3 idlePos;
    private Vector3 activePos;
    [SerializeField] private float activeElevation = -0.3f; // 선택 시 높이 변화 값

    private void Start()
    {
        if (markerModel == null) markerModel = transform;
        
        idlePos = markerModel.localPosition;
        activePos = idlePos + Vector3.up * activeElevation;

        OnDeselect();
    }

    public override void OnHoverEnter() { }
    public override void OnHoverExit() { }

    public override void OnClick()
    {
        // SelectionManager를 통해 이 마커를 선택 상태로 전환
        SelectionManager.Instance.SelectMarkerButton(markerType, this);
        
        base.OnClick();
    }

    public override void OnLongClick() { }

    public override void OnSelect()
    {
        base.OnSelect();
        if (selectionOutline != null) selectionOutline.SetActive(true);
        markerModel.localPosition = activePos;
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        if (selectionOutline != null) selectionOutline.SetActive(false);
        markerModel.localPosition = idlePos;
    }
}