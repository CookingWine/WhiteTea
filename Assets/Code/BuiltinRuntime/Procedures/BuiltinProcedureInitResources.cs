using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 单机模式下，初始化资源流程
    /// </summary>
    internal class BuiltinProcedureInitResources:BuiltinProcedureBase
    {
        /// <summary>
        /// 初始化资源是否完成
        /// </summary>
        private bool m_InitResourceComplete = false;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_InitResourceComplete = false;
            WTGame.Resource.InitResources(OnInitResourceComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_InitResourceComplete)
            {
                return;
            }
            ChangeState<BuiltinProcedurePreloadDll>(procedureOwner);
        }
        /// <summary>
        /// 初始化资源完成回调
        /// </summary>
        private void OnInitResourceComplete( )
        {
            m_InitResourceComplete = true;
            Log.Info("单机模式初始化资源完成");
        }
    }
}
