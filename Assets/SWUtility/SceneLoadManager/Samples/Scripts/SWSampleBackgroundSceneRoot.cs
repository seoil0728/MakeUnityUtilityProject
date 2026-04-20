using System.Collections;
using UnityEngine;
using SWUtility.SceneLoadManager;

namespace SWUtility.Sample
{
    /// <summary>
    /// SWSampleBackground 씬의 최상단 게임오브젝트에 부착되는 루트 스크립트 샘플입니다.
    /// </summary>
    public class SWSampleBackgroundSceneRoot : AdditiveSceneRoot
    {
        // 이 씬이 열려있는 동안 메인 씬을 멈출 것인지 결정합니다. (예: 전체화면 인벤토리, 일시정지 메뉴 등)
        public override bool ShouldPauseMainScene => true;

        // 1. 메인 씬 주입 완료 콜백
        protected override void OnMainSceneInjected()
        {
            // ParentMainScene 프로퍼티를 통해 현재 깔려있는 메인 씬(SWSampleMainSceneRoot)에 접근할 수 있습니다.
            if (ParentMainScene != null)
            {
                Debug.Log($"[SWSampleBackground] OnMainSceneInjected: 부모 씬({ParentMainScene.gameObject.name}) 연결 완료.");
            }
        }

        // 2. 비동기 초기화
        public override IEnumerator InitRoutine()
        {
            Debug.Log("[SWSampleBackground] InitRoutine: Additive 씬 에셋 로드 중...");
            yield return new WaitForSeconds(0.5f);
            Debug.Log("[SWSampleBackground] InitRoutine: 초기화 완료.");
        }

        // 3. 초기화 완료 및 씬 시작
        public override void OnSceneReady()
        {
            Debug.Log("[SWSampleBackground] OnSceneReady: Additive 씬 활성화 완료!");
            
            // 테스트용: 3초 뒤에 스스로를 닫아봅니다.
            Invoke(nameof(TestUnloadSelf), 3.0f);
        }

        // 4. 정리
        public override IEnumerator UnInitRoutine()
        {
            Debug.Log("[SWSampleBackground] UnInitRoutine: Additive 씬 닫는 중...");
            yield return null;
        }

        // --- 테스트용 함수 ---
        private void TestUnloadSelf()
        {
            Debug.Log("[SWSampleBackground] 스스로를 언로드(Unload) 합니다.");
            SceneLoadManager.SceneLoadManager.Instance.UnloadAdditiveScene("SWSampleBackground");
        }
    }
}
