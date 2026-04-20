using System.Collections.Generic;
using UnityEngine;

public class MapGenerator 
{
    private int mapSize;

    private int t_x;
    private int t_y;

    public MapGenerator(int size)
    {
        this.mapSize = size;
    }

    public TileType[,] GenerateMap(Difficulty difficulty)
    {
        TileType[,] map = new TileType[mapSize, mapSize];

        // 1. 초기화
        for (int x = 0; x < mapSize; x++)
            for (int y = 0; y < mapSize; y++)
                map[x, y] = TileType.O;
        
        t_x = t_y = 0;

        // 2. 난이도별 설정
        int rhombusCount = 0;
        int independentMirrorCount = 0;

        switch (difficulty)
        {
            case Difficulty.Easy: rhombusCount = 1; independentMirrorCount = 2; break;
            case Difficulty.Normal: rhombusCount = 1; independentMirrorCount = 4; break;
            case Difficulty.Hard: rhombusCount = 2; independentMirrorCount = 2; break;
        }

        // 3. 기물 배치
        PlaceRhombus(map, rhombusCount);
        PlaceTarget(map);
        PlaceIndependentMirrors(map, independentMirrorCount);

        return map;
    }

    public Vector2Int DebugGetAnswerTargetPosition()
    {
        return new Vector2Int(t_x, t_y);
    }

    private void PlaceRhombus(TileType[,] map, int count)
    {
        int placed = 0;
        int attempts = 0;
        while (placed < count && attempts < 100)
        {
            attempts++;
            int startX = Random.Range(0, mapSize - 1);
            int startY = Random.Range(0, mapSize - 1);

            if (map[startX, startY] == TileType.O && map[startX + 1, startY] == TileType.O &&
                map[startX, startY + 1] == TileType.O && map[startX + 1, startY + 1] == TileType.O)
            {
                map[startX, startY] = TileType.R;
                map[startX + 1, startY] = TileType.L;
                map[startX, startY + 1] = TileType.L;
                map[startX + 1, startY + 1] = TileType.R;
                placed++;
            }
        }
    }

    private void PlaceTarget(TileType[,] map)
    {
        List<Vector2Int> emptyCells = GetEmptyCells(map);
        if (emptyCells.Count > 0)
        {
            Vector2Int pos = emptyCells[Random.Range(0, emptyCells.Count)];
            map[pos.x, pos.y] = TileType.T;
            
            t_x = pos.x;
            t_y = pos.y;
        }
    }

    private void PlaceIndependentMirrors(TileType[,] map, int count)
    {
        for (int i = 0; i < count; i++)
        {
            List<Vector2Int> emptyCells = GetEmptyCells(map);
            if (emptyCells.Count == 0) break;
            Vector2Int pos = emptyCells[Random.Range(0, emptyCells.Count)];
            map[pos.x, pos.y] = Random.value > 0.5f ? TileType.L : TileType.R;
        }
    }

    private List<Vector2Int> GetEmptyCells(TileType[,] map)
    {
        List<Vector2Int> empties = new List<Vector2Int>();
        for (int x = 0; x < mapSize; x++)
            for (int y = 0; y < mapSize; y++)
                if (map[x, y] == TileType.O) empties.Add(new Vector2Int(x, y));
        return empties;
    }
}