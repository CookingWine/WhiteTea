namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 游戏加载界面
    /// </summary>
    public class LoadingInterface:XMonoBehaviour
    {
        /// <summary>
        /// 网络是否可以连通
        /// </summary>
        public bool IsValidateCompleted
        {
            get;
            set;
        }

        /// <summary>
        /// 视频是否播放完毕
        /// </summary>
        public bool IsPlayVideoOver
        {
            get;
            private set;
        } = true;
    }
}
