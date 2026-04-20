using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaserResult 
{
    public int startButton;
    public int endButton;
    public int collisionCount;
    public List<Vector3> pathPoints; // LineRenderer를 위한 월드 좌표 리스트
}