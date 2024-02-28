using WhiteTea.BuiltinRuntime;
using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 进入登录流程
    /// </summary>
    public class ProcedureLogin:ProcedureBase
    {
        private int m_LoginInterfaceId = 0;

        private bool m_IsEnterUserSelectionProcedure;
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_IsEnterUserSelectionProcedure = false;
            m_LoginInterfaceId = (int)WTGame.UI.OpenUIForm(UIFormId.HotfixGameLoginInterface , this);
            //TODO:验证本地账户是否登录
        }
        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_IsEnterUserSelectionProcedure)
            {
                return;
            }
            ChangeState<ProrcedureUserSelection>(procedureOwner);
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            if(WTGame.UI.HasUIForm(m_LoginInterfaceId))
            {
                WTGame.UI.CloseUIForm(m_LoginInterfaceId);
            }
            base.OnLeave(procedureOwner , isShutdown);
        }
        public void NextProcedure( )
        {
            m_IsEnterUserSelectionProcedure = true;
        }
    }
}
