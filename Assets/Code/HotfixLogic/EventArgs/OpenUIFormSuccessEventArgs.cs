using UnityGameFramework.Runtime;

namespace WhiteTea.HotfixLogic
{
    public sealed class OpenUIFormSuccessEventArgs:HotfixGameEventArgs
    {
        /// <summary>
        /// 打开界面成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(OpenUIFormSuccessEventArgs).GetHashCode( );

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public override void Clear( )
        {
            UIForm = null;
            Duratio = 0;
            UserData = null;
        }

        public UIFormLogic UIForm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载界面的持续时间
        /// </summary>
        public float Duratio
        {
            get;
            private set;
        }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
