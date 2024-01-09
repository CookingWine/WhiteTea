using UnityEngine;
using UnityGameFramework.Runtime;
namespace WhiteTea.BuiltinRuntime
{
    internal class AppBuiltinSettings:ScriptableObject
    {
        private static AppBuiltinSettings m_Instance;
        public static AppBuiltinSettings Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = Resources.Load<AppBuiltinSettings>(BuiltinRuntimeUtility.AppBuiltinSettingsName);
                }
                return m_Instance;
            }
        }
        /// <summary>
        /// 禁止其他类去new
        /// </summary>
        private AppBuiltinSettings( )
        {

        }

        [Header("必须更新的资源组")]
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

        /// <summary>
        /// 热更程序集
        /// </summary>
        public string HotfixAssembliy
        {
            get
            {
                return "WhiteTea.HotfixLogic";
            }
        }
        /// <summary>
        /// 热更入口类
        /// </summary>
        public string HotfixEntryClass
        {
            get
            {
                return "WhiteTea.HotfixLogic.HotfixEntry";
            }
        }
        /// <summary>
        /// 热更start方法
        /// </summary>
        public string HotfixStartFuntion
        {
            get
            {
                return "Start";
            }
        }
        /// <summary>
        /// 热更update方法
        /// </summary>
        public string HotfixUpdate
        {
            get
            {
                return "Update";
            }
        }
        /// <summary>
        /// 热更Shutdown方法
        /// </summary>
        public string HotfixShutdown
        {
            get
            {
                return "Shutdown";
            }
        }

        /// <summary>
        /// 加载语言配置文件
        /// </summary>
        public void InitLoadLanguageConfigData( )
        {
            var languageTemp = WTGame.Base.EditorLanguage;
            if(languageTemp == GameFramework.Localization.Language.Unspecified)
            {
                languageTemp = GameFramework.Localization.Language.ChineseSimplified;
            }
            TextAsset language = Resources.Load<TextAsset>(BuiltinRuntimeUtility.AssetsUtility.GetLanguageAssets(languageTemp.ToString( ) , false));
            if(language == null)
            {
                Log.Error("Reseources加载语言文件失败");
                return;
            }
            if(!WTGame.Localization.ParseData(language.text))
            {
                Log.Error("解析语言配置文件失败");
                return;
            }
        }
    }
}
