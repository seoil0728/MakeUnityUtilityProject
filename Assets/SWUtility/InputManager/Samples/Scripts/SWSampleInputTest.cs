using UnityEngine;
using SWUtility.InputManager;

namespace SWUtility.Sample
{
    /// <summary>
    /// SWInput의 작동을 테스트하기 위한 샘플 스크립트입니다.
    /// 아무 게임 오브젝트에나 붙여서 테스트할 수 있습니다.
    /// </summary>
    public class SWSampleInputTest : MonoBehaviour
    {
        private void Start()
        {
            // InputManager 초기화 시도 (보통 Boot 씬이나 GameScene의 Root에서 진행됨)
            if (InputManager.InputManager.Instance != null && !InputManager.InputManager.Instance.IsInitialized)
            {
                InputManager.InputManager.Instance.Initialize();
            }
        }

        private void Update()
        {
            // 1. 단발성 버튼 입력 (Jump)
            if (SWInput.GetButtonDown("Jump"))
            {
                Debug.Log("<color=cyan>[InputTest] 점프 (Jump) 눌림!</color>");
            }

            // 2. 지속적인 버튼 누름 확인 (Fire)
            if (SWInput.GetButton("Fire"))
            {
                Debug.Log("<color=red>[InputTest] 발사 (Fire) 누르는 중...</color>");
            }

            // 3. 버튼 뗌 확인 (Fire)
            if (SWInput.GetButtonUp("Fire"))
            {
                Debug.Log("<color=yellow>[InputTest] 발사 (Fire) 뗌!</color>");
            }

            // 4. 2D 벡터 이동 (Move)
            Vector2 moveDir = SWInput.GetVector2("Move");
            if (moveDir != Vector2.zero)
            {
                Debug.Log($"<color=green>[InputTest] 이동 방향 (Move): {moveDir}</color>");
            }
        }

        // GUI를 그려서 입력 활성화/비활성화 및 모드 전환 테스트
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 300));
            
            GUILayout.Label("--- 모드 전환 테스트 ---");
            if (GUILayout.Button("1. 게임 모드로 전환 (Player 맵 활성화)"))
            {
                InputManager.InputManager.Instance.SwitchToGameMode();
            }

            if (GUILayout.Button("2. UI 모드로 전환 (UI 맵 활성화)"))
            {
                InputManager.InputManager.Instance.SwitchToUIMode();
            }
            
            GUILayout.Space(20);
            
            GUILayout.Label("--- 전체 제어 (컷씬 모드) ---");
            if (GUILayout.Button("모든 입력 비활성화 (Disable)"))
            {
                InputManager.InputManager.Instance.DisableInput();
            }

            if (GUILayout.Button("모든 입력 활성화 (Enable)"))
            {
                InputManager.InputManager.Instance.EnableInput();
            }

            GUILayout.Space(10);
            GUILayout.Label("현재 입력 상태 (전체): " + (InputManager.InputManager.Instance.IsInputEnabled ? "ON" : "OFF"));

            GUILayout.EndArea();
        }
    }
}
