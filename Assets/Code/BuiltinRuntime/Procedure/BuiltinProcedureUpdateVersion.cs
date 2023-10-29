using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 更新版本流程
    /// </summary>
    internal class BuiltinProcedureUpdateVersion:BuiltinProcedureBase
    {
        /// <summary>
        /// 更新版本完毕
        /// </summary>
        private bool m_UpdateVersionComplete = false;

        /// <summary>
        /// 初始化版本资源列表更新回调函数集的新实例。
        /// </summary>
        private UpdateVersionListCallbacks m_UpdateVersionListCallbacks = null;

        /// <summary>
        /// 版本列表的长度
        /// </summary>
        private const string s_VersionListLength = "VersionListLength";
        
        /// <summary>
        /// 版本列表的哈希值
        /// </summary>
        private const string s_VersionListHashCode = "VersionListHashCode";

        /// <summary>
        /// 版本列表压缩长度
        /// </summary>
        private const string s_VersionListCompressedLength = "VersionListCompressedLength";

        /// <summary>
        /// 版本列表压缩哈希值
        /// </summary>
        private const string s_VersionListCompressedHashCode = "VersionListCompressedHashCode";

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_UpdateVersionListCallbacks = new UpdateVersionListCallbacks(OnUpdateVersionListSuccess, OnUpdateVersionListFailure);
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入【更新版本】流程");
            GameCollectionEntry.Resource.UpdateVersionList(procedureOwner.GetData<VarInt32>(s_VersionListLength) , procedureOwner.GetData<VarInt32>(s_VersionListHashCode) , procedureOwner.GetData<VarInt32>(s_VersionListCompressedLength) , procedureOwner.GetData<VarInt32>(s_VersionListCompressedHashCode) , m_UpdateVersionListCallbacks);
            procedureOwner.RemoveData(s_VersionListLength);
            procedureOwner.RemoveData(s_VersionListHashCode);
            procedureOwner.RemoveData(s_VersionListCompressedLength);
            procedureOwner.RemoveData(s_VersionListCompressedHashCode);

        }
        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            if(!m_UpdateVersionComplete)
            {
                return;
            }
            ChangeState(procedureOwner , typeof(BuiltinProcedureCheckResources));
        }

        /// <summary>
        /// 版本资源列表更新成功回调函数。
        /// </summary>
        /// <param name="downloadPath">版本资源列表更新后存放路径。</param>
        /// <param name="downloadUri">版本资源列表更新地址</param>
        private void OnUpdateVersionListSuccess(string downloadPath , string downloadUri)
        {
            m_UpdateVersionComplete = true;
        }

        /// <summary>
        /// 版本资源列表更新失败回调函数。
        /// </summary>
        /// <param name="downloadUri">版本资源列表更新地址</param>
        /// <param name="errorMessage">错误信息</param>
        private void OnUpdateVersionListFailure(string downloadUri , string errorMessage)
        {
            Log.Error("Update version list from '{0}' failure, error message is '{1}'." , downloadUri , errorMessage);
        }

    }
}
