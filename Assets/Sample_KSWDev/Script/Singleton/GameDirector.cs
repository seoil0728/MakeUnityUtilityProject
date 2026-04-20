using UnityEditor;
using UnityEngine;

public class GameDirector : MonoBehaviour 
{
    public static GameDirector Instance { get; private set; }

    [Header("Board Config")]
    public int mapSize = 5;
    public float cellSize = 1.2f;
    public Vector3 boardOrigin = Vector3.zero;

    [Header("References")]
    [SerializeField] private LaserRenderSimulator laserRender;
    [SerializeField] private Transform[] hintButtonTransforms; 

    // 실제 목표 기물 좌표 (초기값 -1)
    private int actualTargetX = -1;
    private int actualTargetY = -1;
    
    // 이전에 목표 기물이 배치된 셀을 추적하여 나중에 지우기 위함
    private TileCell currentTargetCell = null;
    
    private TileCell[,] tileCells;
    
    private TileType[,] realMap;
    private TileType[,] virtualMap;
    private MapGenerator mapGenerator;
    private LaserTracker laserTracker;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this; 
            Initialize();
        }

        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        GameEvents.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= OnGameStarted;
    }

    private void Initialize() 
    {
        tileCells = new TileCell[mapSize, mapSize];
        mapGenerator = new MapGenerator(mapSize);
        laserTracker = new LaserTracker(mapSize);
        virtualMap = new TileType[mapSize, mapSize];
    }

    public void UpdateVirtualMap(int x, int y, TileType type) 
    {
        virtualMap[x, y] = type;
    }

    public LaserResult SimulateRealPath(int startButton)
    {
        return laserTracker.CalculateLaserPath(realMap, startButton);   
    }

    public LaserResult SimulateVirtualPath(int startButton)
    {
        return laserTracker.CalculateLaserPath(virtualMap, startButton);   
    }
    
    /// <summary>
    /// [추가] 외부에서 힌트 결과를 텍스트로 얻기 위해 호출하는 메인 함수
    /// </summary>
    /// <param name="startButton">클릭한 버튼 번호</param>
    /// <param name="useRealMap">true면 실제 맵 결과, false면 가상 맵 결과</param>
    public string GetFormattedLaserResult(int startButton, bool useRealMap)
    {
        LaserResult result = useRealMap ? SimulateRealPath(startButton) : SimulateVirtualPath(startButton);
        return laserTracker.FormatResult(result);
    }

    public bool CheckVictory(int x, int y)
    {
        return realMap[x, y] == TileType.T;   
    }

    public Vector3 GetTileWorldPosition(int x, int y) 
    {
        if (tileCells[x, y] != null)
        {
            // 실제 타일 오브젝트의 위치 반환
            Vector3 pos = tileCells[x, y].transform.position;
            return new Vector3(pos.x, pos.y, pos.z);
        }
        
        // 만약 타일이 등록 안 됐다면 기본 계산식 사용 (예외 방지)
        return boardOrigin + new Vector3(x * cellSize, 0, -y * cellSize);
    }
    
    public Vector3 GetButtonWorldPosition(int btnIdx) 
    {
        if (btnIdx < 1 || btnIdx > hintButtonTransforms.Length) return Vector3.zero;
        
        return hintButtonTransforms[btnIdx - 1].position;
    }

    public LaserRenderSimulator GetLaserRenderer()
    {
        return laserRender;
    }
    
    // 실제 목표 기물 배치 정보를 업데이트하는 함수
    public void SetActualTarget(int x, int y, TileCell newTargetCell)
    {
        // 이미 다른 곳에 목표 기물이 배치되어 있다면 기존 타일 초기화
        if (currentTargetCell != null && currentTargetCell != newTargetCell)
        {
            currentTargetCell.ClearTile(); // 기존 타일의 ClearTile 함수 호출
        }

        actualTargetX = x;
        actualTargetY = y;
        currentTargetCell = newTargetCell;
        
        GameEvents.OnActualTargetPlaced?.Invoke(x, y);
    }

    public void RemoveActualTarget()
    {
        actualTargetX = -1;
        actualTargetY = -1;

        currentTargetCell = null;
        
        GameEvents.OnActualTargetRemoved?.Invoke();
    }
    
    /// <summary>
    /// 게임의 현재 상태를 검사하여 힌트 결과 혹은 승패 판정을 실행합니다.
    /// </summary>
    public void ExecuteCheck()
    {
        // 1. 가상 맵에서 목표 기물(T)의 위치 탐색
        if (actualTargetX != -1 && actualTargetY != -1)
        {
            ProcessVictoryDefeat(actualTargetX, actualTargetY);
            return;
        }
        
        // 2. 목표 기물이 없고 힌트 버튼이 선택된 경우 -> 힌트 확인 모드
        if (SelectionManager.Instance.CurrentSelection == SelectionType.HintButton)
        {
            ProcessHintRequest();
        }
        else
        {
            Debug.Log("실행할 작업이 없습니다. 마커(T)를 배치하거나 힌트 버튼을 선택하세요.");
        }
    }

    private void OnGameStarted(Difficulty difficulty)
    {
        actualTargetX = -1;
        actualTargetY = -1;
        currentTargetCell = null;
        
        realMap = mapGenerator.GenerateMap(difficulty);
        
        for (int x = 0; x < mapSize; x++) 
        {
            for (int y = 0; y < mapSize; y++) 
            {
                virtualMap[x, y] = TileType.O;
                
                if (tileCells[x, y] != null)
                {
                    tileCells[x, y].ClearTile();
                }
            }
        }
        
        laserRender.ClearPath();

        Debug.Log($"New Match Started! Difficulty: {difficulty}");
    }
    

    private void ProcessVictoryDefeat(int x, int y)
    {
        bool isVictory = CheckVictory(x, y);
        var answer = mapGenerator.DebugGetAnswerTargetPosition();
        
        if (isVictory)
        {
            Debug.Log("<color=cyan>[결과] 승리! 목표 기물을 정확히 찾았습니다.</color>");
            // TODO: UI 매니저를 통해 승리 팝업 표시
        }
        else
        {
            Debug.Log("<color=red>[결과] 패배! 해당 위치에는 목표가 없습니다.</color>");
            Debug.Log($"Answer is {answer.x}, {answer.y}");
            // TODO: UI 매니저를 통해 패배 팝업 표시
        }
        
        GameEvents.OnGameEnded?.Invoke(isVictory, answer);
    }

    private void ProcessHintRequest()
    {
        int hintIdx = SelectionManager.Instance.SelectedHintIndex;
        // 실제 정답 맵 기반 시뮬레이션 결과 포맷팅
        string resultText = GetFormattedLaserResult(hintIdx, true);
        
        Debug.Log($"<color=yellow>[힌트 로그] {resultText}</color>");
        
        GameEvents.OnHintProcessed?.Invoke(resultText);
        
        // 결과 출력 후 선택 상태 리셋 (턴 종료 연출용)
        SelectionManager.Instance.ResetSelection();
        
        // TODO: UI 로그 리스트에 결과 추가
    }
    
    // TileCell이 생성될 때 자신을 등록하는 함수
    public void RegisterTile(int x, int y, TileCell tile) 
    {
        if (tileCells == null)
        {
            tileCells = new TileCell[mapSize, mapSize];
        }
        
        tileCells[x, y] = tile;
    }

    [ContextMenu("Test")]
    public void Tests()
    {
        Debug.Log("Test.");
    }
}