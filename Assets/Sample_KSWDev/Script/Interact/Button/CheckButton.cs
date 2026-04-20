using UnityEngine;

public class CheckButton : InteractableObject
{
    [SerializeField] private Transform buttonModel;
    [SerializeField] private GameObject selectionOutline;
    private Vector3 idlePos;

    private void Start()
    {
        if (buttonModel == null) buttonModel = transform;
        idlePos = buttonModel.localPosition;
    }

    public override void OnHoverEnter()
    {
        if (selectionOutline == null)
            return;
        
        selectionOutline.SetActive(true);   
    }
    public override void OnHoverExit()
    {
        if (selectionOutline == null)
            return;
        
        selectionOutline.SetActive(false);
    }

    public override void OnClick()
    {
        // 시각적 피드백 연출
        StopAllCoroutines();
        StartCoroutine(ClickAnimation());

        // 로직 권한을 GameDirector에게 위임
        GameDirector.Instance.ExecuteCheck();
        SelectionManager.Instance.ResetSelection();

        base.OnClick();
    }

    private System.Collections.IEnumerator ClickAnimation()
    {
        buttonModel.localPosition = idlePos + Vector3.down * 0.1f;
        yield return new WaitForSeconds(0.1f);
        buttonModel.localPosition = idlePos;
    }

    public override void OnLongClick() { }
}