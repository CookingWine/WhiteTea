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
        [Header("热更新的资源组")]
        [SerializeField]
        private string[] m_MustResourceGroup;

        /// <summary>
        /// 必须的资源组
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
