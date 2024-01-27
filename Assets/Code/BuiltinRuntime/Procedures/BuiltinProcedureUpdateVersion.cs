using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 更新版本流程
    /// </summary>
    internal class BuiltinProcedureUpdateVersion:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// /更新版本是否完成
        /// </summary>
        private bool m_UpdateVersionComplete = false;

        /// <summary>
        /// 版本资源列表更新回调函数集
        /// </summary>
        private UpdateVersionListCallbacks m_UpdateVersionListCallbacks = null;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_UpdateVersionListCallbacks = new UpdateVersionListCallbacks(OnUpdateVersionListSuccess , OnUpdateVersionListFailure);

        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("<color=lime>进入【更新版本】流程</color>");
            m_UpdateVersionComplete = false;
            WTGame.Resource.UpdateVersionList(procedureOwner.GetData<VarInt32>("VersionListLength") , procedureOwner.GetData<VarInt32>("VersionListHashCode") , procedureOwner.GetData<VarInt32>("VersionListCompressedLength") , procedureOwner.GetData<VarInt32>("VersionListCompressedHashCode") , m_UpdateVersionListCallbacks);
            procedureOwner.RemoveData("VersionListLength");
            procedureOwner.RemoveData("VersionListHashCode");
            procedureOwner.RemoveData("VersionListCompressedLength");
            procedureOwner.RemoveData("VersionListCompressedHashCode");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);


            if(!m_UpdateVersionComplete)
            {
                return;
            }
            ChangeState<BuiltinProcedureCheckResources>(procedureOwner);
        }


        private void OnUpdateVersionListSuccess(string downloadPath , string downloadUrl)
        {
            m_UpdateVersionComplete = true;
            Log.Info("Update version list from '{0}' success.Assets path from '{1}'." , downloadUrl , downloadPath);
        }
        private void OnUpdateVersionListFailure(string downloadUrl , string errorMessage)
        {
            Log.Error("Update version list from '{0}' failure, error message is '{1}'." , downloadUrl , errorMessage);
        }
    }
}
