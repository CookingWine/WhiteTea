using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
using GameFramework.Event;
using System.Collections.Generic;
using GameFramework.Resource;
using UnityEngine;
namespace WhiteTea.HotfixLogic
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
            Log.Debug("<color=line>进入热更入口</color>");
            WTGame.Event.Subscribe(LoadConfigSuccessEventArgs.EventId , OnLoadConfigSuccess);
            WTGame.Event.Subscribe(LoadConfigFailureEventArgs.EventId , OnLoadConfigFailure);
            WTGame.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId , OnLoadDataTableSuccess);
            WTGame.Event.Subscribe(LoadDataTableFailureEventArgs.EventId , OnLoadDataTableFailure);
            WTGame.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId , OnLoadDictionarySuccess);
            WTGame.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId , OnLoadDictionaryFailure);
            m_LoadedFlag.Clear( );
            m_LoadSuccess = 0;
            m_current = 0;
            StartLoadResources( );
        }
        private float m_current;
        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            GetLoadSuccessCount( );
            m_current += elapseSeconds;
            if(m_current > 2.0f && m_LoadSuccess >= m_LoadedFlag.Count)
            {
                procedureOwner.SetData<VarInt32>(HotfixConstantUtility.NextSceneID , (int)ScenesId.HotfixEntryScenes);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }

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
        /// <summary>
        /// 加载资源
        /// </summary>
        private void StartLoadResources( )
        {
            for(int i = 0; i < HotfixEntry.AppRuntimeConfig.DataTables.Length; i++)
            {
                LoadDataTable(HotfixEntry.AppRuntimeConfig.DataTables[i]);
            }
            for(int i = 0; i < HotfixEntry.FontManagers.FontAssets.Length; i++)
            {
                LoadFont(HotfixEntry.FontManagers.FontAssets[i]);
            }
        }
        /// <summary>
        /// 获取加载成功得个数
        /// </summary>
        /// <returns></returns>
        private int GetLoadSuccessCount( )
        {
            m_LoadSuccess = 0;
            IEnumerator<bool> item = m_LoadedFlag.Values.GetEnumerator( );
            while(item.MoveNext( ))
            {
                if(item.Current)
                {
                    m_LoadSuccess++;
                }
            }
            return m_LoadSuccess;
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <param name="dataTableName"></param>
        private void LoadDataTable(string dataTableName)
        {
            string dataAssetsName = BuiltinRuntimeUtility.AssetsUtility.GetDataTableAsset(dataTableName);
            m_LoadedFlag.Add(dataAssetsName , false);
            WTGame.DataTable.LoadDataTable(dataTableName , dataAssetsName , this);
        }
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="dictionaryName"></param>
        private void LoadDictionary(string dictionaryName)
        {
            string dictionaryAsset = BuiltinRuntimeUtility.AssetsUtility.GetDictionaryAsset(dictionaryName , false);
            m_LoadedFlag.Add(dictionaryAsset , false);

        }
        /// <summary>
        /// 加载字体
        /// </summary>
        /// <param name="fontName"></param>
        private void LoadFont(string fontName)
        {
            string fontAssetName = BuiltinRuntimeUtility.AssetsUtility.GetFontAsset(fontName);
            m_LoadedFlag.Add(fontAssetName , false);
            WTGame.Resource.LoadAsset(fontAssetName , new LoadAssetCallbacks(LoadFontSuccess , LoadFontFailed));
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
            m_LoadedFlag[ne.DataTableAssetName] = true;
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
        /// <summary>
        /// 加载字体成功回调
        /// </summary>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="asset">已加载的资源。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        private void LoadFontSuccess(string assetName , object asset , float duration , object userData)
        {
            m_LoadedFlag[assetName] = true;
            Font fontAsset = asset as Font;
            HotfixEntry.FontManagers.AddHotfixFontToCache(assetName , fontAsset);
            BuiltinUGuiForm.SetMainFont(fontAsset);

        }
        /// <summary>
        /// 加载字体资源失败回调
        /// </summary>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="status">加载资源状态。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        private void LoadFontFailed(string assetName , LoadResourceStatus status , string errorMessage , object userData)
        {
            Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'" , assetName , assetName , errorMessage);
        }
        #endregion
    }
}
