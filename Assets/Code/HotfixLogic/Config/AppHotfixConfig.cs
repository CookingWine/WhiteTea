namespace UGHGame.HotfixLogic
{
    public class AppHotfixConfig
    {
        /// <summary>
        /// Aot文件列表
        /// </summary>
        public static string[] AotFileList { get; } = new string[]
        {
            "GameFramework",
            "UnityGameFramework.Runtime"
        };
        /// <summary>
        /// 热更流程
        /// </summary>
        public static string[] Procedures { get; } = new string[]
        {
            "ProcedureHotfixEntry"
        };
    }
}
