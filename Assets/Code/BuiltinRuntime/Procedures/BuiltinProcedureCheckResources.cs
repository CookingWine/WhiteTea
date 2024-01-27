using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 检查资源流程
    /// </summary>
    internal class BuiltinProcedureCheckResources:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 检查资源完成
        /// </summary>
        private bool m_CheckReourcesComplete = false;
        /// <summary>
        /// 是否需要更新资源
        /// </summary>
        private bool m_NeedUpdateResources = false;
        /// <summary>
        /// 更新资源长度
        /// </summary>
        private int m_UpdateResourceCount = 0;

        /// <summary>
        /// 更新资源总压缩长度
        /// </summary>
        private long m_UpdateResourceTotalCompressedLength = 0L;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("<color=lime>进入【检查资源】流程</color>");
            m_CheckReourcesComplete = false;
            m_NeedUpdateResources = false;
            m_UpdateResourceCount = 0;
            m_UpdateResourceTotalCompressedLength = 0L;
            WTGame.Resource.CheckResources(OnCheckResourcesComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            if(!m_CheckReourcesComplete)
            {
                return;
            }
            if(m_NeedUpdateResources)
            {
                procedureOwner.SetData<VarInt32>("UpdateResourceCount" , m_UpdateResourceCount);
                procedureOwner.SetData<VarInt64>("UpdateResourceTotalCompressedLength" , m_UpdateResourceTotalCompressedLength);
                ChangeState<BuiltinProcedureUpdateResource>(procedureOwner);
            }
            else
            {
                ChangeState<BuiltinProcedurePreloadDll>(procedureOwner);
            }
        }


        /// <summary>
        /// 使用可更新模式并检查资源完成时的回调函数。
        /// </summary>
        /// <param name="movedCount">已移动的资源数量。</param>
        /// <param name="removedCount">已移除的资源数量。</param>
        /// <param name="updateCount">可更新的资源数量。</param>
        /// <param name="updateTotalLength">可更新的资源总大小。</param>
        /// <param name="updateTotalCompressedLength">可更新的压缩后总大小。</param>
        private void OnCheckResourcesComplete(int movedCount , int removedCount , int updateCount , long updateTotalLength , long updateTotalCompressedLength)
        {
            IResourceGroupCollection resourceGroupCollection=WTGame.Resource.GetResourceGroupCollection(WTGame.AppBuiltinConfigs.MustResourceGroup);
            if(resourceGroupCollection == null)
            {
                Log.Error("{0}资源组不存在" , WTGame.AppBuiltinConfigs.MustResourceGroup);
                return;
            }

            m_CheckReourcesComplete = true;
            m_NeedUpdateResources = !resourceGroupCollection.Ready;
            m_UpdateResourceCount = resourceGroupCollection.TotalCount - resourceGroupCollection.ReadyCount;
            m_UpdateResourceTotalCompressedLength = updateTotalCompressedLength;
        }
    }
}
