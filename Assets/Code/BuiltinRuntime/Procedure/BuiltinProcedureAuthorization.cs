using UnityEngine;
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
            Log.Info("Enter the network verification process.");
            //TODO:进行网络通信验证
            GameCollectionEntry.BuiltinData.GameMainInterface.IsValidateCompleted = Application.internetReachability != NetworkReachability.NotReachable;
            IsEnterNextProduce = true;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            if(!GameCollectionEntry.BuiltinData.GameMainInterface.IsPlayVideoOver)
            {
                //等待视频播放完成
                return;
            }
            //根据不同模式进入不同流程
            if(GameCollectionEntry.Base.EditorResourceMode)
            {
                //编辑器模式直接进入加载dll
                ChangeState(procedureOwner , typeof(BuiltinProcedurePreloadDll));
            }
            else if(GameCollectionEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
            {
                //TODO:单机模式
                ChangeState(procedureOwner , typeof(BuiltinProcedureInitResources));
            }
            else
            {
                //TODO:更新模式
            }

        }
    }
}
