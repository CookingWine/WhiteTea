namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 构建信息
    /// </summary>
    public class BuildInfo
    {
        /// <summary>
        /// 游戏版本
        /// </summary>
        public string GameVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 测试游戏版本
        /// </summary>
        public int InternalGameVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 检查版本URL
        /// </summary>
        public string CheckVersionUrl
        {
            get;
            set;
        }

        /// <summary>
        /// windows平台更新URL
        /// </summary>
        public string WindowsAppUrl
        {
            get;
            set;
        }

        /// <summary>
        /// MacOs平台更新URL
        /// </summary>
        public string MacOSAppUrl
        {
            get;
            set;
        }

        /// <summary>
        /// IOS平台更新URL
        /// </summary>
        public string IOSAppUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Android平台更新URL
        /// </summary>
        public string AndroidAppUrl
        {
            get;
            set;
        }
    }
}
