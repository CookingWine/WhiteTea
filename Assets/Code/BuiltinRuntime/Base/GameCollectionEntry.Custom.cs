using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public partial class GameCollectionEntry
    {
        /// <summary>
        /// 自定义组件
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
            BuiltinData = GameEntry.GetComponent<BuiltinDataComponent>( );
            Hybridclr = GameEntry.GetComponent<HybridclrComponent>( );
        }
    }
}
