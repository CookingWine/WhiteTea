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
