using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUIManager : MonoBehaviour
{
    public static WorldSpaceUIManager Instance { get; private set; }

    // Type을 키로 사용하여 제네릭하게 UI 인스턴스를 캐싱
    private Dictionary<Type, WorldSpaceUIBase> uiCache = new Dictionary<Type, WorldSpaceUIBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// WorldSpaceUIBase를 상속받는 UI들을 딕셔너리에 등록합니다.
    /// </summary>
    public void RegisterUI(WorldSpaceUIBase ui)
    {
        Type type = ui.GetType();
        if (!uiCache.ContainsKey(type))
        {
            uiCache.Add(type, ui);
        }
    }

    public void UnregisterUI(WorldSpaceUIBase ui)
    {
        Type type = ui.GetType();
        if (uiCache.ContainsKey(type))
        {
            uiCache.Remove(type);
        }
    }

    /// <summary>
    /// 등록된 특정 3D UI 인스턴스를 가져옵니다.
    /// 예: WorldSpaceUIManager.Instance.GetUI<HintLogUI>();
    /// </summary>
    public T GetUI<T>() where T : WorldSpaceUIBase
    {
        Type type = typeof(T);
        if (uiCache.TryGetValue(type, out WorldSpaceUIBase ui))
        {
            return ui as T;
        }
        
        Debug.LogWarning($"[WorldSpaceUIManager] {type.Name} 타입의 UI를 찾을 수 없습니다. 씬에 배치되어 있는지 확인하세요.");
        return null;
    }

    // 편의성 함수: 특정 UI를 바로 켜거나 끄고 싶을 때 사용
    public void ShowUI<T>() where T : WorldSpaceUIBase
    {
        T ui = GetUI<T>();
        if (ui != null) ui.Show();
    }

    public void HideUI<T>() where T : WorldSpaceUIBase
    {
        T ui = GetUI<T>();
        if (ui != null) ui.Hide();
    }
}