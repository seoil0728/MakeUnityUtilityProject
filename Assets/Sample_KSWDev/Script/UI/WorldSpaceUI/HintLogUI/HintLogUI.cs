using UnityEngine;
using TMPro;

public class HintLogUI : WorldSpaceUIBase
{
    [SerializeField] private TextMeshProUGUI logText;

    public override void Initialize()
    {
        Show();
    }

    // UI가 켜질 때 이벤트 구독
    private void OnEnable()
    {
        GameEvents.OnGameStarted += OnGameStarted;
        GameEvents.OnHintProcessed += HandleHintProcessed;
    }

    // UI가 꺼지거나 파괴될 때 구독 해제 (메모리 누수 방지)
    private void OnDisable()
    {
        GameEvents.OnGameStarted -= OnGameStarted;
        GameEvents.OnHintProcessed -= HandleHintProcessed;
    }


    private void OnGameStarted(Difficulty difficulty)
    {
        if (logText != null)
        {
            logText.text = "";
        }
    }

    private void HandleHintProcessed(string resultText)
    {
        if (logText != null)
        {
            // 기존 텍스트에 줄바꿈으로 추가하거나, 새로 덮어씌울 수 있습니다.
            logText.text += $"{resultText}\n";
        }
    }
}