using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 资源更新流程
    /// </summary>
    internal class BuiltinProcedureUpdateResources:BuiltinProcedureBase
    {
        /// <summary>
        /// 更新资源完成
        /// </summary>
        private bool m_UpdateResourceComplete = false;

        /// <summary>
        /// 更新长度
        /// </summary>
        private int m_UpdateCount = 0;

        /// <summary>
        /// 更新压缩总长度
        /// </summary>
        private long m_UpdateTotalCompressedLength = 0L;

        /// <summary>
        /// 更新成功的个数
        /// </summary>
        private int m_UpdateSuccessCount = 0;

        /// <summary>
        /// 更新列表数据
        /// </summary>
        private readonly List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>( );

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
            Log.Debug("进入【更新资源】流程");
            InitValue(procedureOwner);
            GameCollectionEntry.Event.Subscribe(ResourceUpdateStartEventArgs.EventId , OnResourceUpdateStart);
            GameCollectionEntry.Event.Subscribe(ResourceUpdateChangedEventArgs.EventId , OnResourceUpdateChanged);
            GameCollectionEntry.Event.Subscribe(ResourceUpdateSuccessEventArgs.EventId , OnResourceUpdateSuccess);
            GameCollectionEntry.Event.Subscribe(ResourceUpdateFailureEventArgs.EventId , OnResourceUpdateFailure);

            //TODO:根据网络情况打开弹窗更新资源
            StartUpdateResources( );
        }

        private void InitValue(ProcedureOwner procedureOwner)
        {
            m_UpdateResourceComplete = false;
            m_UpdateSuccessCount = 0;
            m_UpdateLengthData.Clear( );
            m_UpdateCount = procedureOwner.GetData<VarInt32>(s_UpdateResourceCount);
            m_UpdateTotalCompressedLength = procedureOwner.GetData<VarInt64>(s_UpdateResourceTotalCompressedLength);
            procedureOwner.RemoveData(s_UpdateResourceCount);
            procedureOwner.RemoveData(s_UpdateResourceTotalCompressedLength);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            GameCollectionEntry.Event.Unsubscribe(ResourceUpdateStartEventArgs.EventId , OnResourceUpdateStart);
            GameCollectionEntry.Event.Unsubscribe(ResourceUpdateChangedEventArgs.EventId , OnResourceUpdateChanged);
            GameCollectionEntry.Event.Unsubscribe(ResourceUpdateSuccessEventArgs.EventId , OnResourceUpdateSuccess);
            GameCollectionEntry.Event.Unsubscribe(ResourceUpdateFailureEventArgs.EventId , OnResourceUpdateFailure);
            base.OnLeave(procedureOwner , isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            RefreshProgress( );
            if(!m_UpdateResourceComplete)
            {
                return;
            }
            ChangeState(procedureOwner , typeof(BuiltinProcedurePreloadDll));
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

            GameCollectionEntry.BuiltinData.GameMainInterface.SetProgressInfo(progressTotal , $"{GetByteLengthString(currentTotalUpdateLength)}/{GetByteLengthString(m_UpdateTotalCompressedLength)}.当前速度{GetByteLengthString((int)GameCollectionEntry.Download.CurrentSpeed)}");
        }

        /// <summary>
        /// 开始更新资源
        /// </summary>
        private void StartUpdateResources( )
        {
            Log.Debug("Start update resource");
            GameCollectionEntry.Resource.UpdateResources(OnUpdateResourceComplete);
        }

        /// <summary>
        /// 使用可更新模式并更新指定资源组完成时的回调函数。
        /// </summary>
        /// <param name="resourceGroup">更新的资源组。</param>
        /// <param name="result">更新资源结果，全部成功为 true，否则为 false。</param>
        private void OnUpdateResourceComplete(GameFramework.Resource.IResourceGroup resourceGroup , bool result)
        {
            if(result)
            {
                m_UpdateResourceComplete = true;
            }
            else
            {
                Log.Error("Update resources complete with errors.");
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
                if(m_UpdateLengthData[i].Name == ne.Name)
                {
                    Log.Warning("Update resource 【{0}】 is invalid." , ne.Name);
                    m_UpdateLengthData[i].Length = 0;
                    return;
                }
            }
            m_UpdateLengthData.Add(new UpdateLengthData(ne.Name));
        }

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateChanged(object sender , GameEventArgs e)
        {
            ResourceUpdateChangedEventArgs ne = (ResourceUpdateChangedEventArgs)e;

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.CurrentLength;
                    return;
                }
            }
            Log.Warning("Update resource 【{0}】 is invalid." , ne.Name);
        }

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceUpdateSuccess(object sender , GameEventArgs e)
        {
            ResourceUpdateSuccessEventArgs ne = (ResourceUpdateSuccessEventArgs)e;
            Log.Info("Update resource '{0}' success." , ne.Name);

            for(int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if(m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData[i].Length = ne.Length;
                    m_UpdateSuccessCount++;
                    return;
                }
            }

            Log.Warning("Update resource 【{0}】 is invalid." , ne.Name);
        }

        /// <summary>
        /// 资源更新失败事件。
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
                if(m_UpdateLengthData[i].Name == ne.Name)
                {
                    m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
                    return;
                }
            }

            Log.Warning("Update resource 【{0}】 is invalid." , ne.Name);
        }

        private string GetByteLengthString(long byteLength)
        {
            if(byteLength < 1024L) // 2 ^ 10
            {
                return Utility.Text.Format("{0} Bytes" , byteLength.ToString( ));
            }

            if(byteLength < 1048576L) // 2 ^ 20
            {
                return Utility.Text.Format("{0} KB" , ( byteLength / 1024f ).ToString("F2"));
            }

            if(byteLength < 1073741824L) // 2 ^ 30
            {
                return Utility.Text.Format("{0} MB" , ( byteLength / 1048576f ).ToString("F2"));
            }

            if(byteLength < 1099511627776L) // 2 ^ 40
            {
                return Utility.Text.Format("{0} GB" , ( byteLength / 1073741824f ).ToString("F2"));
            }

            if(byteLength < 1125899906842624L) // 2 ^ 50
            {
                return Utility.Text.Format("{0} TB" , ( byteLength / 1099511627776f ).ToString("F2"));
            }

            if(byteLength < 1152921504606846976L) // 2 ^ 60
            {
                return Utility.Text.Format("{0} PB" , ( byteLength / 1125899906842624f ).ToString("F2"));
            }

            return Utility.Text.Format("{0} EB" , ( byteLength / 1152921504606846976f ).ToString("F2"));
        }

        private class UpdateLengthData
        {
            /// <summary>
            /// 资源名
            /// </summary>
            public string Name
            {
                get;
                private set;
            }
            /// <summary>
            /// 长度
            /// </summary>
            public int Length
            {
                get;
                set;
            }
            public UpdateLengthData(string name)
            {
                Name = name;
            }
        }
    }
}
