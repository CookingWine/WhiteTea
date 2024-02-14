using UnityGameFramework.Runtime;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class SystemSettings
    {
        private static SystemSettings m_Instance;
        public static SystemSettings Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new SystemSettings( );
                    LoadSystemSettings( );
                }
                return m_Instance;
            }
        }
        /// <summary>
        /// 系统设置
        /// </summary>
        private SystemSettings( ) { }
        /// <summary>
        /// 声音设置
        /// </summary>
        private static GameVolume m_GameVolumeSettings;
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMute
        {
            get
            {
                return m_GameVolumeSettings.TotalVolumeMute;
            }
        }

        /// <summary>
        /// 获取游戏总音量大小
        /// </summary>
        public float GameTotalVolumeSize
        {
            get
            {
                return m_GameVolumeSettings.TotalVolumeValue;
            }
            set
            {
                m_GameVolumeSettings.TotalVolumeValue = value;
            }
        }

        /// <summary>
        /// 加载系统配置
        /// </summary>
        private static void LoadSystemSettings( )
        {
            m_GameVolumeSettings = new GameVolume( );
        }

        /// <summary>
        /// 初始化系统配置
        /// </summary>
        public void InitSystemSetting( )
        {
            //进入游戏时不静音
            m_GameVolumeSettings.TotalVolumeMute = false;

        }
        /// <summary>
        /// 改变游戏总音量的状态
        /// </summary>
        /// <returns>是否静音</returns>
        public bool ChangeTotalGameVolumeState( )
        {
            m_GameVolumeSettings.TotalVolumeMute = !m_GameVolumeSettings.TotalVolumeMute;
            return m_GameVolumeSettings.TotalVolumeMute;
        }
    }
}
