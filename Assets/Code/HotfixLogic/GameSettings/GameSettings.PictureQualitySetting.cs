using UGHGame.BuiltinRuntime;

namespace UGHGame.HotfixLogic
{
    public partial class GameSettingsManager
    {
        /// <summary>
        /// 游戏画质设置
        /// </summary>
        public class GamePictureQualitySetting
        {
          
            /// <summary>
            /// 实例
            /// </summary>
            private static GamePictureQualitySetting m_Instance;

            /// <summary>
            /// 加载游戏画质设置
            /// </summary>
            /// <returns></returns>
            public static GamePictureQualitySetting LoadGamePictureQualitySetting( )
            {
                if(m_Instance == null)
                {
                    m_Instance = new GamePictureQualitySetting( );
                }
                return m_Instance;
            }

            /// <summary>
            /// 游戏画质设置
            /// </summary>
            private GamePictureQualitySetting( )
            {

            }

        }
    }
}
