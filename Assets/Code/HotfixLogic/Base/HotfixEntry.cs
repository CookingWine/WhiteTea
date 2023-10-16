using GameFramework.Resource;
using UGHGame.BuiltinRuntime;
using UnityEngine;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 热更入口
    /// </summary>
    public class HotfixEntry
    {
        /// <summary>
        /// 加载AOT进度
        /// </summary>
        private static int m_CurrentProcess;
        /// <summary>
        /// 加载元数据完成
        /// </summary>
        private static bool m_LoadMetadataForAOTAssembliesFlage = false;
        /// <summary>
        /// 不可调用,供给HybridclrComponent使用【相当于Mono.Start】
        /// </summary>
        public static void Start( )
        {
            Debug.Log("开始启动热更");
            LoadMetadataForAOTData( );
        }
        /// <summary>
        /// 不可调用,供给HybridclrComponent使用【相当于Mono.Update】
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public static void Update(float elapseSeconds , float realElapseSeconds)
        {
            if(m_LoadMetadataForAOTAssembliesFlage)
            {

            }
        }

        /// <summary>
        /// 不可调用,供给HybridclrComponent使用
        /// </summary>
        public static void Shutdown( )
        {

        }
        /// <summary>
        /// 加载AOT元数据
        /// </summary>
        private static void LoadMetadataForAOTData( )
        {
            m_LoadMetadataForAOTAssembliesFlage = false;
            GameCollectionEntry.BuiltinData.GameMainInterface.SetProgressInfo(0 , "补充AOT数据");
            for(int i = 0; i < AppHotfixConfig.AotFileList.Length; i++)
            {
                GameCollectionEntry.Resource.LoadAsset(AssetUtility.GetAotMetadataAsset(AppHotfixConfig.AotFileList[i]) , new LoadAssetCallbacks((assetName , asset , duration , userData) =>
                {
                    byte[] bytes = ( asset as TextAsset ).bytes;
                    bool state = GameCollectionEntry.Hybridclr.LoadMetadataForAOTAssembly(bytes);
                    Debug.Log($"LoadMetadataForAOTAssembly:{userData}.加载状态:{state}");
                    GameCollectionEntry.BuiltinData.GameMainInterface.SetGameUpdateProgress(++m_CurrentProcess * 1.0f / AppHotfixConfig.AotFileList.Length);
                } , (assetName , status , errorMessage , userData) =>
                {
                    Debug.LogError($"Can not load aot dll '{assetName}' error message '{errorMessage}'.");
                }) , AppHotfixConfig.AotFileList[i]);
            }
        }
    }
}
