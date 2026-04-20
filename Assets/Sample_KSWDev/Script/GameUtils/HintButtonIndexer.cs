using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HintButtonIndexer : MonoBehaviour
{
    [ContextMenu("Auto Index Buttons")]
    public void AutoIndexButtons()
    {
        // 하위 오브젝트에서 HintButton 컴포넌트를 모두 가져옴
        HintButton[] buttons = GetComponentsInChildren<HintButton>(true);

        if (buttons.Length == 0)
        {
            Debug.LogWarning("하위에 HintButton 컴포넌트가 없습니다.");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            // 1번부터 시작하도록 인덱스 부여
            buttons[i].index = i + 1;

            // 에디터 변경 사항 저장 설정
#if UNITY_EDITOR
            EditorUtility.SetDirty(buttons[i]);
#endif
        }

        Debug.Log($"총 {buttons.Length}개의 힌트 버튼 인덱싱이 완료되었습니다.");

#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }
}