using UnityEngine;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// hotfix配置
    /// </summary>
    [CreateAssetMenu(fileName = "AppHotfixConfig" , menuName = "ScriptableObject/AppConfig【热更配置】" , order = 1)]
    public class AppHotfixConfig:ScriptableObject
    {
        [SerializeField]
        private string[] m_AotFileList;

        /// <summary>
        /// aot文件列表
        /// </summary>
        public string[] AotFileList
        {
            get
            {
                string[] aot = new string[] { "GameFramework" , "UnityGameFramework.Runtime" };
                if(m_AotFileList == null)
                {
                    return aot;
                }
                if(m_AotFileList.Length == 0)
                {
                    return aot;
                }
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
                string[] hotfxi = new string[] { "ProcedureHotfixEntry" };
                if(m_HotfixProcedures == null)
                {
                    return hotfxi;
                }
                if(m_HotfixProcedures.Length == 0)
                {
                    return hotfxi;
                }
                return m_HotfixProcedures;
            }
        }
    }
}
