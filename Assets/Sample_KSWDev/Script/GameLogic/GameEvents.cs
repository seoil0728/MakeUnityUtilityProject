using System;
using UnityEngine;

public static class GameEvents
{
    // 게임이 시작되었을 때 (난이도)
    public static Action<Difficulty> OnGameStarted;
    
    // 게임이 종료되었을 때 (승리 여부, 정답 좌표)
    public static Action<bool, Vector2Int> OnGameEnded;
    
    // 힌트 처리가 완료되었을 때 (결과 텍스트 전달)
    public static Action<string> OnHintProcessed;

    // 플레이어의 선택 상태가 변경되었을 때 (현재 선택 타입, 타일 타입, 힌트 인덱스)
    public static Action<SelectionType, TileType, int> OnSelectionChanged;

    // 실제 목표 기물이 타일에 배치되었을 때 (x 좌표, y 좌표)
    public static Action<int, int> OnActualTargetPlaced;

    // 실제 목표 기물이 제거되었을 때
    public static Action OnActualTargetRemoved;
    
    
    public static void ClearAllEvents()
    {
        OnGameStarted = null;
        OnGameEnded = null;
        
        OnHintProcessed = null;
        OnSelectionChanged = null;
        OnActualTargetPlaced = null;
        OnActualTargetRemoved = null;
    }
}