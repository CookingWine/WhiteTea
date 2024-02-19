using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 验证SDK
    /// </summary>
    public class ProcedureVerificationSDK:ProcedureBase
    {
        private float m_current;
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_current = 0;
            Log.Debug("<color=line>开始加载SDK</color>");
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            m_current += elapseSeconds;
            WTGame.BuiltinData.GameMainInterface.SetUpdateSchedule("加载SDK" , m_current / 5.0f);
            if(m_current > 5.0f)
            {
                //procedureOwner.SetData<VarInt32>(HotfixConstantUtility.NextSceneID , (int)ScenesId.HotfixEntryScenes);
                ChangeState<ProcedureLogin>(procedureOwner);
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            base.OnLeave(procedureOwner , isShutdown);
        }
    }
}
