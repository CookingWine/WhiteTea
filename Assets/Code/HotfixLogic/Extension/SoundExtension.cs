using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

namespace WhiteTea.HotfixLogic
{
    public static class SoundExtension
    {
        /// <summary>
        /// 淡出音量持续时间
        /// </summary>
        private const float FadeVolumeDuration = 1f;

        /// <summary>
        /// 音乐ID
        /// </summary>
        private static int? s_MusicSerialId = null;

        public static int? PlayMusic(this SoundComponent soundComponent , int musicId , object userData = null)
        {
            soundComponent.StopMusic( );

            IDataTable<DRMusic> dtMusic = WTGame.DataTable.GetDataTable<DRMusic>( );
            DRMusic drMusic = dtMusic.GetDataRow(musicId);
            if(drMusic == null)
            {
                Log.Warning("Can not load music '{0}' from data table." , musicId.ToString( ));
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create( );
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.FadeInSeconds = FadeVolumeDuration;
            playSoundParams.SpatialBlend = 0f;
            s_MusicSerialId = soundComponent.PlaySound(BuiltinRuntimeUtility.AssetsUtility.GetMusicAssets(drMusic.AssetName) , "Music" , 20 , playSoundParams , null , userData);
            return s_MusicSerialId;
        }
        public static void StopMusic(this SoundComponent soundComponent)
        {
            if(!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value , FadeVolumeDuration);
            s_MusicSerialId = null;
        }
    }
}
