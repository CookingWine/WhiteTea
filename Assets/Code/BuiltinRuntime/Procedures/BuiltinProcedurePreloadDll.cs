using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 加载热更dll流程
    /// </summary>
    internal class BuiltinProcedurePreloadDll:BuiltinProcedureBase
    {
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
            ReadyEnterHotfixEntry( );
        }
        /// <summary>
        /// 准备进入热更入口
        /// </summary>
        private void ReadyEnterHotfixEntry( )
        {
            WTGame.Hybridclr.HotfixEntry(( ) =>
            {
                WTGame.Fsm.DestroyFsm<IProcedureManager>( );
            });
        }
    }
}
