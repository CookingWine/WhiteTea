using GameFramework.Procedure;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 流程基类
    /// </summary>
    internal abstract class BuiltinProcedureBase:ProcedureBase
    {
        /// <summary>
        /// 是否使用本地对话框
        /// <para>在一些特殊的流程（如游戏逻辑对话框资源更新完成前的流程）中，可以考虑调用原生对话框进行消息提示行为</para>
        /// </summary>
        public abstract bool UseNativeDialog { get; }

        /// <summary>
        /// 是否进入下一个流程
        /// </summary>
        protected bool IsEnterNextProduce = false;
    }
}
