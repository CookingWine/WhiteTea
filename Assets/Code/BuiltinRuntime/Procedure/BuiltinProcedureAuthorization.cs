using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 网络验证流程
    /// </summary>
    internal class BuiltinProcedureAuthorization:BuiltinProcedureBase
    {

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Network communication status is verified.");
            //TODO:进行网络通信验证


            IsEnterNextProduce = true;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!IsEnterNextProduce)
            {
                return;
            }

            //TODO:等待网络通信验证通过

            //TODO:等待视频播放完成

            //TODO:根据不同模式进入不同流程
        }
    }
}
