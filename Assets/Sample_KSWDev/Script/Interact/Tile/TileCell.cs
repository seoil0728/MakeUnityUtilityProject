using UnityEngine;

public class TileCell : InteractableObject 
{
    public int x, y;
    
    [Header("Visuals")]
    [SerializeField] private GameObject[] ghostModels; // Order: L, R, T, O
    [SerializeField] private GameObject[] realModels;  // Order: L, R, T, O
    [SerializeField] private GameObject ghostTargetModel;
    [SerializeField] private GameObject realTargetModel;
    
    public void ClearTile() 
    {
        // 배치된 모든 모델 비활성화
        UpdateRealVisuals(TileType.Max, false);
        UpdateGhostModels(TileType.Max, false);
    }
    
    public override void OnHoverEnter() 
    {
        if (SelectionManager.Instance.CurrentSelection == SelectionType.MarkerButton) 
        {
            TileType type = SelectionManager.Instance.SelectedType;
            
            UpdateGhostModels(type, false);
        }
        else if (SelectionManager.Instance.CurrentSelection == SelectionType.TargetButton)
        {
            UpdateGhostModels(TileType.Max, true);
        }
    }

    public override void OnHoverExit() 
    {
        UpdateGhostModels(TileType.Max, false);
    }

    public override void OnClick() 
    {
        if (SelectionManager.Instance.CurrentSelection == SelectionType.MarkerButton) 
        {
            TileType type = SelectionManager.Instance.SelectedType;
            
            GameDirector.Instance.RemoveActualTarget();
            GameDirector.Instance.UpdateVirtualMap(x, y, type);
            UpdateRealVisuals(type, false);
        }
        else if (SelectionManager.Instance.CurrentSelection == SelectionType.TargetButton)
        {
            GameDirector.Instance.SetActualTarget(x, y, this);
            GameDirector.Instance.UpdateVirtualMap(x, y, TileType.T);
            UpdateRealVisuals(TileType.Max, true);
        }
        
        base.OnClick();
    }

    public override void OnLongClick() 
    {
        GameDirector.Instance.UpdateVirtualMap(x, y, TileType.O);
        UpdateRealVisuals(TileType.O, false);
    }

    private void UpdateGhostModels(TileType type, bool targetActive)
    {
        if (ghostTargetModel != null)
        {
            ghostTargetModel.SetActive(targetActive);    
        }
        
        int idx = GetModelIndex(type);

        if (idx == -1 || ghostModels.Length <= idx)
        {
            foreach (var m in ghostModels) m.SetActive(false);
            return;
        }
            
        ghostModels[idx].SetActive(true);   
    }

    private void UpdateRealVisuals(TileType type, bool targetActive) 
    {
        if (realTargetModel != null)
        {
            realTargetModel.SetActive(targetActive);
        }
        
        foreach (var m in realModels) m.SetActive(false);
        
        int idx = GetModelIndex(type);
        
        if (idx == -1 || realModels.Length <= idx)
        {
            return;
        }
        
        realModels[idx].SetActive(true);
    }

    private int GetModelIndex(TileType type) 
    {
        if (type == TileType.L) return 0;
        if (type == TileType.R) return 1;
        if (type == TileType.T) return 2;
        if (type == TileType.O) return 3;
        
        return -1;
    }

    private void Start()
    {
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.RegisterTile(x, y, this);
        }

        ClearTile();
    }
}