using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameResultPopupUI : PopupUIBase
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;       // 성공/실패 텍스트
    [SerializeField] private TextMeshProUGUI descriptionText; // 상세 내용 (정답 좌표 등)
    [SerializeField] private Button retryButton;              // 재시작 버튼
    [SerializeField] private Button exitButton;               // 나가기 버튼

    private void OnEnable()
    {
        GameEvents.OnGameEnded += OnGameEnded;
    }

    // [추가] UI가 파괴되거나 꺼질 때 이벤트 구독 해제
    private void OnDisable()
    {
        GameEvents.OnGameEnded -= OnGameEnded;
    }
    
    public override void Initialize()
    {
        // 버튼 이벤트 리스너 등록
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(OnClickRetry);
        
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(OnClickExit);

        // 초기 상태는 숨김 처리
        Hide(); 
    }

    /// <summary>
    /// 외부(GameDirector)에서 승패 결과를 전달하며 팝업을 띄울 때 호출합니다.
    /// </summary>
    public void DisplayResult(bool isVictory, Vector2Int answerPos = default)
    {
        if (isVictory)
        {
            titleText.text = "VICTORY!";
            titleText.color = Color.cyan;
            descriptionText.text = "정확한 목표 기물의 위치를 찾았습니다!";
        }
        else
        {
            titleText.text = "DEFEAT";
            titleText.color = Color.red;
            // 좌표는 SelectionInfoUI와 동일하게 1부터 시작하도록 출력 (+1 처리)
            descriptionText.text = $"틀렸습니다.\n진짜 목표 기물의 위치는 ({answerPos.x + 1}, {answerPos.y + 1}) 입니다.";
        }

        // PopupUIBase의 Show()가 호출되며 자동으로 매니저의 팝업 스택에 Push 됩니다.
        Show();
    }

    private void OnClickRetry()
    {
        Hide(); // 자동으로 팝업 스택에서 Pop
        
        ScreenSpaceUIManager.Instance.ShowUI<GameDifficultyPopupUI>();
    }

    private void OnClickExit()
    {
        // 당장 로비 씬이 없으므로 로그만 띄워둡니다.
        // 추후 SceneManager.LoadScene("LobbyScene"); 등으로 구현
        Debug.Log("로비로 나가기 클릭됨");
        
        Hide();
    }

    private void OnGameEnded(bool isVictory, Vector2Int answer)
    {
        if (isVictory)
        {
            titleText.text = "VICTORY!";
            titleText.color = Color.cyan;
            descriptionText.text = "정확한 목표 기물의 위치를 찾았습니다!";
        }
        else
        {
            titleText.text = "DEFEAT";
            titleText.color = Color.red;
            descriptionText.text = $"틀렸습니다.\n진짜 목표 기물의 위치는 ({answer.x + 1}, {answer.y + 1}) 입니다.";
        }

        // PopupUIBase의 Show()를 호출하여 화면에 띄우고 스택에 추가
        Show();
    }

    // 결과 팝업은 중요한 진행 흐름이므로 ESC로 닫히지 않도록 오버라이드하여 막아둡니다.
    public override void ClosePopup()
    {
        Debug.Log("결과 창은 ESC로 닫을 수 없습니다. 화면의 버튼을 선택해 주세요.");
    }
}