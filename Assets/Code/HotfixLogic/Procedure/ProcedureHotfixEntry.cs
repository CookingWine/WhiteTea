using UnityGameFramework.Runtime;
using ProcedureOwner = UGHGame.HotfixLogic.IFsm;
namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 热更流程的入口
    /// </summary>
    public class ProcedureHotfixEntry:ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入热更流程的入口了");
        }

    }
}
