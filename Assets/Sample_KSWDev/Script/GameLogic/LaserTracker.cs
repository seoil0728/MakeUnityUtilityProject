using System.Collections.Generic;
using UnityEngine;

public class LaserTracker 
{
    private int mapSize;

    public LaserTracker(int size)
    {
        this.mapSize = size;
    }

    public LaserResult CalculateLaserPath(TileType[,] map, int startButton)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(GameDirector.Instance.GetButtonWorldPosition(startButton));

        Vector2Int gridPos = GetStartPosition(startButton);
        Vector2Int dir = GetStartDirection(startButton);
        int collisionCount = 0;

        int safetyBreaker = 0;
        while (gridPos.x >= 0 && gridPos.x < mapSize && gridPos.y >= 0 && gridPos.y < mapSize)
        {
            if (safetyBreaker++ > 100) break;

            TileType tile = map[gridPos.x, gridPos.y];
            if (tile != TileType.O)
            {
                collisionCount++;
                points.Add(GameDirector.Instance.GetTileWorldPosition(gridPos.x, gridPos.y));

                if (tile == TileType.L) dir = new Vector2Int(dir.y, dir.x);
                else if (tile == TileType.R) dir = new Vector2Int(-dir.y, -dir.x);
                else if (tile == TileType.T) dir = new Vector2Int(-dir.x, -dir.y);
            }
            gridPos += dir;
        }

        int endBtn = GetEndButton(gridPos);
        points.Add(GameDirector.Instance.GetButtonWorldPosition(endBtn));

        return new LaserResult {
            startButton = startButton,
            endButton = endBtn,
            collisionCount = collisionCount,
            pathPoints = points
        };
    }
    
    public string FormatResult(LaserResult result)
    {
        return $"{result.startButton} -> ({result.collisionCount}) -> {result.endButton}";
    }

    private Vector2Int GetStartPosition(int btn)
    {
        if (btn >= 1 && btn <= 5) return new Vector2Int(btn - 1, 0);
        if (btn >= 6 && btn <= 10) return new Vector2Int(4, btn - 6);
        if (btn >= 11 && btn <= 15) return new Vector2Int(15 - btn, 4);
        if (btn >= 16 && btn <= 20) return new Vector2Int(0, 20 - btn);
        return Vector2Int.zero;
    }

    private Vector2Int GetStartDirection(int btn)
    {
        if (btn >= 1 && btn <= 5) return new Vector2Int(0, 1);
        if (btn >= 6 && btn <= 10) return new Vector2Int(-1, 0);
        if (btn >= 11 && btn <= 15) return new Vector2Int(0, -1);
        if (btn >= 16 && btn <= 20) return new Vector2Int(1, 0);
        return Vector2Int.zero;
    }

    private int GetEndButton(Vector2Int pos)
    {
        if (pos.y < 0) return pos.x + 1;
        if (pos.x >= mapSize) return pos.y + 6;
        if (pos.y >= mapSize) return 15 - pos.x;
        if (pos.x < 0) return 20 - pos.y;
        return -1;
    }
}