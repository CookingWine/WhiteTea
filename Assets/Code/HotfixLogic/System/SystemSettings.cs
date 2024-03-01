using UnityGameFramework.Runtime;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class SystemSettings
    {
        private static SystemSettings m_Instance;

        /// <summary>
        /// 是否初始化配置
        /// </summary>
        private bool m_InitializedSetting;

        public static SystemSettings Instance
        {
            get
            {
                m_Instance ??= new SystemSettings( );
                return m_Instance;
            }
        }

        /// <summary>
        /// 系统设置
        /// </summary>
        private SystemSettings( )
        {
            m_InitializedSetting = false;
            LoadSystemSettings( );
        }
        /// <summary>
        /// 声音设置
        /// </summary>
        public GameVolume GameVolumeSetting { get; private set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserSettings GameUser { get; private set; }


        /// <summary>
        /// 加载系统配置
        /// </summary>
        private void LoadSystemSettings( )
        {
            GameUser = new UserSettings( );

            GameVolumeSetting = new GameVolume( );
        }

        /// <summary>
        /// 初始化系统配置
        /// </summary>
        public void InitSystemSetting( )
        {
            //防止其它地方调用，导致多次初始化配置
            if(m_InitializedSetting)
            {
                return;
            }
            //进入游戏时不静音
            GameVolumeSetting.TotalVolumeMute = false;

            m_InitializedSetting = true;
        }
        /// <summary>
        /// 改变游戏总音量的状态
        /// </summary>
        /// <returns>是否静音</returns>
        public bool ChangeTotalGameVolumeState( )
        {
            GameVolumeSetting.TotalVolumeMute = !GameVolumeSetting.TotalVolumeMute;
            return GameVolumeSetting.TotalVolumeMute;
        }
    }
}
