using UnityEngine;
using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    public partial class WTGame
    {
        /// <summary>
        /// 应用设置
        /// </summary>
        private AppBuiltinSettings m_AppBuiltinSettings;

        /// <summary>
        /// 自定义数据组件
        /// </summary>
        public static BuiltinDataComponent BuiltinData;

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private void InitCustomComponents( )
        {
            m_AppBuiltinSettings = Resources.Load<AppBuiltinSettings>("AppSettings");
            if(m_AppBuiltinSettings == null)
            {
                Log.Fatal("Load app builtin setting failure.");
                Shutdown(ShutdownType.Restart);
                return;
            }

            BuiltinData = GameEntry.GetComponent<BuiltinDataComponent>( );
        }

        /// <summary>
        /// 关闭游戏框架。
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型。</param>
        public static void Shutdown(ShutdownType shutdownType)
        {
            GameEntry.Shutdown(shutdownType);
        }
    }
}
