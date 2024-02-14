using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 切换场景流程
    /// </summary>
    public class ProcedureChangeScene:ProcedureBase
    {
        /// <summary>
        /// 是否切换场景完毕
        /// </summary>
        private bool m_IsChangeSceneComplete = false;

        /// <summary>
        /// 要切换目标场景ID
        /// </summary>
        private int m_TargetSceneId = 0;

        /// <summary>
        /// 背景音乐ID
        /// </summary>
        private int m_BackgroundMusicId = 0;

        /// <summary>
        /// 场景ID-流程切换方法的字典
        /// </summary>
        private readonly Dictionary<int , Action> m_TargetProcedureChange = new Dictionary<int , Action>( );
        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_TargetProcedureChange.Add((int)ScenesId.HotfixEntryScenes , ( ) =>
            {
                ChangeState<ProcedureLogin>(procedureOwner);
            });
        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_IsChangeSceneComplete = false;

            WTGame.Event.Subscribe(LoadSceneSuccessEventArgs.EventId , OnLoadSceneSuccess);
            WTGame.Event.Subscribe(LoadSceneFailureEventArgs.EventId , OnLoadSceneFailure);
            WTGame.Event.Subscribe(LoadSceneUpdateEventArgs.EventId , OnLoadSceneUpdate);
            WTGame.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId , OnLoadSceneDependencyAsset);

            //停止加载所以音效
            WTGame.Sound.StopAllLoadingSounds( );
            WTGame.Sound.StopAllLoadedSounds( );

            //隐藏所有的实体
            WTGame.Entity.HideAllLoadingEntities( );
            WTGame.Entity.HideAllLoadedEntities( );
            //卸载所以场景
            string[] loadSceneAssetName = WTGame.Scene.GetLoadedSceneAssetNames( );
            for(int i = 0; i < loadSceneAssetName.Length; i++)
            {
                WTGame.Scene.UnloadScene(loadSceneAssetName[i]);
            }
            //还原游戏速度
            WTGame.Base.ResetNormalGameSpeed( );
            m_TargetSceneId = procedureOwner.GetData<VarInt32>(HotfixConstantUtility.NextSceneID).Value;
            IDataTable<DRScenes> dtScene = WTGame.DataTable.GetDataTable<DRScenes>( );
            DRScenes drScene = dtScene.GetDataRow(m_TargetSceneId);
            if(drScene == null)
            {

                return;
            }
            WTGame.Scene.LoadScene(BuiltinRuntimeUtility.AssetsUtility.GetSceneAsset(drScene.AssetName) , 0 , this);
            m_BackgroundMusicId = drScene.BackgroundMusicId;
        }
        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            WTGame.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId , OnLoadSceneSuccess);
            WTGame.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId , OnLoadSceneFailure);
            WTGame.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId , OnLoadSceneUpdate);
            WTGame.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId , OnLoadSceneDependencyAsset);
            base.OnLeave(procedureOwner , isShutdown);
        }
        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_IsChangeSceneComplete)
            {
                return;
            }
            if(m_TargetProcedureChange.ContainsKey(m_TargetSceneId))
            {
                m_TargetProcedureChange[m_TargetSceneId]?.Invoke( );
            }
        }

        /// <summary>
        /// 场景加载成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneSuccess(object sender , GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            if(m_BackgroundMusicId > 0)
            {
                WTGame.Sound.PlayMusic(m_BackgroundMusicId);
            }

            m_IsChangeSceneComplete = true;
        }
        /// <summary>
        /// 场景加载失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneFailure(object sender , GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'." , ne.SceneAssetName , ne.ErrorMessage);
        }
        /// <summary>
        /// 加载场景更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneUpdate(object sender , GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'." , ne.SceneAssetName , ne.Progress.ToString("P2"));
        }

        /// <summary>
        /// 加载场景时加载依赖资源事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneDependencyAsset(object sender , GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'." , ne.SceneAssetName , ne.DependencyAssetName , ne.LoadedCount.ToString( ) , ne.TotalCount.ToString( ));
        }
    }
}
