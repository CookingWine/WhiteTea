namespace UGHGame.HotfixLogic
{
    public partial class GameSettingsManager
    {
        /// <summary>
        /// 游戏音效设置
        /// </summary>
        public class GameSoundSetting
        {
            /// <summary>
            /// 音效设置
            /// </summary>
            private GameSoundSetting( )
            {

            }
            /// <summary>
            /// 实例
            /// </summary>
            private static GameSoundSetting m_Instance;

            /// <summary>
            /// 加载音效设置
            /// </summary>
            /// <returns></returns>
            public static GameSoundSetting LoadGameSoundSetting( )
            {
                if(m_Instance == null)
                {
                    m_Instance = new GameSoundSetting( );
                }
                return m_Instance;
            }
        }
    }
}
