using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// app热更配置
    /// </summary>
    [CreateAssetMenu(fileName = "AppHotfixConfig" , menuName = "AppConfig/AppHotfixConfig" , order = 1)]
    public class AppHotfixConfig:ScriptableObject
    {
        [SerializeField]
        private string[] m_DataTable;
        /// <summary>
        /// 数据表
        /// </summary>
        public string[] DataTable
        {
            get
            {
                return m_DataTable;
            }
        }
        [SerializeField]
        private string[] m_ConfigTable;
        /// <summary>
        /// 配置表
        /// </summary>
        public string[] ConfigTable
        {
            get
            {
                return m_ConfigTable;
            }
        }
        [SerializeField]
        private string[] m_HotfixProcedures;
        /// <summary>
        /// 热更流程
        /// </summary>
        public string[] HotfixProcedures
        {
            get { return m_HotfixProcedures; }
        }
        [SerializeField]
        private string[] m_MetadataAotData;
        /// <summary>
        /// MetadataAot数据
        /// </summary>
        public string[] MetadataAotData
        {
            get
            {
                return m_MetadataAotData;
            }
        }

        [SerializeField]
        private string[] m_HotfixFileList;
        /// <summary>
        /// 热更文件列表
        /// </summary>
        public string[] HotfixFileList
        {
            get
            {
                return m_HotfixFileList;
            }
        }
    }
}
