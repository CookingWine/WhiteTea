using UnityGameFramework.Runtime;
using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 用户主界面选择流程
    /// </summary>
    public class ProrcedureUserSelection:ProcedureBase
    {
        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug("进入主界面流程");
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            base.OnLeave(procedureOwner , isShutdown);
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
        }
    }
}
