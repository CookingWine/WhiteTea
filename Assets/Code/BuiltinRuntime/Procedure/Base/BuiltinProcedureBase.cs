using GameFramework.Procedure;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 内置运行时流程基类
    /// </summary>
    internal abstract class BuiltinProcedureBase:ProcedureBase
    {
        /// <summary>
        /// 是否进入下一个流程
        /// </summary>
        protected bool IsEnterNextProduce = false;
    }
}
