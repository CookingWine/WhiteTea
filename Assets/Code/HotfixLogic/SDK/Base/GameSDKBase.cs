using System;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 加载SDK基类
    /// </summary>
    public abstract class GameSDKBase
    {
        /// <summary>
        /// 是否完成初始化
        /// </summary>
        public abstract bool Initialized { get; }

        /// <summary>
        /// 初始化SDK
        /// </summary>
        public abstract void InitializedSDK(Action action);
    }
}
