using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{

    /// <summary>
    /// 加载dll流程【加载dll流程永远都是应用初始化流程中的最后一个流程】
    /// </summary>
    internal class BuiltinProcedurePreloadDll:BuiltinProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Warning("Enter the hot patch process.");
            ReadyEnterHotfixEntry( );
        }

        /// <summary>
        /// 准备进入热更入口
        /// </summary>
        private void ReadyEnterHotfixEntry( )
        {
            GameCollectionEntry.Hybridclr.HotfixEntry(( ) =>
            {
                GameCollectionEntry.Fsm.DestroyFsm<IProcedureManager>( );
            });
            IsEnterNextProduce = true;
        }

    }
}
