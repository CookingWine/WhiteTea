using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
using GameFramework.Event;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 热更流程的入口
    /// </summary>
    public class ProcedureHotfixEntry:ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("<color=line>进入热更入口</color>");
            WTGame.Event.Subscribe(LoadConfigSuccessEventArgs.EventId , OnLoadConfigSuccess);
            WTGame.Event.Subscribe(LoadConfigFailureEventArgs.EventId , OnLoadConfigFailure);
            WTGame.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId , OnLoadDataTableSuccess);
            WTGame.Event.Subscribe(LoadDataTableFailureEventArgs.EventId , OnLoadDataTableFailure);
            WTGame.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId , OnLoadDictionarySuccess);
            WTGame.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId , OnLoadDictionaryFailure);
        }
        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
        }
        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            //取消监听事件
            WTGame.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId , OnLoadConfigSuccess);
            WTGame.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId , OnLoadConfigFailure);
            WTGame.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId , OnLoadDataTableSuccess);
            WTGame.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId , OnLoadDataTableFailure);
            WTGame.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId , OnLoadDictionarySuccess);
            WTGame.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId , OnLoadDictionaryFailure);
            base.OnLeave(procedureOwner , isShutdown);
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
