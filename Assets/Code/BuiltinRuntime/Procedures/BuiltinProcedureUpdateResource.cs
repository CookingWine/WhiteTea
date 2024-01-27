using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 更新资源流程
    /// </summary>
    internal class BuiltinProcedureUpdateResource:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 更新资源是否完成
        /// </summary>
        private bool m_UpdateResourcesComplete = false;
        /// <summary>
        /// 更新长度
        /// </summary>
        private int m_UpdateCount = 0;
        /// <summary>
        /// 更新资源总压缩长度
        /// </summary>
        private long m_UpdateTotalCompressedLength = 0L;
        /// <summary>
        /// 更新成功的个数
        /// </summary>
        private int m_UpdateSuccessCount = 0;
        private readonly List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>( );
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("<color=lime>进入【更新资源】流程</color>");
            m_UpdateResourcesComplete = false;
            m_UpdateCount = procedureOwner.GetData<VarInt32>("UpdateResourceCount");
            procedureOwner.RemoveData("UpdateResourceCount");
            m_UpdateTotalCompressedLength = procedureOwner.GetData<VarInt64>("UpdateResourceTotalCompressedLength");
            procedureOwner.RemoveData("UpdateResourceTotalCompressedLength");
            m_UpdateSuccessCount = 0;
            m_UpdateLengthData.Clear( );

            WTGame.Event.Subscribe(ResourceUpdateStartEventArgs.EventId , OnResourceUpdateStart);
            WTGame.Event.Subscribe(ResourceUpdateChangedEventArgs.EventId , OnResourceUpdateChanged);
            WTGame.Event.Subscribe(ResourceUpdateSuccessEventArgs.EventId , OnResourceUpdateSuccess);
            WTGame.Event.Subscribe(ResourceUpdateFailureEventArgs.EventId , OnResourceUpdateFailure);

            //移动网络的话提示弹窗是否需要更新
            if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                //TODO:打开弹窗
                return;
            }
            StartUpdateResources(WTGame.AppBuiltinConfigs.MustResourceGroup);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            WTGame.Event.Unsubscribe(ResourceUpdateStartEventArgs.EventId , OnResourceUpdateStart);
            WTGame.Event.Unsubscribe(ResourceUpdateChangedEventArgs.EventId , OnResourceUpdateChanged);
            WTGame.Event.Unsubscribe(ResourceUpdateSuccessEventArgs.EventId , OnResourceUpdateSuccess);
            WTGame.Event.Unsubscribe(ResourceUpdateFailureEventArgs.EventId , OnResourceUpdateFailure);
            base.OnLeave(procedureOwner , isShutdown);
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            RefreshProgress( );
            if(!m_UpdateResourcesComplete)
            {
                return;
            }
            ChangeState<BuiltinProcedurePreloadDll>(procedureOwner);
        }
        /// <summary>
        /// 开始更新资源
        /// </summary>
        /// <param name="userData"></param>
        private void StartUpdateResources(params string[] userData)
        {
            for(int i = 0; i < userData.Length; i++)
            {
                WTGame.Resource.UpdateResources(userData[i] , OnUpdateResourcesComplete);
            }
        }
        /// <summary>
        /// 刷新更新进度
        /// </summary>
        private void RefreshProgress( )
        {
            long currentTotalUpdateLength = 0L;
            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                currentTotalUpdateLength += m_UpdateLengthData[i].Length;
            }
            float progressTotal = (float)currentTotalUpdateLength / m_UpdateTotalCompressedLength;
            string currentUpdateLength = BuiltinRuntimeUtility.ValuerUtility.GetByteLengthString(currentTotalUpdateLength);
            string totalUpdateLength = BuiltinRuntimeUtility.ValuerUtility.GetByteLengthString(m_UpdateTotalCompressedLength);
            string currentSpeed = BuiltinRuntimeUtility.ValuerUtility.GetByteLengthString((int)WTGame.Download.CurrentSpeed);
            Log.Debug($"当前更新成功个数{m_UpdateSuccessCount}更新总个数{m_UpdateCount}进度{progressTotal}已更新大小{currentUpdateLength}总大小{totalUpdateLength}当前下载速度{currentSpeed}");
        }
        /// <summary>
        /// 使用可更新模式并更新指定资源组完成时的回调函数。
        /// </summary>
        /// <param name="resourceGroup">更新的资源组。</param>
        /// <param name="result">更新资源结果，全部成功为 true，否则为 false。</param>
        private void OnUpdateResourcesComplete(GameFramework.Resource.IResourceGroup resourceGroup , bool result)
        {
            if(result)
            {
                for(int i = 0; i < WTGame.AppBuiltinConfigs.MustResourceGroup.Length; i++)
                {
                    if(!WTGame.Resource.HasResourceGroup(WTGame.AppBuiltinConfigs.MustResourceGroup[i]))
                    {
                        return;
                    }
                }
                m_UpdateResourcesComplete = true;

            }
            else
            {
                Log.Error("更新资源失败");
            }
        }
        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateStart(object sender , GameEventArgs e)
        {
            ResourceUpdateStartEventArgs ne = (ResourceUpdateStartEventArgs)e;

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Names == ne.Name)
                {
                    Log.Warning("Update resource '{0}' is invalid." , ne.Name);
                    m_UpdateLengthData[i].Length = 0;
                    return;
                }
            }

            m_UpdateLengthData.Add(new UpdateLengthData(ne.Name));
        }
        /// <summary>
        /// 资源更新改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateChanged(object sender , GameEventArgs e)
        {
            ResourceUpdateChangedEventArgs ne = (ResourceUpdateChangedEventArgs)e;

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Names == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.CurrentLength;
                    //RefreshProgress();
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid." , ne.Name);
        }
        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateSuccess(object sender , GameEventArgs e)
        {
            ResourceUpdateSuccessEventArgs ne = (ResourceUpdateSuccessEventArgs)e;
            Log.Info("Update resource '{0}' success." , ne.Name);

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Names == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.Length;
                    m_UpdateSuccessCount++;
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid." , ne.Name);
        }
        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateFailure(object sender , GameEventArgs e)
        {
            ResourceUpdateFailureEventArgs ne = (ResourceUpdateFailureEventArgs)e;
            if(ne.RetryCount >= ne.TotalRetryCount)
            {
                Log.Error("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'." , ne.Name , ne.DownloadUri , ne.ErrorMessage , ne.RetryCount.ToString( ));
                return;
            }
            else
            {
                Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'." , ne.Name , ne.DownloadUri , ne.ErrorMessage , ne.RetryCount.ToString( ));
            }

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Names == ne.Name)
                {
                    m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
                    return;
                }
            }

            Log.Warning("Update resource '{0}' is invalid." , ne.Name);
        }
        private class UpdateLengthData
        {
            public string Names
            {
                get; private set;
            }
            public int Length
            {
                get; set;
            }
            public UpdateLengthData(string names)
            {
                Names = names;
            }
        }
    }
}
