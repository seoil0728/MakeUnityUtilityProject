using UnityEngine;
using System.Collections.Generic;


namespace SWUtility.UIManager
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance = null;
        #endregion
        
        #region Inspector
        [SerializeField]
        private HUDUI[] huds = null;
        [SerializeField]
        private PageUI[] pages = null;
        [SerializeField]
        private PopupUI[] popups = null;

        #endregion
        
        #region Private Field
        private bool inited = false;

        private Dictionary<string, HUDUI> hudDict = new Dictionary<string, HUDUI>();
        private Dictionary<string, PageUI> pagesDict = new Dictionary<string, PageUI>();
        private Dictionary<string, PopupUI> popupDict = new Dictionary<string, PopupUI>();
        #endregion
        
        #region Public Field
        
        public bool Inited => inited;

        #endregion

        public void Initialize()
        {
            // Singleton instance
            if (Instance != null)
                return;
            
            Instance = this;
        }

        public void UnInitialize()
        {
            if (Instance == this)
                Instance = null;
        }

        public void Init()
        {
            // UIManagers Setting.
            foreach (var hud in huds)
            {
                hudDict.Add(hud.name, hud);
            }

            foreach (var page in pages)
            {
                pagesDict.Add(page.name, page);
            }

            foreach (var popup in popups)
            {
                popupDict.Add(popup.name, popup);
            }
            
            inited = true;
            
            Debug.Log("[UIManager] Init.");
        }

        public void UnInit()
        {
            hudDict.Clear();
            pagesDict.Clear();
            popupDict.Clear();
            
            inited = false;
            
            Debug.Log("[UIManager] UnInit.");
        }
        

        public void MoveFrame()
        {
            
        }
        
        // TODO : Open and Close Page, Popup, HUD Function
        
        
        
        // Unity Callbacks
        private void Reset()
        {
            huds = GetComponentsInChildren<HUDUI>();
            pages = GetComponentsInChildren<PageUI>();
            popups = GetComponentsInChildren<PopupUI>();
        }
        
        
        /*
         * This Part is Just a Sample of Singleton Initialize.
         * If you have another Singleton Solution, Change This Field.
         */

        private void Awake()
        {
            Initialize();
        }
    }

}
