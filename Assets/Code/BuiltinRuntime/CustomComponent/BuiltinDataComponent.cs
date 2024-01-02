using GameFramework;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 自定义组件
    /// </summary>
    public class BuiltinDataComponent:GameFrameworkComponent
    {
        [Header("热更组件父物体")]
        [SerializeField]
        private Transform m_HotfixModuleParentObject;

        /// <summary>
        /// 热更组件父物体
        /// </summary>
        public Transform HotfixModuleParentObject
        {
            get { return m_HotfixModuleParentObject; }
        }

        /// <summary>
        /// 构建信息文件
        /// </summary>
        [Header("构建信息文件")]
        [SerializeField]
        private TextAsset m_BuildInfoAsset;

        /// <summary>
        /// 游戏加载界面【主界面】
        /// </summary>
        public LoadingInterface GameMainInterface
        {
            get;
            private set;
        }

        /// <summary>
        /// 构建信息
        /// </summary>
        private BuildInfo m_BuildInfoData = null;

        /// <summary>
        /// 构建信息
        /// </summary>
        public BuildInfo BuildInfoData
        {
            get
            {
                return m_BuildInfoData;
            }
        }

        /// <summary>
        /// UI相机
        /// </summary>
        private Camera m_UICamera;

        /// <summary>
        /// UI相机
        /// </summary>
        public Camera UICamera
        {
            get
            {
                return m_UICamera;
            }
        }
        /// <summary>
        /// 初始化相机参数
        /// </summary>
        public void InitGameCamera( )
        {
            m_UICamera = GameObject.Find("UICamera").GetComponent<Camera>( );
        }

        /// <summary>
        /// 初始化构建信息
        /// </summary>
        public void InitBuildInfo( )
        {
            if(m_BuildInfoAsset == null || m_BuildInfoAsset.text.IsNullOrEmpty( ))
            {
                Log.Error("Build info can not be found or empty.");
                return;
            }
            m_BuildInfoData = Utility.Json.ToObject<BuildInfo>(m_BuildInfoAsset.text);
            if(m_BuildInfoData == null)
            {
                Log.Error("Parse build info failure.");
            }
            else
            {
                Log.Debug("Load build info success");
            }
        }

        /// <summary>
        /// 初始化字典配置
        /// </summary>
        public void InitDefaultDictionary( )
        {
            string path = "Builtin/Language/" + GetDefalutDictionaryConfigPath(GameCollectionEntry.Localization.Language) + ".xml";
            TextAsset languageAsset = Resources.Load<TextAsset>(path);
            if(languageAsset == null)
            {
                Log.Error("Load language config file failure.");
                return;
            }
            if(!GameCollectionEntry.Localization.ParseData(languageAsset.text))
            {
                Log.Warning("Parse language config failure.");
                return;
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        public void InitResourceUI( )
        {
            if(GameCollectionEntry.UI.HasUIGroup("Overlay"))
            {
                UIGroupHelper group = (UIGroupHelper)GameCollectionEntry.UI.GetUIGroup("Overlay").Helper;
                if(group == null)
                {
                    Log.Error("加载UI界面失败");
                    GameCollectionEntry.ShutdownGameFramework(ShutdownType.Restart);
                    return;
                }
                GameMainInterface = Instantiate(Resources.Load<GameObject>("Builtin/UIPrefabs/LoadingInterface") , group.transform).GetComponent<LoadingInterface>( );
                GameMainInterface.transform.SetLocalPositionAndRotation(Vector3.one , Quaternion.identity);
                GameMainInterface.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// 获取默认字典配置的路径
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private string GetDefalutDictionaryConfigPath(Language language)
        {
            switch(language)
            {
                case Language.English:
                    return "language-english";
                case Language.ChineseSimplified:
                    return "language-chineseSimplified";
                default:
                    return "language-english";
            }
        }
    }
}
