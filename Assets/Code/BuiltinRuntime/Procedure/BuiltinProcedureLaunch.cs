using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 内置运行时流程入口
    /// </summary>
    internal class BuiltinProcedureLaunch:BuiltinProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("The framework is successfully started and the default application configuration is loaded.");
            //TODO:初始化构建配置

            //TODO:初始化默认语言配置

            //TODO:初始化变体配置

            //TODO:初始化字典配置

            //TODO:初始化加载界面配置

            IsEnterNextProduce = true;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            if(!IsEnterNextProduce)
            {
                return;
            }
            ChangeState(procedureOwner , typeof(BuiltinProcedureAuthorization));
        }
    }
}
