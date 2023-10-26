using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 初始化资源流程【单机模式下】
    /// </summary>
    internal class BuiltinProcedureInitResources:BuiltinProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("Enter standalone mode and begin initializing resources.");
            GameCollectionEntry.Resource.InitResources(OnInitResourceComplete);
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!IsEnterNextProduce)
            {
                //等待资源初始化完成
                return;
            }
            ChangeState(procedureOwner , typeof(BuiltinProcedurePreloadDll));
        }

        /// <summary>
        /// 初始化资源完成回调
        /// </summary>
        private void OnInitResourceComplete( )
        {
            IsEnterNextProduce = true;
        }
    }
}
