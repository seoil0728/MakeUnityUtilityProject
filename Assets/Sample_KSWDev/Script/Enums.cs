public enum TileType 
{ 
    O, // Empty
    L, // \ Wall
    R, // / Wall
    T,  // Target
    Max
}

public enum Difficulty 
{ 
    Easy, 
    Normal, 
    Hard 
}

public enum SelectionType 
{ 
    None, 
    HintButton, 
    MarkerButton,
    TargetButton
}