using UnityEngine;

public class LaserRenderSimulator : MonoBehaviour 
{
    [SerializeField] private LineRenderer lineRenderer;
    [Header("Visual Settings")]
    [Tooltip("레이저의 시각적 높이를 조절합니다. 기본 좌표에 이 값만큼 더해집니다.")]
    [SerializeField] private float yOffset = 0.0f;

    public void DrawPath(LaserResult res) 
    {
        if (lineRenderer == null || res.pathPoints == null) return;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = res.pathPoints.Count;

        // 각 좌표에 yOffset을 더하여 LineRenderer에 세팅
        for (int i = 0; i < res.pathPoints.Count; i++)
        {
            Vector3 finalPos = res.pathPoints[i];
            finalPos.y += yOffset;
            lineRenderer.SetPosition(i, finalPos);
        }
    }

    public void ClearPath() 
    {
        if (lineRenderer == null) return;
        lineRenderer.enabled = false;
    }
}