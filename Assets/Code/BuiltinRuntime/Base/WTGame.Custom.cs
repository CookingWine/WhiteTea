using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    public partial class WTGame
    {
        /// <summary>
        /// 应用设置
        /// </summary>
        public static AppBuiltinSettings AppBuiltinConfigs
        {
            get;
            private set;
        }

        /// <summary>
        /// 自定义数据组件
        /// </summary>
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }
        /// <summary>
        /// Hybridclr热更组件
        /// </summary>
        public static HybridclrComponent Hybridclr
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private void InitCustomComponents( )
        {
            AppBuiltinConfigs = AppBuiltinSettings.Instance;
            if(AppBuiltinConfigs == null)
            {
                Log.Fatal("Load app builtin setting failure.");
                Shutdown(ShutdownType.Restart);
                return;
            }
            Log.Debug("Load app config success.");
            BuiltinData = GameEntry.GetComponent<BuiltinDataComponent>( );
            Hybridclr = GameEntry.GetComponent<HybridclrComponent>( );

            DontDestroyOnLoad(this);
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
