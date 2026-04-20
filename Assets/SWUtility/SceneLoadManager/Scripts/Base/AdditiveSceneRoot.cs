using UnityEngine;

namespace SWUtility.SceneLoadManager
{
    public abstract class AdditiveSceneRoot : SceneRoot
    {
        /// <summary>
        /// 바탕이 되는 메인 씬에 대한 참조입니다.
        /// </summary>
        protected MainSceneRoot ParentMainScene { get; private set; }

        /// <summary>
        /// 이 Additive 씬이 활성화되어 있는 동안 부모 메인 씬을 일시정지(Pause) 시킬지 여부입니다.
        /// </summary>
        public virtual bool ShouldPauseMainScene => false;

        /// <summary>
        /// SceneLoadManager에 의해 바탕이 되는 메인 씬이 주입됩니다.
        /// </summary>
        public void InjectMainScene(MainSceneRoot mainScene)
        {
            ParentMainScene = mainScene;
            OnMainSceneInjected();
        }

        /// <summary>
        /// 메인 씬이 주입된 직후에 호출되는 콜백입니다. 필요한 경우 오버라이드하여 사용하세요.
        /// </summary>
        protected virtual void OnMainSceneInjected() { }
    }
}
