using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileCellsIndexMaker : MonoBehaviour
{
    public int mapSize = 5;

    [ContextMenu("Auto Index Tiles")]
    public void AutoIndexTiles()
    {
        // 하위 오브젝트에서 TileCell 컴포넌트를 모두 가져옴
        TileCell[] tiles = GetComponentsInChildren<TileCell>(true);

        if (tiles.Length == 0)
        {
            Debug.LogWarning("하위에 TileCell 컴포넌트가 없습니다.");
            return;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            // 인덱스를 좌표로 변환 (0,0 -> 1,0 -> 2,0 ... 0,1 -> 1,1 ...)
            int x = i % mapSize;
            int y = i / mapSize;

            tiles[i].x = x;
            tiles[i].y = y;

            // 에디터에서 변경 사항을 저장할 수 있도록 상태 표시 (Dirty 설정)
#if UNITY_EDITOR
            EditorUtility.SetDirty(tiles[i]);
#endif
        }

        Debug.Log($"총 {tiles.Length}개의 타일 인덱싱이 완료되었습니다. (MapSize: {mapSize})");
        
        // 씬 변경 사항 저장 알림
#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }
}