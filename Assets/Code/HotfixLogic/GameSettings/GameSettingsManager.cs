namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 游戏设置
    /// </summary>
    public partial class GameSettingsManager
    {
        /// <summary>
        /// 游戏设置
        /// </summary>
        private GameSettingsManager( )
        {
            BasicSetting = GameBasicSetting.LoadGameBasicSetting( );
            SoundSetting = GameSoundSetting.LoadGameSoundSetting( );
            PictureQualitySetting = GamePictureQualitySetting.LoadGamePictureQualitySetting( );
        }

        /// <summary>
        /// 实例,保证全局唯一
        /// </summary>
        private static GameSettingsManager m_Instance;

        /// <summary>
        /// 加载游戏设置
        /// </summary>
        /// <returns></returns>
        public static GameSettingsManager LoadGameSettings( )
        {
            if(m_Instance == null)
            {
                m_Instance = new GameSettingsManager( );
            }
            return m_Instance;
        }

        /// <summary>
        /// 基础设置
        /// </summary>
        public GameBasicSetting BasicSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// 游戏画质设置
        /// </summary>
        public GamePictureQualitySetting PictureQualitySetting
        {
            get;
            private set;
        }

        /// <summary>
        /// 音效设置
        /// </summary>
        public GameSoundSetting SoundSetting
        {
            get;
            private set;
        }

    }
}
