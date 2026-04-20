using System.Collections;
using UnityEngine;

namespace SWUtility.SceneLoadManager
{
    public abstract class MainSceneRoot : SceneRoot
    {
        // 이전 씬에서 전달된 데이터를 저장할 수 있는 변수
        protected object ContextPayload { get; private set; }

        /// <summary>
        /// 씬 로드 직후, InitRoutine이 실행되기 전에 호출되어 데이터를 전달받습니다.
        /// </summary>
        public virtual void SetupContext(object payload)
        {
            ContextPayload = payload;
        }

        /// <summary>
        /// Additive 씬이 이 메인 씬의 동작을 일시정지시킬 때 호출됩니다.
        /// </summary>
        public virtual void OnPause()
        {
            // 하위 클래스에서 일시정지 로직 (예: Time.timeScale = 0, 입력 차단 등) 구현
        }

        /// <summary>
        /// Additive 씬이 언로드되어 이 메인 씬의 동작이 재개될 때 호출됩니다.
        /// </summary>
        public virtual void OnResume()
        {
            // 하위 클래스에서 재개 로직 (예: Time.timeScale = 1, 입력 허용 등) 구현
        }
    }
}
