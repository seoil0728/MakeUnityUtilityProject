using UnityEngine;

namespace SWUtility.UIManager
{
    /*
     * This Class is the Base of UI Elements.
     *
     * Basic Flow
     * Init() -> SetupData() -> Open() -> Close() -> ReleaseData() -> UnInit()
     * if Open called, SetupData() is automatically called Before Open().
     * But Close and Release is not same.
     * Close() and ReleaseData() is separated.
     * PopupUI, PageUI, HUDUI is not same logic flow.
     * But If you want to call ReleaseData(), recommend to call after Close().
     * 
     */
    public abstract class UIBase : MonoBehaviour
    {
        public abstract bool isOpen { get; }
        
        /// <summary>
        /// UI Should Init First.
        /// UI Base Setup Method.
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// Close and Release all Datas.
        /// After UnInit, this UI cannot open.
        /// </summary>
        public abstract void UnInit();
        /// <summary>
        /// Before we open this UI, Setup Datas if neccesary.
        /// This method is called before Open method.
        /// </summary>
        /// <param name="param"></param>
        public abstract void SetupData(params object[] param);
        /// <summary>
        /// If UI called Close, Release Datas After Close Method call.
        /// </summary>
        public abstract void ReleaseData();
        /// <summary>
        /// UIObject is Activate, and its UI is shown.
        /// This Method is called after SetupData().
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// UIObject is Deactivate, and its UI is not shown.
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// This is Update Code.
        /// MoveFrame() is called by UIManager.
        /// </summary>
        public abstract void MoveFrame();
        }
}
