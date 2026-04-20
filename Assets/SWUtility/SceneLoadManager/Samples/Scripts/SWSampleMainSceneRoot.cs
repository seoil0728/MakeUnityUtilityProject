using System.Collections;
using UnityEngine;
using SWUtility.SceneLoadManager;

namespace SWUtility.Sample
{
    /// <summary>
    /// SWSampleMain 씬의 최상단 게임오브젝트에 부착되는 루트 스크립트 샘플입니다.
    /// </summary>
    public class SWSampleMainSceneRoot : MainSceneRoot
    {
        private string welcomeMessage = "기본 환영 메시지";

        // 1. 데이터 수신 (선택 사항)
        public override void SetupContext(object payload)
        {
            base.SetupContext(payload);
            
            // 이전 씬에서 SceneLoadManager.Instance.LoadMainScene("SWSampleMain", "안녕하세요!"); 처럼 호출했을 때
            if (payload is string message)
            {
                welcomeMessage = message;
                Debug.Log($"[SWSampleMain] SetupContext: Payload 수신 완료 - {message}");
            }
        }

        // 2. 비동기 초기화
        public override IEnumerator InitRoutine()
        {
            Debug.Log("[SWSampleMain] InitRoutine: 씬 초기화 시작 (에셋 로드 대기 중...)");
            
            // 시간이 걸리는 작업 (예: 어드레서블 로드, 서버 통신 등)을 코루틴으로 대기합니다.
            yield return new WaitForSeconds(1.0f); 
            
            Debug.Log("[SWSampleMain] InitRoutine: 씬 초기화 완료");
        }

        // 3. 초기화 완료 및 씬 시작
        public override void OnSceneReady()
        {
            Debug.Log($"[SWSampleMain] OnSceneReady: 씬 준비 완료! 시작 메시지: {welcomeMessage}");
            
            // 테스트용: 3초 뒤에 백그라운드(Additive) 씬을 로드해봅니다.
            Invoke(nameof(TestLoadAdditiveScene), 3.0f);
        }

        // 4. 정리
        public override IEnumerator UnInitRoutine()
        {
            Debug.Log("[SWSampleMain] UnInitRoutine: 다음 씬으로 넘어가기 전 데이터 정리 중...");
            yield return null;
        }

        // 5. Additive 씬에 의한 상태 제어
        public override void OnPause()
        {
            Debug.Log("<color=orange>[SWSampleMain] OnPause: Additive 씬에 의해 메인 씬이 일시정지 되었습니다. (Time.timeScale = 0)</color>");
            Time.timeScale = 0f;
        }

        public override void OnResume()
        {
            Debug.Log("<color=green>[SWSampleMain] OnResume: Additive 씬이 닫혀 메인 씬이 재개되었습니다. (Time.timeScale = 1)</color>");
            Time.timeScale = 1f;
        }

        // --- 테스트용 함수 ---
        private void TestLoadAdditiveScene()
        {
            Debug.Log("[SWSampleMain] SWSampleBackground 씬을 Additive 모드로 로드합니다.");
            SceneLoadManager.SceneLoadManager.Instance.LoadAdditiveScene("SWSampleBackground");
        }
    }
}
