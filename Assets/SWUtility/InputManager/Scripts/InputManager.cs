using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SWUtility.Singleton;

namespace SWUtility.InputManager
{
    /// <summary>
    /// New Input System을 로드하고 관리하는 싱글톤 매니저입니다.
    /// 외부에서는 이 클래스 대신 정적 클래스인 'SWInput'을 사용하세요.
    /// </summary>
    public class InputManager : GlobalSingleton<InputManager>
    {
        [Header("Input Asset Settings")]
        [Tooltip("Resources 폴더에 있는 .inputactions 에셋의 이름 (예: 'PlayerControls')")]
        [SerializeField] private string inputActionAssetName = "PlayerControls";
        
        [Header("Action Map Names")]
        [Tooltip("인게임 캐릭터 조작용 맵 이름")]
        [SerializeField] private string playerMapName = "Player";
        [Tooltip("UI 조작용 맵 이름")]
        [SerializeField] private string uiMapName = "UI";

        private InputActionAsset inputAsset;
        private Dictionary<string, InputAction> actionCache = new Dictionary<string, InputAction>();

        public bool IsInputEnabled { get; private set; } = false;

        public override void Initialize()
        {
            if (IsInitialized) return;

            LoadAndSetupInputAsset();

            base.Initialize();
        }

        private void LoadAndSetupInputAsset()
        {
            // 1. Resources 폴더에서 InputActionAsset 로드
            inputAsset = Resources.Load<InputActionAsset>(inputActionAssetName);

            if (inputAsset == null)
            {
                Debug.LogError($"[InputManager] '{inputActionAssetName}' 에셋을 Resources 폴더에서 찾을 수 없습니다! " +
                               "1. Window > Data > Input Action을 생성하고, " +
                               "2. Resources 폴더 안에 넣은 뒤 이름을 확인하세요.");
                return;
            }

            // 2. 에셋을 복제(Instantiate)하여 원본 수정을 방지하고, 런타임에 사용할 독립적인 인스턴스 생성
            inputAsset = Instantiate(inputAsset);

            // 3. 모든 액션을 찾아 딕셔너리에 캐싱 (빠른 검색을 위해)
            foreach (var actionMap in inputAsset.actionMaps)
            {
                foreach (var action in actionMap.actions)
                {
                    string key = action.name;
                    if (!actionCache.ContainsKey(key))
                    {
                        actionCache.Add(key, action);
                    }
                    else
                    {
                        Debug.LogWarning($"[InputManager] 액션 이름 중복 발생: '{key}' (맵: {actionMap.name}). 첫 번째 발견된 액션만 캐싱됩니다.");
                    }
                }
            }

            // 4. 초기 상태 활성화 (기본은 게임 모드)
            IsInputEnabled = true;
            SwitchToGameMode();
        }

        // ==========================================
        // --- 모드 제어 (Action Map 전환) ---
        // ==========================================

        /// <summary>
        /// 인게임 조작 모드로 전환합니다. (UI 입력 차단, 캐릭터 조작 허용)
        /// </summary>
        public void SwitchToGameMode()
        {
            if (inputAsset == null || !IsInputEnabled) return;

            var uiMap = inputAsset.FindActionMap(uiMapName);
            var playerMap = inputAsset.FindActionMap(playerMapName);

            if (uiMap != null) uiMap.Disable();
            if (playerMap != null) playerMap.Enable();

            Debug.Log("<color=cyan>[InputManager] 게임 모드 입력 활성화 (캐릭터 조작 가능)</color>");
        }

        /// <summary>
        /// UI 조작 모드로 전환합니다. (캐릭터 조작 차단, UI 입력 허용)
        /// 팝업이 열릴 때 호출하세요.
        /// </summary>
        public void SwitchToUIMode()
        {
            if (inputAsset == null || !IsInputEnabled) return;

            var playerMap = inputAsset.FindActionMap(playerMapName);
            var uiMap = inputAsset.FindActionMap(uiMapName);

            if (playerMap != null) playerMap.Disable();
            if (uiMap != null) uiMap.Enable();

            Debug.Log("<color=yellow>[InputManager] UI 모드 입력 활성화 (캐릭터 조작 불가)</color>");
        }

        // ==========================================
        // --- 전체 입력 제어 (컷씬 등에서 사용) ---
        // ==========================================

        /// <summary>
        /// 모든 입력을 완전히 활성화합니다. (게임 재개 시 호출)
        /// 기본적으로 게임 모드로 켜집니다.
        /// </summary>
        public void EnableInput()
        {
            if (inputAsset != null && !IsInputEnabled)
            {
                IsInputEnabled = true;
                SwitchToGameMode();
            }
        }

        /// <summary>
        /// 모든 입력을 완전히 비활성화합니다. (컷씬, 로딩 등에서 호출)
        /// </summary>
        public void DisableInput()
        {
            if (inputAsset != null && IsInputEnabled)
            {
                inputAsset.Disable();
                IsInputEnabled = false;
                Debug.Log("<color=red>[InputManager] 모든 입력이 강제로 비활성화됨 (컷씬/로딩 모드).</color>");
            }
        }

        // ==========================================
        // --- SWInput 정적 클래스에서 호출할 캐싱된 Action 검색용 내부 API ---
        // ==========================================

        public InputAction GetAction(string actionName)
        {
            if (!IsInitialized || !IsInputEnabled) return null;

            if (actionCache.TryGetValue(actionName, out InputAction action))
            {
                return action;
            }

            Debug.LogWarning($"[InputManager] '{actionName}' 액션을 찾을 수 없습니다. Input Action Asset을 확인하세요.");
            return null;
        }

        protected override void OnDestroy()
        {
            if (inputAsset != null)
            {
                inputAsset.Disable();
            }
            base.OnDestroy();
        }
    }
}
