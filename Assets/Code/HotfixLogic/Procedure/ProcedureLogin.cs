using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 进入登录流程
    /// </summary>
    public class ProcedureLogin:ProcedureBase
    {
        private int m_LoginInterfaceId = 0;
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_LoginInterfaceId = (int)WTGame.UI.OpenUIForm(UIFormId.HotfixGameLoginInterface);
        }
        protected internal override void OnUpdate(IFsm procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
        }

        protected internal override void OnLeave(IFsm procedureOwner , bool isShutdown)
        {
            if(WTGame.UI.HasUIForm(m_LoginInterfaceId))
            {
                WTGame.UI.CloseUIForm(m_LoginInterfaceId);
            }
            base.OnLeave(procedureOwner , isShutdown);
        }
    }
}
