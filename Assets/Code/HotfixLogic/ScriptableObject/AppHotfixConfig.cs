using UnityEngine;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// hotfix配置
    /// </summary>
    [CreateAssetMenu(fileName = "AppHotfixConfig" , menuName = "ScriptableObject/AppConfig【热更配置】" , order = 1)]
    public class AppHotfixConfig:ScriptableObject
    {
        [SerializeField]
        private string[] m_DataTables;
        /// <summary>
        /// 预加载数据表
        /// </summary>
        public string[] DataTables
        {
            get
            {
                return m_DataTables;
            }
        }
        [SerializeField]
        private string[] m_AotFileList;

        /// <summary>
        /// aot文件列表
        /// </summary>
        public string[] AotFileList
        {
            get
            {
                return m_AotFileList;
            }
        }

        /// <summary>
        /// 热更内游戏流程
        /// </summary>
        [SerializeField]
        private string[] m_HotfixProcedures;

        /// <summary>
        /// 热更内的游戏流程
        /// </summary>
        public string[] HotfixProcedure
        {
            get
            {
                return m_HotfixProcedures;
            }
        }
        /// <summary>
        /// 热更内使用得sdk
        /// </summary>
        [SerializeField]
        private string[] m_HotfixGameSDK;
        /// <summary>
        /// 热更内使用得sdk
        /// </summary>
        public string[] HotfixGameSDK
        {
            get
            {
                return new string[] { "WhiteTea.HotfixLogic.AndroidCommunication" };
                //return m_HotfixGameSDK;
            }
        }

        /// <summary>
        /// 热更新资源组
        /// </summary>
        [SerializeField]
        private string[] m_MustResourceGroup;

        /// <summary>
        /// 热更新资源组
        /// </summary>
        public string[] MustResourceGroup
        {
            get
            {
                return m_MustResourceGroup;
            }
        }
    }
}
