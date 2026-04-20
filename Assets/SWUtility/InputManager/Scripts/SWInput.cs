using UnityEngine;
using UnityEngine.InputSystem;

namespace SWUtility.InputManager
{
    /// <summary>
    /// 개발자가 사용할 정적(Static) 입력 인터페이스입니다.
    /// 레거시 Input 클래스와 100% 동일한 사용감을 제공합니다.
    /// </summary>
    public static class SWInput
    {
        // ==========================================
        // --- 1. 버튼 계열 (Button) ---
        // ==========================================

        /// <summary>
        /// 이번 프레임에 버튼을 처음 눌렀는지 확인합니다. (Input.GetButtonDown과 동일)
        /// </summary>
        public static bool GetButtonDown(string actionName)
        {
            InputAction action = InputManager.Instance.GetAction(actionName);
            return action != null && action.WasPressedThisFrame();
        }

        /// <summary>
        /// 버튼을 누르고 있는 상태인지 확인합니다. (Input.GetButton과 동일)
        /// </summary>
        public static bool GetButton(string actionName)
        {
            InputAction action = InputManager.Instance.GetAction(actionName);
            return action != null && action.IsPressed();
        }

        /// <summary>
        /// 이번 프레임에 버튼을 떼었는지 확인합니다. (Input.GetButtonUp과 동일)
        /// </summary>
        public static bool GetButtonUp(string actionName)
        {
            InputAction action = InputManager.Instance.GetAction(actionName);
            return action != null && action.WasReleasedThisFrame();
        }

        // ==========================================
        // --- 2. 축/벡터 계열 (Axis, Vector2) ---
        // ==========================================

        /// <summary>
        /// 1D 축 값을 가져옵니다. 보통 -1.0 ~ 1.0 사이의 값을 반환합니다. (Input.GetAxis와 동일)
        /// </summary>
        public static float GetAxis(string actionName)
        {
            InputAction action = InputManager.Instance.GetAction(actionName);
            if (action != null)
            {
                return action.ReadValue<float>();
            }
            return 0f;
        }

        /// <summary>
        /// 2D 벡터 값을 가져옵니다. (WASD 이동, 조이스틱 조작 등에 사용)
        /// </summary>
        public static Vector2 GetVector2(string actionName)
        {
            InputAction action = InputManager.Instance.GetAction(actionName);
            if (action != null)
            {
                return action.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }
    }
}
