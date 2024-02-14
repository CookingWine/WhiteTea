using UnityEditor;
using WhiteTea.HotfixLogic;

namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 游戏数据更新
    /// </summary>
    public static class WhiteTeaGameDataUpdate
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        private static bool m_IsInitialized = false;

        private static AppHotfixConfig m_AppHotfixConfig;

        public static AppHotfixConfig AppHotfixConfigs
        {
            get
            {
                return m_AppHotfixConfig;
            }
        }
        [InitializeOnLoadMethod]
        private static void Init( )
        {
            if(m_IsInitialized)
            {
                return;
            }
            EditorApplication.update += EditorUpdate;


            //m_AppHotfixConfig = await AppHotfixConfig.GetAppHotfixConfig( );

            m_IsInitialized = true;
        }

        private static void EditorUpdate( )
        {
            if(!m_IsInitialized)
            {
                return;
            }

        }
    }
}
