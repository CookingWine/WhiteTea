using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 检查版本流程
    /// </summary>
    internal class BuiltinProcedureCheckVersion:BuiltinProcedureBase
    {
        /// <summary>
        /// 检查版本完毕
        /// </summary>
        private bool m_CheckVersionComplete = false;

        /// <summary>
        /// 需要更新版本
        /// </summary>
        private bool m_NeedUpdateVersion = false;

        /// <summary>
        /// 版本信息
        /// </summary>
        private VersionInfo m_VersionInfo = null;

        private void InitValue( )
        {
            m_CheckVersionComplete = false;
            m_NeedUpdateVersion = false;
            m_VersionInfo = null;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入【检查版本】流程");
            InitValue( );
            GameCollectionEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId , OnWebRequestSuccess);
            GameCollectionEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId , OnWebRequestFailure);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            GameCollectionEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId , OnWebRequestSuccess);
            GameCollectionEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId , OnWebRequestFailure);
            base.OnLeave(procedureOwner , isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(!m_CheckVersionComplete)
            {
                return;
            }
            if(m_NeedUpdateVersion)
            {
                //进入更新版本流程

            }
            else
            {
                //进入检查资源流程
            }
        }





        /// <summary>
        /// web请求成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebRequestSuccess(object sender , GameEventArgs e)
        {
            WebRequestSuccessEventArgs ea = (WebRequestSuccessEventArgs)e;
            if(ea.UserData != this)
            {
                Log.Error("Parse VersionInfo failure");
                return;
            }
            //解析版本信息
            byte[] versionInfoBytes = ea.GetWebResponseBytes( );
            string versionInfoString = Utility.Converter.GetString(versionInfoBytes);
            m_VersionInfo = Utility.Json.ToObject<VersionInfo>(versionInfoString);
            if(m_VersionInfo == null)
            {
                return;
            }

        }
        private void OnWebRequestFailure(object sender , GameEventArgs e)
        {
            WebRequestFailureEventArgs ea = (WebRequestFailureEventArgs)e;
            if(ea.UserData != this)
            {
                return;
            }
            Log.Error("Check version failure,error message is {0}" , ea.ErrorMessage);
        }
    }
}
