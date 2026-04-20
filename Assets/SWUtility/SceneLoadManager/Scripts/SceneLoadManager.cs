using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWUtility.SceneLoadManager
{
    public class SceneLoadManager : GlobalSingleton<SceneLoadManager>
    {
        private MainSceneRoot currentMainSceneRoot;

        public override void Initialize()
        {
            if (IsInitialized) return;

            // Boot 씬에서 시작했을 때, 현재 씬의 Root를 찾아 최초 1회 Init 실행
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
        public void LoadMainScene(string sceneName)
        {
            StartCoroutine(LoadMainSceneRoutine(sceneName));
        }

        private IEnumerator LoadMainSceneRoutine(string sceneName)
        {
            // 1. 기존 씬의 UnInit 호출 및 대기 (코루틴이 끝날 때까지 기다림)
            if (currentMainSceneRoot != null)
            {
                yield return StartCoroutine(currentMainSceneRoot.UnInitRoutine());
                currentMainSceneRoot = null;
            }

            // 2. 비동기 씬 로드
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            
            // (선택) 로딩이 90%에서 멈추게 하여 로딩 스크린 연출을 넣을 수 있는 옵션
            // asyncLoad.allowSceneActivation = false;
            // while (asyncLoad.progress < 0.9f) { yield return null; }
            // asyncLoad.allowSceneActivation = true;

            yield return new WaitUntil(() => asyncLoad.isDone);

            // 3. 새로운 씬의 SceneRoot 탐색
            currentMainSceneRoot = FindSceneRootInActiveScene<MainSceneRoot>();

            // 4. 새로운 씬의 Init 호출 및 대기
            if (currentMainSceneRoot != null)
            {
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

            // 로드된 Additive 씬을 이름으로 찾아 해당 씬 안에서만 탐색
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            AdditiveSceneRoot additiveRoot = FindSceneRootInScene<AdditiveSceneRoot>(loadedScene);

            if (additiveRoot != null)
            {
                yield return StartCoroutine(additiveRoot.InitRoutine());
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
                }
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        // ==========================================
        // --- 최상단 오브젝트 탐색 최적화 (GetRootGameObjects) ---
        // ==========================================
        private T FindSceneRootInActiveScene<T>() where T : SceneRoot
        {
            return FindSceneRootInScene<T>(SceneManager.GetActiveScene());
        }

        private T FindSceneRootInScene<T>(Scene targetScene) where T : SceneRoot
        {
            if (!targetScene.IsValid() || !targetScene.isLoaded) return null;

            // 씬 내의 '최상단 게임오브젝트'들만 배열로 가져와 검사 (성능 최적화)
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

