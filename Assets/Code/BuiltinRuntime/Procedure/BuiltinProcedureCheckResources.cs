using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 检查资源流程
    /// </summary>
    internal class BuiltinProcedureCheckResources:BuiltinProcedureBase
    {
        /// <summary>
        /// 检查资源完成
        /// </summary>
        private bool m_CheckResourcesComplete = false;

        /// <summary>
        /// 是否需要更新资源
        /// </summary>
        private bool m_NeedUpdateResource = false;

        /// <summary>
        /// 更新资源长度
        /// </summary>
        private int m_UpdateResourceCount = 0;

        /// <summary>
        /// 更新资源的总长度
        /// </summary>
        private long m_UpdateResourceTotalCompressdLength = 0L;

        /// <summary>
        /// 更新资源长度
        /// </summary>
        private const string s_UpdateResourceCount = "UpdateResourceCount";

        /// <summary>
        /// 更新资源的总长度
        /// </summary>
        private const string s_UpdateResourceTotalCompressedLength = "UpdateResourceTotalCompressedLength";
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入【检查资源】流程");
            InitValue( );
            GameCollectionEntry.Resource.CheckResources(OnCheckResourcesComplete);
        }
        private void InitValue( )
        {
            m_CheckResourcesComplete = false;
            m_NeedUpdateResource = false;
            m_UpdateResourceCount = 0;
            m_UpdateResourceTotalCompressdLength = 0L;
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_CheckResourcesComplete)
            {
                return;
            }

            if(m_NeedUpdateResource)
            {
                procedureOwner.SetData<VarInt32>(s_UpdateResourceCount, m_UpdateResourceCount);
                procedureOwner.SetData<VarInt64>(s_UpdateResourceTotalCompressedLength , m_UpdateResourceTotalCompressdLength);
                ChangeState(procedureOwner , typeof(BuiltinProcedureUpdateResources));
            }
            else
            {
                //需要更新资源的话，进入dll流程
                ChangeState(procedureOwner , typeof(BuiltinProcedurePreloadDll));
            }
        }

        /// <summary>
        ///  使用可更新模式并检查资源完成时的回调函数。
        /// </summary>
        /// <param name="movedCount">已移动的资源数量</param>
        /// <param name="removedCount">已移除的资源数量</param>
        /// <param name="updateCount">可更新的资源数量</param>
        /// <param name="updateTotalLength">可更新的资源总大小</param>
        /// <param name="updateTotalCompressedLength">可更新的压缩后总大小</param>
        private void OnCheckResourcesComplete(int movedCount , int removedCount , int updateCount , long updateTotalLength , long updateTotalCompressedLength)
        {
            m_CheckResourcesComplete = true;
            m_NeedUpdateResource = updateCount > 0;
            m_UpdateResourceCount = updateCount;
            m_UpdateResourceTotalCompressdLength = updateTotalLength;
        }
    }
}
