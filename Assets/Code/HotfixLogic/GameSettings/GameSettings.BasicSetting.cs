using UGHGame.BuiltinRuntime;

namespace UGHGame.HotfixLogic
{
    public partial class GameSettingsManager
    {
        /// <summary>
        /// 基础设置
        /// </summary>
        public class GameBasicSetting
        {
            /// <summary>
            /// 基础设置的实例
            /// </summary>
            private static GameBasicSetting m_Instance;

            /// <summary>
            /// 基础设置
            /// </summary>
            private GameBasicSetting( )
            {

            }

            /// <summary>
            /// 加载基础设置
            /// </summary>
            /// <returns></returns>
            public static GameBasicSetting LoadGameBasicSetting( )
            {
                if(m_Instance == null)
                {
                    m_Instance = new GameBasicSetting( );
                }
                return m_Instance;
            }

            /// <summary>
            /// 游戏帧率
            /// </summary>
            public enum GameFrameRate
            {
                /// <summary>
                /// 30帧
                /// </summary>
                ThirtyFrame,
                /// <summary>
                /// 45帧
                /// </summary>
                FortyFiveFrame,
                /// <summary>
                /// 60帧
                /// </summary>
                SixtyFrame,
                /// <summary>
                /// 120帧
                /// </summary>
                OneHundredAndTwenty
            }

            /// <summary>
            /// 设置游戏帧率
            /// </summary>
            /// <param name="gameFrameRate"></param>
            public void SetGameFrameRate(GameFrameRate gameFrameRate)
            {
                GameCollectionEntry.Base.FrameRate = GetGameFrameRate(gameFrameRate);
            }

            /// <summary>
            /// 获取游戏帧率
            /// </summary>
            /// <param name="rate">帧率</param>
            /// <returns></returns>
            private int GetGameFrameRate(GameFrameRate rate)
            {
                switch(rate)
                {
                    case GameFrameRate.ThirtyFrame:
                        return 30;
                    case GameFrameRate.FortyFiveFrame:
                        return 45;
                    case GameFrameRate.SixtyFrame:
                        return 60;
                    case GameFrameRate.OneHundredAndTwenty:
                        return 120;
                    default:
                        return 60;
                }
            }
        }
    }
}
