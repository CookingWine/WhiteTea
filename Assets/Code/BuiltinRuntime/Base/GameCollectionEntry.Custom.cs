using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public partial class GameCollectionEntry
    {

        /// <summary>
        /// Hybridclr热更组件
        /// </summary>
        public HybridclrComponent Hybridclr
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private void InitCustomComponents( )
        {
            Hybridclr = GameEntry.GetComponent<HybridclrComponent>( );
        }
    }
}
