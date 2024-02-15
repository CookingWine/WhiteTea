#if UNITY_EDITOR
using GameFramework;
using GameFramework.Resource;
using System.Threading.Tasks;
using WhiteTea.BuiltinRuntime;
#endif
using UnityEngine;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// hotfix配置
    /// </summary>
    [CreateAssetMenu(fileName = "AppHotfixConfig" , menuName = "ScriptableObject/AppConfig【热更配置】" , order = 1)]
    public class AppHotfixConfig:ScriptableObject
    {
        [SerializeField]
        private string[] m_DataTables;
        /// <summary>
        /// 预加载数据表
        /// </summary>
        public string[] DataTables
        {
            get
            {
                return m_DataTables;
            }
        }
        [SerializeField]
        private string[] m_AotFileList;

        /// <summary>
        /// aot文件列表
        /// </summary>
        public string[] AotFileList
        {
            get
            {
                return m_AotFileList;
            }
        }

        /// <summary>
        /// 热更内游戏流程
        /// </summary>
        [SerializeField]
        private string[] m_HotfixProcedures;

        /// <summary>
        /// 热更内的游戏流程
        /// </summary>
        public string[] HotfixProcedure
        {
            get
            {
                return m_HotfixProcedures;
            }
        }
        [Header("热更新的资源组")]
        [SerializeField]
        private string[] m_MustResourceGroup;

        /// <summary>
        /// 必须的资源组
        /// </summary>
        public string[] MustResourceGroup
        {
            get
            {
                return m_MustResourceGroup;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 获取热更配置
        /// </summary>
        /// <returns></returns>
        public static async Task<AppHotfixConfig> GetAppHotfixConfig( )
        {
            var config = BuiltinRuntimeUtility.AssetsUtility.GetHotfixUISpritesAssets(WTGame.AppBuiltinConfigs.AppHotfixConfig);
            AppHotfixConfig data = await LoadAssets<AppHotfixConfig>(config);
            return data;
        }

        private static Task<T> LoadAssets<T>(string assetName)
        {
            TaskCompletionSource<T> loadAssetTcs = new TaskCompletionSource<T>( );
            WTGame.Resource.LoadAsset(assetName , typeof(T) , new LoadAssetCallbacks(
                (tempAssetName , asset , duration , userdata) =>
                {
                    var source = loadAssetTcs;
                    loadAssetTcs = null;
                    if(asset is T tAsset)
                    {
                        source.SetResult(tAsset);
                    }
                    else
                    {
                        Debug.LogError($"Load asset failure load type is {asset.GetType( )} but asset type is {typeof(T)}.");
                        source.SetException(new GameFrameworkException($"Load asset failure load type is {asset.GetType( )} but asset type is {typeof(T)}."));
                    }
                } ,
                (tempAssetName , status , errorMessage , userdata) =>
                {
                    Debug.LogError(errorMessage);
                    loadAssetTcs.SetException(new GameFrameworkException(errorMessage));
                }
            ));

            return loadAssetTcs.Task;
        }
#endif
    }
}
