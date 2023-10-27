using GameFramework.Event;
using System;
using System.Collections.Generic;
using UGHGame.BuiltinRuntime;
using UnityGameFramework.Runtime;
using ProcedureOwner = UGHGame.HotfixLogic.IFsm;
namespace UGHGame.HotfixLogic
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

        ///// <summary>
        ///// 背景音乐ID
        ///// </summary>
        //private int m_BackgroundMusicId = 0;

        /// <summary>
        /// 切换目标场景ID
        /// </summary>
        private int m_TargetSceneId = 0;

        /// <summary>
        /// 场景ID-流程切换方法的字典
        /// </summary>
        private readonly Dictionary<int , Action> m_TragetProcedureChange = new Dictionary<int , Action>( );

        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            //TODO:配置场景ID与切换对应流程的方法

        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_IsChangeSceneComplete = false;
            //注册场景加载事件
            GameCollectionEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId , OnLoadSceneSuccess);
            GameCollectionEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId , OnLoadSceneFailure);
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_IsChangeSceneComplete)
            {
                return;
            }
            //根据切换目标场景ID执行对应事件
            if(m_TragetProcedureChange.ContainsKey(m_TargetSceneId))
            {
                m_TragetProcedureChange[m_TargetSceneId]?.Invoke( );
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            //注销场景加载事件
            GameCollectionEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId , OnLoadSceneSuccess);
            GameCollectionEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId , OnLoadSceneFailure);

            base.OnLeave(procedureOwner , isShutdown);
        }
        /// <summary>
        /// 场景加载成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneSuccess(object sender , GameEventArgs e)
        {
            LoadSceneSuccessEventArgs args = (LoadSceneSuccessEventArgs)e;
            if(args.UserData != this)
            {
                return;
            }
            Log.Debug("Load scene 【{0}】OK." , args.SceneAssetName);
        }

        /// <summary>
        /// 场景加载失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadSceneFailure(object sender , GameEventArgs e)
        {
            LoadSceneFailureEventArgs args = (LoadSceneFailureEventArgs)e;
            if(args.UserData != this)
            {
                return;
            }
            Log.Error("Load scene【{0}】 failure,error message:{1}" , args.SceneAssetName , args.ErrorMessage);
        }
    }
}
