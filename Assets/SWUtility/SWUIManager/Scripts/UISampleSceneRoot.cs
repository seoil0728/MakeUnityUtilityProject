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
            
            inited = true;
        }

        private void UnInitialze()
        {
            if (!inited)
                return;
            
            inited = false;
        }

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            UnInitialze();
        }
    }
}
