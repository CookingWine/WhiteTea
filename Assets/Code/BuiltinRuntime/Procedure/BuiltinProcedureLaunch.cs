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
            Log.Info("Launch the framework and start loading the application's default configuration.");

            //TODO:初始化构建配置
            GameCollectionEntry.BuiltinData.InitBuildInfo( );
            //TODO:初始化默认语言配置

            //TODO:初始化变体配置

            //TODO:初始化字典配置

            //TODO:初始化加载界面配置
            GameCollectionEntry.BuiltinData.InitResourceUI( );
            //到这里应用基础配置加载完毕:300毫秒
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
