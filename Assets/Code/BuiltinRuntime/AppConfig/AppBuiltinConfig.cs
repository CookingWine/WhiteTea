using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 应用运行时配置
    /// </summary>
    [CreateAssetMenu(fileName = "AppBuiltinConfig" , menuName = "AppConfig/AppBuiltinConfig" , order = 0)]
    public class AppBuiltinConfig:ScriptableObject
    {
        /// <summary>
        /// 检查版本URL
        /// </summary>
        public string CheckVersionURL;


    }
}
