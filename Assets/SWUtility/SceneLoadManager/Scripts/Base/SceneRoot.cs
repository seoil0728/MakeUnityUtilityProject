using System.Collections;
using UnityEngine;

namespace SWUtility.SceneLoadManager
{
    public abstract class SceneRoot : MonoBehaviour
    {
        // 1. 비동기 로드 및 준비 작업 (코루틴 대기 가능)
        public abstract IEnumerator InitRoutine();

        // [추가] 2. InitRoutine 완료 및 로딩 스크린 해제 후 호출되는 씬의 진짜 시작점 (동기 함수)
        // abstract 대신 virtual로 하여 필요한 씬에서만 구현하도록 합니다.
        public virtual void OnSceneReady() { }

        // 3. 씬 파괴 직전 정리 작업
        public abstract IEnumerator UnInitRoutine();
    }
}