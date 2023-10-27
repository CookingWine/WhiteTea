using UnityEngine;
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

        private LogReportingComponent m_LogReporting;

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private void InitCustomComponents( )
        {
            //创建日志上报
            m_LogReporting = LogReportingComponent.Create( );

            BuiltinData = GameEntry.GetComponent<BuiltinDataComponent>( );
            Hybridclr = GameEntry.GetComponent<HybridclrComponent>( );
        }
        private void Update( )
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                m_LogReporting.Update( );
            }
        }
    }
}
