using UnityEngine;
using UnityEngine.UI;

public class GameDifficultyPopupUI : PopupUIBase
{
    [Header("Difficulty Buttons")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;

    [Header("Optional Buttons")]
    [SerializeField] private Button closeButton; // 팝업 닫기 버튼 (필요시 사용)

    public override void Initialize()
    {
        // 람다식을 사용하여 버튼 클릭 시 난이도 매개변수를 함께 전달
        if (easyButton != null)
        {
            easyButton.onClick.RemoveAllListeners();
            easyButton.onClick.AddListener(() => OnClickDifficulty(Difficulty.Easy));
        }

        if (normalButton != null)
        {
            normalButton.onClick.RemoveAllListeners();
            normalButton.onClick.AddListener(() => OnClickDifficulty(Difficulty.Normal));
        }

        if (hardButton != null)
        {
            hardButton.onClick.RemoveAllListeners();
            hardButton.onClick.AddListener(() => OnClickDifficulty(Difficulty.Hard));
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(ClosePopup);
        }

        // 초기 상태는 숨김 처리
        Hide();
    }

    private void OnClickDifficulty(Difficulty difficulty)
    {
        // 1. 팝업을 닫고 팝업 스택에서 제거
        Hide();
        
        // 2. 선택된 난이도로 게임 시작 이벤트 브로드캐스팅
        GameEvents.OnGameStarted?.Invoke(difficulty);
    }
}