using UnityEngine;

namespace SWUtility.UIManager
{   
    public class UISampleSceneRoot : MonoBehaviour
    {
        private bool inited = false;

        public bool Inited => inited;
        
        private void Initialize()
        {
            if (inited)
                return;
            
            // Managers in DontDestroy Initialize.
            // UIManager is depands on Scene.
            // So, Do not call UIManager.instance in this Method.
            // UIManager is Initialize at Awake (Just in Sample),
            // So you should call UIManagers instance in Init (Start).

            inited = true;

        }

        private void UnInitialze()
        {
            if (!inited)
                return;
            
            // Managers in Scene (ex UIManager) UnInitialize
            // this Logic is just sample.
            UnInit();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UnInitialize();
            }
            
            inited = false;
        }

        private void Init()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.Init();   
            }
        }

        private void UnInit()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UnInit();    
            }
        }

        private void MoveFrame()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.MoveFrame();    
            }
        }
        
        // Unity Callbacks
        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (!inited)
                return;

            MoveFrame();
        }

        private void OnDestroy()
        {
            UnInitialze();
        }
        
    }

}
