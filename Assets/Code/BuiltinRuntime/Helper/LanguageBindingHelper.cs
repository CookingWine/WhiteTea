using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 语言配置绑定器
    /// </summary>
    public class LanguageBindingHelper:MonoBehaviour
    {
        /// <summary>
        /// 当前语言
        /// </summary>
        private Language m_CurrentLanguage;

        [SerializeField]
        private string m_LanguageKey;
        /// <summary>
        /// 语言Key
        /// </summary>
        public string LanguageKey
        {
            get
            {
                return m_LanguageKey;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [SerializeField]
        private string m_Describe;

        private void Start( )
        {
            Log.Debug("初始化事件");
            if(m_CurrentLanguage != GameCollectionEntry.Localization.Language)
            {
                m_CurrentLanguage = GameCollectionEntry.Localization.Language;
            }
        }
    }
}
