using UnityEngine;

public class HintButton : InteractableObject 
{
    public int index;
    private LaserRenderSimulator laserRender;

    public override void OnHoverEnter() 
    {
        var result = GameDirector.Instance.SimulateVirtualPath(index);
        laserRender.DrawPath(result);
    }

    public override void OnHoverExit() 
    {
        laserRender.ClearPath();
    }

    public override void OnClick() 
    {
        SelectionManager.Instance.SelectHintButton(index, this);
        
        base.OnClick();
    }

    public override void OnLongClick() { }

    private void Start()
    {
        if (GameDirector.Instance == null)
            return;

        laserRender = GameDirector.Instance.GetLaserRenderer();
    }
}