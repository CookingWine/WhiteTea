using GameFramework;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 资源加载工具
    /// </summary>
    public class AssetUtility
    {
        /// <summary>
        /// 获取配置资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <param name="fromBytes">是否是bytes文件</param>
        /// <returns>配置资源路径</returns>
        public static string GetConfigAsset(string assetsName , bool fromBytes)
        {
            return Utility.Text.Format("Assets/HotfixAssets/BuiltinConfigs/{0}.{1}" , assetsName , fromBytes ? "bytes" : "txt");
        }

        /// <summary>
        /// 获取数据表资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <param name="fromBytes">是否是bytes文件</param>
        /// <returns>数据表资源路径</returns>
        public static string GetDataTableAsset(string assetsName , bool fromBytes)
        {
            return Utility.Text.Format("Assets/HotfixAssets/DataTables/{0}.{1}" , assetsName , fromBytes ? "bytes" : "txt");
        }

        /// <summary>
        /// 获取本地字典资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <param name="fromBytes">是否是bytes文件</param>
        /// <returns>字典资源路径</returns>
        public static string GetDictionaryAsset(string assetsName , bool fromBytes)
        {
            return Utility.Text.Format("Assets/HotfixAssets/Localization/{0}/Dictionaries/{1}.{2}" , GameCollectionEntry.Localization.Language.ToString( ) , assetsName , fromBytes ? "bytes" : "xml");
        }

        /// <summary>
        /// 获取字体资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>字体资源路径</returns>
        public static string GetFontAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/Fonts/{0}" , assetsName);
        }

        /// <summary>
        /// 获取场景资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>场景资源路径</returns>
        public static string GetSceneAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/Scenes/{0}.unity" , assetsName);
        }
        /// <summary>
        /// 获取音乐资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>音乐资源路径</returns>
        public static string GetMusicAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/Music/{0}.mp3" , assetsName);
        }
        /// <summary>
        /// 获取音效资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>音效资源路径</returns>
        public static string GetSoundAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/Sounds/{0}.wav" , assetsName);
        }
        /// <summary>
        /// 获取UI预制体资源
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>UI预制体路径</returns>
        public static string GetUIFormAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/UI/UIForms/{0}.prefab" , assetsName);
        }

        /// <summary>
        /// 获取UI音效资源路径
        /// </summary>
        /// <param name="assetsName">资源名</param>
        /// <returns>UI音效资源路径</returns>
        public static string GetUISoundAsset(string assetsName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/UI/UISounds/{0}.mp3" , assetsName);
        }
        /// <summary>
        /// 获取UIsprite资源路径
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <returns>UIsprite资源路径</returns>
        public static string GetUISpriteAsset(string assetName)
        {
            return string.Format("Assets/HotfixAssets/UI/UISprites/{0}" , assetName);
        }
        /// <summary>
        /// 获取UIItem资源路径
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <returns>UIItem资源路径</returns>
        public static string GetUIItemAsset(string assetName)
        {
            return string.Format("Assets/HotfixAssets/UI/UIItems/{0}.prefab" , assetName);
        }

        /// <summary>
        /// 获取热更dll资源路径
        /// </summary>
        /// <param name="assetName">热更dll资源名</param>
        /// <returns>热更dll资源路径</returns>
        public static string GetHotfixDllAsset(string assetName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/HotfixDLL/{0}.bytes" , assetName);
        }

        /// <summary>
        /// 获取元数据资源路径
        /// </summary>
        /// <param name="assetName">dll文件名</param>
        /// <returns>补充元数据的路径</returns>
        public static string GetAotMetadataAsset(string assetName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/AotMetadata/{0}.bytes" , assetName);
        }

        /// <summary>
        /// 获取ScriptableObject资源路径
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <returns>ScriptableObject资源路径</returns>
        public static string GetScriptableObjectAsset(string assetName)
        {
            return Utility.Text.Format("Assets/HotfixAssets/ScriptableObjectAssets/{0}.asset" , assetName);
        }
    }
}
