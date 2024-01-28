using UnityEditor;
namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 编辑器工具
    /// </summary>
    internal static class WhiteTeaGameUtility
    {
        /// <summary>
        /// 添加宏定义
        /// </summary>
        /// <param name="macro">宏</param>
        public static void AddScriptingDefineSymbolsForGroup(string macro)
        {
#if UNITY_2021_1_OR_NEWER

#endif
            var target = GetCurrentBuildTarget( );
            string[] defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target).Split(';');
            if(!ArrayUtility.Contains(defines , macro))
            {
                ArrayUtility.Add<string>(ref defines , macro);
            }
            string temp = string.Empty;
            for(int i = 0; i < defines.Length; i++)
            {
                temp = defines[i] + ";";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target , temp);
        }

        /// <summary>
        /// 移除宏定义
        /// </summary>
        /// <param name="macro"></param>
        public static void RemoveScriptingDefineSymbolsForGroup(string macro)
        {
#if UNITY_2021_1_OR_NEWER
        
#endif
            var target = GetCurrentBuildTarget( );
            string[] defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target).Split(';');
            if(ArrayUtility.Contains(defines , macro))
            {
                ArrayUtility.Remove<string>(ref defines , macro);
            }
            string temp = string.Empty;
            for(int i = 0; i < defines.Length; i++)
            {
                temp = defines[i] + ";";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target , temp);

        }

        public static void SetAppCompanyAndProductName(string name)
        {
            PlayerSettings.companyName = name;
            PlayerSettings.productName = name;
        }

        /// <summary>
        /// 获取当前构建的平台
        /// </summary>
        /// <returns></returns>
        public static UnityEditor.BuildTargetGroup GetCurrentBuildTarget( )
        {
#if UNITY_ANDROID
            return UnityEditor.BuildTargetGroup.Android;
#elif UNITY_IOS
            return UnityEditor.BuildTargetGroup.iOS;
#elif UNITY_STANDALONE
            return UnityEditor.BuildTargetGroup.Standalone;
#elif UNITY_WEBGL
            return UnityEditor.BuildTargetGroup.WebGL;
#else
            return UnityEditor.BuildTargetGroup.Unknown;
#endif
        }
#if UNITY_2021_1_OR_NEWER
        public static UnityEditor.Build.NamedBuildTarget GetCurrentNamedBuildTarget()
        {
#if UNITY_ANDROID
            return UnityEditor.Build.NamedBuildTarget.Android;
#elif UNITY_IOS
            return UnityEditor.Build.NamedBuildTarget.iOS;
#elif UNITY_STANDALONE
            return UnityEditor.Build.NamedBuildTarget.Standalone;
#elif UNITY_WEBGL
            return UnityEditor.Build.NamedBuildTarget.WebGL;
#else
            return UnityEditor.Build.NamedBuildTarget.Unknown;
#endif
        }
#endif
    }

    /// <summary>
    /// 选择资源
    /// </summary>
    internal class SelectAssetsData
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable;

        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetsName { get; private set; }

        public SelectAssetsData(string assetsName , bool isEnable)
        {
            AssetsName = assetsName;
            IsEnable = isEnable;
        }
    }

}
