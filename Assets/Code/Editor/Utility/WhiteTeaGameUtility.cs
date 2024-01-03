namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 编辑器工具
    /// </summary>
    public static class WhiteTeaGameUtility
    {
        /// <summary>
        /// 添加宏定义
        /// </summary>
        /// <param name="macro">宏</param>
        public static void AddScriptingDefineSymbolsForGroup(string macro)
        {
#if UNITY_2021_1_OR_NEWER
        
#endif

        }

        /// <summary>
        /// 移除宏定义
        /// </summary>
        /// <param name="macro"></param>
        public static void RemoveScriptingDefineSymbolsForGroup(string macro)
        {
#if UNITY_2021_1_OR_NEWER
        
#endif
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
}
