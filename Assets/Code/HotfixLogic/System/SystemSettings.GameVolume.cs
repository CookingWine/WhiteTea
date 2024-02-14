using WhiteTea.BuiltinRuntime;

namespace WhiteTea.HotfixLogic
{
    public partial class SystemSettings
    {
        /// <summary>
        /// 游戏音量
        /// </summary>
        private class GameVolume
        {
            ///// <summary>
            ///// 游戏背景音乐组
            ///// </summary>
            //private readonly string m_GameBGM = "Music";
            ///// <summary>
            ///// 游戏音效组
            ///// </summary>
            //private readonly string m_GameSound = "Sound";

            /// <summary>
            /// 游戏是否静音
            /// </summary>
            private bool m_TotalVolumeMute;

            /// <summary>
            /// 游戏是否静音
            /// </summary>
            public bool TotalVolumeMute
            {
                get => m_TotalVolumeMute;
                set
                {
                    m_TotalVolumeMute = value;
                    ChangeGameTotalVolume(m_TotalVolumeMute);

                    WTGame.Setting.SetBool(HotfixConstantUtility.GameSoundMuted , m_TotalVolumeMute);
                }
            }

            /// <summary>
            /// 游戏总音量大小
            /// </summary>
            private float m_TotalVolumeValue;

            /// <summary>
            /// 游戏总音量大小
            /// </summary>
            public float TotalVolumeValue
            {
                get => m_TotalVolumeValue;
                set
                {
                    m_TotalVolumeValue = value;
                    ChangeGameTotalVolumeSize(m_TotalVolumeValue);
                    WTGame.Setting.SetFloat(HotfixConstantUtility.GameSoundVolumeValue , m_TotalVolumeValue);
                }
            }

            /// <summary>
            /// 改变游戏音量的状态
            /// </summary>
            /// <param name="mute">是否静音</param>
            private void ChangeGameTotalVolume(bool mute)
            {
                foreach(var item in WTGame.Sound.GetAllSoundGroups( ))
                {
                    item.Mute = mute;
                }
            }

            /// <summary>
            /// 改变游戏音量大小
            /// </summary>
            /// <param name="size"></param>
            private void ChangeGameTotalVolumeSize(float size)
            {
                foreach(var item in WTGame.Sound.GetAllSoundGroups( ))
                {
                    item.Volume = size;
                }
            }

            //private void ChangeGameVolumeValue(float size , string groups)
            //{
            //    foreach(var item in WTGame.Sound.GetAllSoundGroups( ))
            //    {
            //        if(item.Name == groups)
            //        {
            //            item.Volume = size;
            //        }
            //    }
            //}
        }
    }
}
