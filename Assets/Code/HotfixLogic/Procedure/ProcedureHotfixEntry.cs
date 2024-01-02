using GameFramework.Event;
using System.Collections.Generic;
using UGHGame.BuiltinRuntime;
using UnityGameFramework.Runtime;
using ProcedureOwner = UGHGame.HotfixLogic.IFsm;
namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 热更流程的入口
    /// </summary>
    public class ProcedureHotfixEntry:ProcedureBase
    {
        /// <summary>
        /// 加载标识
        /// </summary>
        private readonly Dictionary<string , bool> m_LoadedFlag = new Dictionary<string , bool>( );

        /// <summary>
        /// 加载成功个数
        /// </summary>
        private int m_LoadSuccess;
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //注册监听事件
            GameCollectionEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId , OnLoadConfigSuccess);
            GameCollectionEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId , OnLoadConfigFailure);
            GameCollectionEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId , OnLoadDataTableSuccess);
            GameCollectionEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId , OnLoadDataTableFailure);
            GameCollectionEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId , OnLoadDictionarySuccess);
            GameCollectionEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId , OnLoadDictionaryFailure);
            Log.Debug("进入热更流程的入口了");
            m_LoadedFlag.Clear( );
            m_LoadSuccess = 0;
            StartLoadResources( );
            GameCollectionEntry.BuiltinData.GameMainInterface.SetProgressInfo(m_LoadSuccess , m_LoadedFlag.Count + 10 , "加载配置");
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            GetLoadSuccessCount( );
        }
        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            //取消监听事件
            GameCollectionEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId , OnLoadConfigSuccess);
            GameCollectionEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId , OnLoadConfigFailure);
            GameCollectionEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId , OnLoadDataTableSuccess);
            GameCollectionEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId , OnLoadDataTableFailure);
            GameCollectionEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId , OnLoadDictionarySuccess);
            GameCollectionEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId , OnLoadDictionaryFailure);
            base.OnLeave(procedureOwner , isShutdown);
        }


        /// <summary>
        /// 加载资源
        /// </summary>
        private void StartLoadResources( )
        {
            Log.Debug("开始加载数据表");
            foreach(string data in HotfixEntry.AppRuntimeConfig.DataTables)
            {
                LoadHotfixDataTable(data);
            }
        }

        /// <summary>
        /// 获取加载成功个数
        /// </summary>
        /// <returns></returns>
        private int GetLoadSuccessCount( )
        {
            m_LoadSuccess = 0;
            foreach(KeyValuePair<string , bool> item in m_LoadedFlag)
            {
                if(item.Value)
                {
                    m_LoadSuccess++;
                }
            }
            return m_LoadSuccess;
        }
        /// <summary>
        /// 加载热更数据表
        /// </summary>
        /// <param name="dataTableName"></param>
        private void LoadHotfixDataTable(string dataTableName)
        {
            string data = AssetUtility.GetDataTableAsset(dataTableName , true);
            m_LoadedFlag.Add(data , false);

        }

        #region Event
        /// <summary>
        /// 加载配置成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadConfigSuccess(object sender , GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            Log.Debug("Load config '{0}' OK." , ne.ConfigAssetName);
        }
        /// <summary>
        /// 加载配置失败回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadConfigFailure(object sender , GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'." , ne.ConfigAssetName , ne.ConfigAssetName , ne.ErrorMessage);
        }
        /// <summary>
        /// 加载数据表成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadDataTableSuccess(object sender , GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            Log.Debug("Load data table '{0}' OK." , ne.DataTableAssetName);
        }
        /// <summary>
        /// 加载数据表失败回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadDataTableFailure(object sender , GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'." , ne.DataTableAssetName , ne.DataTableAssetName , ne.ErrorMessage);
        }
        /// <summary>
        /// 加载字典成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadDictionarySuccess(object sender , GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            Log.Debug("Load dictionary '{0}' OK." , ne.DictionaryAssetName);
        }
        /// <summary>
        /// 加载字典失败回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadDictionaryFailure(object sender , GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'." , ne.DictionaryAssetName , ne.DictionaryAssetName , ne.ErrorMessage);
        }
        #endregion
    }
}
