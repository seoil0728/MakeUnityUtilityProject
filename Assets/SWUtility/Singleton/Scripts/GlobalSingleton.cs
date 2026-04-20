using UnityEngine;

namespace SWUtility.Singleton
{
    public abstract class GlobalSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isApplicationQuitting = false;

        // [추가] 초기화 완료 여부를 외부에서 확인할 수 있는 프로퍼티
        public bool IsInitialized { get; protected set; } = false;

        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting) return null;

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        T prefab = Resources.Load<T>(typeof(T).Name);

                        if (prefab != null)
                        {
                            _instance = Instantiate(prefab);
                            _instance.gameObject.name = typeof(T).Name;
                        }
                        else
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            _instance = singletonObject.AddComponent<T>();
                        }

                        _instance.transform.SetParent(null);
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject); 
            }
        }

        // ==========================================
        // [추가] 공통 초기화 가상 함수
        // ==========================================
        /// <summary>
        /// Boot 씬에서 매니저를 명시적으로 초기화할 때 호출합니다.
        /// 하위 클래스에서 override하여 사용하세요.
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized) return; // 중복 초기화 방지
            
            // (기본적으로는 아무 동작도 하지 않음)
            
            IsInitialized = true; // 초기화 완료 마킹 (하위 클래스에서 base.Initialize()를 호출하거나 직접 true로 설정해야 함)
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                IsInitialized = false; // 파괴 시 초기화 상태도 해제
            }
        }
    }
}
