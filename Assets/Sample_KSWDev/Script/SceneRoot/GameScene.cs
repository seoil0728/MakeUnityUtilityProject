using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void Init()
    {
        if (ObjectPoolManager.Instance != null)
            ObjectPoolManager.Instance.Initialize();
        
        if (SoundManager.Instance != null)
            SoundManager.Instance.Initialize();
        
        Debug.Log("Init GameScene.");
    }
    
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        ScreenSpaceUIManager.Instance.ShowUI<GameDifficultyPopupUI>();
    }

    private void OnDestroy()
    {
        GameEvents.ClearAllEvents();
    }
}
