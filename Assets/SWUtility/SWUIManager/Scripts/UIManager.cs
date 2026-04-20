using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWUtility.UIManager
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private Dictionary<Type, UIBase> uiCache = new Dictionary<Type, UIBase>();

        private Stack<PageUI> pageStack = new Stack<PageUI>();
        private PageUI currentPage = null; 
        
        private Stack<PopupUI> popupStack = new Stack<PopupUI>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (currentPage != null && currentPage.UseMoveFrame)
            {
                currentPage.MoveFrame();
            }
        }

        public void RegisterUI(UIBase ui)
        {
            Type type = ui.GetType();
            if (!uiCache.ContainsKey(type)) uiCache.Add(type, ui);
        }

        public void UnregisterUI(UIBase ui)
        {
            Type type = ui.GetType();
            if (uiCache.ContainsKey(type)) uiCache.Remove(type);
        }

        public T GetUI<T>() where T : UIBase
        {
            Type type = typeof(T);
            if (uiCache.TryGetValue(type, out UIBase ui)) return ui as T;
            
            Debug.LogWarning($"[UIManager] {type.Name} UI를 찾을 수 없습니다.");
            return null;
        }

        // ==========================================
        // --- 외부 호출용 편의성 함수 ---
        // ==========================================
        public void ShowUI<T>() where T : UIBase
        {
            T ui = GetUI<T>();
            if (ui != null) ui.Show(); 
        }

        public void HideUI<T>() where T : UIBase
        {
            T ui = GetUI<T>();
            if (ui != null) ui.Hide();
        }

        // ==========================================
        // --- Page 로직 (페이지 네비게이션 스택) ---
        // ==========================================
        public void PushPage(PageUI newPage)
        {
            if (currentPage == newPage) return;

            if (currentPage != null)
            {
                currentPage.SetVisible(false);
            }

            if (pageStack.Contains(newPage))
            {
                RemovePageFromStack(newPage);
            }

            pageStack.Push(newPage);
            
            currentPage = newPage;
            currentPage.SetVisible(true);
        }

        public void PopPage(PageUI page)
        {
            if (currentPage == page)
            {
                var poppedPage = pageStack.Pop();
                poppedPage.SetVisible(false);

                if (pageStack.Count > 0)
                {
                    currentPage = pageStack.Peek();
                    currentPage.SetVisible(true);
                }
                else
                {
                    currentPage = null; 
                }
            }
        }

        private void RemovePageFromStack(PageUI page)
        {
            var tempArray = pageStack.ToArray();
            pageStack.Clear();
            for (int i = tempArray.Length - 1; i >= 0; i--)
            {
                if (tempArray[i] != page) pageStack.Push(tempArray[i]);
            }
        }

        // ==========================================
        // --- Popup 로직 (중첩 모달 스택) ---
        // ==========================================
        public void PushPopup(PopupUI popup)
        {
            if (!popupStack.Contains(popup)) popupStack.Push(popup);
        }

        public void PopPopup(PopupUI popup)
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
}
