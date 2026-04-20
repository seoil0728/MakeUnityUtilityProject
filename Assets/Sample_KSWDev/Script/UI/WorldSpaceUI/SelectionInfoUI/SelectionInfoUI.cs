using UnityEngine;
using TMPro;

public class SelectionInfoUI : WorldSpaceUIBase
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI infoText; // 실행될 액션의 정보를 표시할 단일 텍스트

    // 현재 상태를 캐싱할 변수들
    private bool isTargetPlaced = false;
    private int currentTargetX = -1;
    private int currentTargetY = -1;
    
    private SelectionType currentSelection = SelectionType.None;
    private int currentHintIndex = -1;

    public override void Initialize()
    {
        ResetData();
        Show();
    }

    private void OnEnable()
    {
        GameEvents.OnGameStarted += OnGameStarted;
        GameEvents.OnSelectionChanged += HandleSelectionChanged;
        GameEvents.OnActualTargetPlaced += HandleTargetPlaced;
        GameEvents.OnActualTargetRemoved += HandleTargetRemoved;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= OnGameStarted;
        GameEvents.OnSelectionChanged -= HandleSelectionChanged;
        GameEvents.OnActualTargetPlaced -= HandleTargetPlaced;
        GameEvents.OnActualTargetRemoved -= HandleTargetRemoved;
    }

    private void ResetData()
    {
        isTargetPlaced = false;
        currentTargetX = -1;
        currentTargetY = -1;
        currentSelection = SelectionType.None;
        currentHintIndex = -1;
        
        UpdateDisplay();
    }

    private void OnGameStarted(Difficulty difficulty)
    {
        ResetData();
    }
    

    // 1. 선택 상태가 변경되었을 때 (힌트, 마커 등)
    private void HandleSelectionChanged(SelectionType selectionType, TileType tileType, int hintIndex)
    {
        currentSelection = selectionType;
        currentHintIndex = hintIndex;
        
        UpdateDisplay();
    }

    // 2. 실제 목표 기물이 배치되었을 때
    private void HandleTargetPlaced(int x, int y)
    {
        isTargetPlaced = true;
        currentTargetX = x;
        currentTargetY = y;
        
        UpdateDisplay();
    }

    // 3. 실제 목표 기물이 제거되었을 때 (마커 덮어쓰기 등으로 인해)
    private void HandleTargetRemoved()
    {
        isTargetPlaced = false;
        currentTargetX = -1;
        currentTargetY = -1;
        
        UpdateDisplay();
    }

    // 우선순위에 따라 텍스트를 결정하고 출력하는 핵심 로직
    private void UpdateDisplay()
    {
        if (infoText == null) return;

        // 1순위: 목표 기물이 배치된 상태 (무조건 우선 표시, 1부터 시작하는 좌표로 변환)
        if (isTargetPlaced)
        {
            infoText.text = $"({currentTargetX + 1}, {currentTargetY + 1})";
        }
        // 2순위: 목표 기물이 없고, 힌트 버튼이 선택된 상태
        else if (currentSelection == SelectionType.HintButton)
        {
            infoText.text = $"{currentHintIndex}";
        }
        // 3순위: 그 외의 상태 (아무것도 출력하지 않음)
        else
        {
            infoText.text = "";
        }
    }
}