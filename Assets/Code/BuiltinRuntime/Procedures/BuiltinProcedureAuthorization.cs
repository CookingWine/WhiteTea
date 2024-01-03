using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 网络验证流程
    /// </summary>
    internal class BuiltinProcedureAuthorization:BuiltinProcedureBase
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
            IsEnterNextProduce = Application.internetReachability != NetworkReachability.NotReachable;
            Log.Debug($"网络验证是否成功:{IsEnterNextProduce}");

        }
        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            //等待网络验证成功
            if(!IsEnterNextProduce)
            {
                //如果网络验证不成功,就请求打开本地界面退出游戏
                return;
            }
            //根据不同模式进入不同的流程
            if(WTGame.Base.EditorResourceMode)
            {
                //编辑器模式进入加载dll

            }
            else if(WTGame.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
            {
                //单机模式进入资源验证
            }
            else
            {
                //更新模式,进入版本请求
            }
        }



    }
}
