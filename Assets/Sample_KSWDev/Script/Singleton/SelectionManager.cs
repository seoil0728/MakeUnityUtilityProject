using UnityEngine;

public class SelectionManager : MonoBehaviour 
{
    public static SelectionManager Instance { get; private set; }
    
    public SelectionType CurrentSelection { get; private set; } = SelectionType.None;
    public TileType SelectedType { get; private set; } = TileType.O;
    public int SelectedHintIndex { get; private set; } = -1;

    private InteractableObject lastSelected;

    private void Awake() => Instance = this;
    
    private void OnEnable()
    {
        GameEvents.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= OnGameStarted;
    }

    public void SelectMarkerButton(TileType type, InteractableObject obj) 
    {
        SetSelection(obj);
        CurrentSelection = SelectionType.MarkerButton;
        SelectedType = type;
        
        GameEvents.OnSelectionChanged?.Invoke(CurrentSelection, SelectedType, SelectedHintIndex);
    }

    public void SelectHintButton(int index, InteractableObject obj) 
    {
        SetSelection(obj);
        CurrentSelection = SelectionType.HintButton;
        SelectedHintIndex = index;
        
        GameEvents.OnSelectionChanged?.Invoke(CurrentSelection, SelectedType, SelectedHintIndex);
    }
    
    public void SelectTargetButton(InteractableObject obj)
    {
        SetSelection(obj);
        CurrentSelection = SelectionType.TargetButton;
        
        GameEvents.OnSelectionChanged?.Invoke(CurrentSelection, SelectedType, SelectedHintIndex);
    }

    private void SetSelection(InteractableObject obj) 
    {
        if (lastSelected != null) lastSelected.OnDeselect();
        lastSelected = obj;
        if (lastSelected != null) lastSelected.OnSelect();
    }
    
    public void ResetSelection() 
    {
        if (lastSelected != null) lastSelected.OnDeselect();
        lastSelected = null;
        CurrentSelection = SelectionType.None;
        SelectedType = TileType.O;
        SelectedHintIndex = -1;
        
        GameEvents.OnSelectionChanged?.Invoke(CurrentSelection, SelectedType, SelectedHintIndex);
    }


    private void OnGameStarted(Difficulty difficulty)
    {
        ResetSelection();
    }
}