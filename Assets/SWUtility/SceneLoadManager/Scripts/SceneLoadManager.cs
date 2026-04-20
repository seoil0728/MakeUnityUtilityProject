using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWUtility.SceneLoadManager
{
    public class SceneLoadManager : SWUtility.Singleton.GlobalSingleton<SceneLoadManager>
    {
        private MainSceneRoot currentMainSceneRoot;
        
        // 현재 띄워져 있는 Additive 씬들을 추적하여 Pause 상태 관리에 사용
        private List<AdditiveSceneRoot> activeAdditiveScenes = new List<AdditiveSceneRoot>();

        public override void Initialize()
        {
            if (IsInitialized) return;

            currentMainSceneRoot = FindSceneRootInActiveScene<MainSceneRoot>();
            if (currentMainSceneRoot != null)
            {
                StartCoroutine(currentMainSceneRoot.InitRoutine());
            }

            base.Initialize();
        }

        // ==========================================
        // --- 메인 씬 (Single Load) ---
        // ==========================================
        public void LoadMainScene(string sceneName, object payload = null)
        {
            StartCoroutine(LoadMainSceneRoutine(sceneName, payload));
        }

        private IEnumerator LoadMainSceneRoutine(string sceneName, object payload)
        {
            if (currentMainSceneRoot != null)
            {
                yield return StartCoroutine(currentMainSceneRoot.UnInitRoutine());
                currentMainSceneRoot = null;
            }

            // 기존에 떠있던 Additive 씬 목록 초기화 (Single 로드 시 모두 닫히므로)
            activeAdditiveScenes.Clear();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncLoad.isDone);

            currentMainSceneRoot = FindSceneRootInActiveScene<MainSceneRoot>();

            if (currentMainSceneRoot != null)
            {
                // InitRoutine 실행 전 Payload 전달
                currentMainSceneRoot.SetupContext(payload);

                yield return StartCoroutine(currentMainSceneRoot.InitRoutine());
                currentMainSceneRoot.OnSceneReady();
            }
            else
            {
                Debug.LogWarning($"[{sceneName}] 씬의 최상단에 MainSceneRoot를 상속받은 스크립트가 없습니다.");
            }
        }

        // ==========================================
        // --- 배경 씬 (Additive Load) ---
        // ==========================================
        public void LoadAdditiveScene(string sceneName)
        {
            StartCoroutine(LoadAdditiveSceneRoutine(sceneName));
        }

        private IEnumerator LoadAdditiveSceneRoutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncLoad.isDone);

            Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            AdditiveSceneRoot additiveRoot = FindSceneRootInScene<AdditiveSceneRoot>(loadedScene);

            if (additiveRoot != null)
            {
                // Main 씬 주입
                additiveRoot.InjectMainScene(currentMainSceneRoot);
                
                // Additive 씬 목록에 추가
                activeAdditiveScenes.Add(additiveRoot);

                // Pause 로직 처리 (자신이 메인 씬을 일시정지시켜야 한다면)
                if (additiveRoot.ShouldPauseMainScene && currentMainSceneRoot != null)
                {
                    currentMainSceneRoot.OnPause();
                }

                yield return StartCoroutine(additiveRoot.InitRoutine());
                additiveRoot.OnSceneReady(); // Additive 씬도 준비 완료 콜백 호출
            }
        }

        public void UnloadAdditiveScene(string sceneName)
        {
            StartCoroutine(UnloadAdditiveSceneRoutine(sceneName));
        }

        private IEnumerator UnloadAdditiveSceneRoutine(string sceneName)
        {
            Scene targetScene = SceneManager.GetSceneByName(sceneName);
            
            if (targetScene.IsValid() && targetScene.isLoaded)
            {
                AdditiveSceneRoot additiveRoot = FindSceneRootInScene<AdditiveSceneRoot>(targetScene);
                if (additiveRoot != null)
                {
                    yield return StartCoroutine(additiveRoot.UnInitRoutine());
                    
                    // 목록에서 제거 및 Resume 로직 재평가
                    activeAdditiveScenes.Remove(additiveRoot);
                    UpdateMainScenePauseState();
                }
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }
        
        private void UpdateMainScenePauseState()
        {
            if (currentMainSceneRoot == null) return;

            bool shouldBePaused = false;
            foreach (var additive in activeAdditiveScenes)
            {
                if (additive.ShouldPauseMainScene)
                {
                    shouldBePaused = true;
                    break;
                }
            }

            if (shouldBePaused)
            {
                currentMainSceneRoot.OnPause();
            }
            else
            {
                currentMainSceneRoot.OnResume();
            }
        }

        // ==========================================
        // --- 최상단 오브젝트 탐색 최적화 ---
        // ==========================================
        private T FindSceneRootInActiveScene<T>() where T : SceneRoot
        {
            return FindSceneRootInScene<T>(SceneManager.GetActiveScene());
        }

        private T FindSceneRootInScene<T>(Scene targetScene) where T : SceneRoot
        {
            if (!targetScene.IsValid() || !targetScene.isLoaded) return null;

            GameObject[] rootObjects = targetScene.GetRootGameObjects();
            foreach (GameObject go in rootObjects)
            {
                T root = go.GetComponent<T>();
                if (root != null)
                {
                    return root;
                }
            }
            return null;
        }
    }
}
