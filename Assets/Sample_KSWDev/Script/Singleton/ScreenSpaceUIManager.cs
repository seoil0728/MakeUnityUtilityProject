using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceUIManager : MonoBehaviour
{
    public static ScreenSpaceUIManager Instance { get; private set; }

    private Dictionary<Type, ScreenSpaceUIBase> uiCache = new Dictionary<Type, ScreenSpaceUIBase>();

    // 스택과 캐싱 변수 분리
    private Stack<ScreenUIBase> screenStack = new Stack<ScreenUIBase>();
    private ScreenUIBase currentScreen = null; // Update 최적화를 위한 캐싱 변수
    
    private Stack<PopupUIBase> popupStack = new Stack<PopupUIBase>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Peek() 호출 없이 캐싱된 currentScreen만 검사하여 최적화
        if (currentScreen != null && currentScreen.UseMoveFrame)
        {
            currentScreen.MoveFrame();
        }
    }

    public void RegisterUI(ScreenSpaceUIBase ui)
    {
        Type type = ui.GetType();
        if (!uiCache.ContainsKey(type)) uiCache.Add(type, ui);
    }

    public void UnregisterUI(ScreenSpaceUIBase ui)
    {
        Type type = ui.GetType();
        if (uiCache.ContainsKey(type)) uiCache.Remove(type);
    }

    public T GetUI<T>() where T : ScreenSpaceUIBase
    {
        Type type = typeof(T);
        if (uiCache.TryGetValue(type, out ScreenSpaceUIBase ui)) return ui as T;
        
        Debug.LogWarning($"[ScreenSpaceUIManager] {type.Name} UI를 찾을 수 없습니다.");
        return null;
    }

    // ==========================================
    // --- 외부 호출용 편의성 함수 ---
    // ==========================================
    public void ShowUI<T>() where T : ScreenSpaceUIBase
    {
        T ui = GetUI<T>();
        if (ui != null) ui.Show(); 
    }

    public void HideUI<T>() where T : ScreenSpaceUIBase
    {
        T ui = GetUI<T>();
        if (ui != null) ui.Hide();
    }

    // ==========================================
    // --- Screen 로직 (페이지 네비게이션 스택) ---
    // ==========================================
    public void PushScreen(ScreenUIBase newScreen)
    {
        if (currentScreen == newScreen) return;

        // 기존 스크린 숨김
        if (currentScreen != null)
        {
            currentScreen.SetVisible(false);
        }

        if (screenStack.Contains(newScreen))
        {
            RemoveScreenFromStack(newScreen);
        }

        screenStack.Push(newScreen);
        
        // 캐싱 변수 갱신 및 시각적 활성화
        currentScreen = newScreen;
        currentScreen.SetVisible(true);
    }

    public void PopScreen(ScreenUIBase screen)
    {
        // 현재 최상단 스크린을 닫으려고 할 때만 동작
        if (currentScreen == screen)
        {
            var poppedScreen = screenStack.Pop();
            poppedScreen.SetVisible(false);

            // 이전 스크린 복원
            if (screenStack.Count > 0)
            {
                currentScreen = screenStack.Peek();
                currentScreen.SetVisible(true);
            }
            else
            {
                currentScreen = null; // 스택이 비었으면 null 처리
            }
        }
    }

    private void RemoveScreenFromStack(ScreenUIBase screen)
    {
        var tempArray = screenStack.ToArray();
        screenStack.Clear();
        for (int i = tempArray.Length - 1; i >= 0; i--)
        {
            if (tempArray[i] != screen) screenStack.Push(tempArray[i]);
        }
    }

    // ==========================================
    // --- Popup 로직 (중첩 모달 스택) ---
    // ==========================================
    public void PushPopup(PopupUIBase popup)
    {
        if (!popupStack.Contains(popup)) popupStack.Push(popup);
    }

    public void PopPopup(PopupUIBase popup)
    {
        if (popupStack.Count > 0 && popupStack.Peek() == popup) popupStack.Pop();
    }

    public void CloseTopPopup()
    {
        if (popupStack.Count > 0)
        {
            popupStack.Peek().ClosePopup();
        }
    }
}