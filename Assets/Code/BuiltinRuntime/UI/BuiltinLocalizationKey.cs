
using UnityEngine;
namespace WhiteTea.BuiltinRuntime
{
    public class BuiltinLocalizationKey:MonoBehaviour
    {
        [Header("语言key")]
        [SerializeField]
        private string m_LocalizationKey;

        public string LocalizationKey
        {
            get
            {
                return m_LocalizationKey;
            }
        }

        public string Values;

        public void SetLocalizationKey(string key)
        {
            m_LocalizationKey = key;
        }
    }
}
