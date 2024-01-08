using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 检查版本流程
    /// </summary>
    internal class BuiltinProcedureCheckVersion:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 检查版本完成
        /// </summary>
        private bool m_CheckVersionComplete;

        /// <summary>
        /// 是否需要更新版本
        /// </summary>
        private bool m_NeedUpdateVersion;

        /// <summary>
        /// 版本信息
        /// </summary>
        private VersionInfo m_VersionInfo;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("<color=lime>进入版本检查流程</color>");
            m_CheckVersionComplete = false;
            m_NeedUpdateVersion = false;
            m_VersionInfo = null;
            WTGame.Event.Subscribe(WebRequestSuccessEventArgs.EventId , OnWebRequestSuccess);
            WTGame.Event.Subscribe(WebRequestFailureEventArgs.EventId , OnWebRequestFailure);
            //请求检查版本
            WTGame.WebRequest.AddWebRequest("");

            Log.Info("信息为{0}" , BuiltinRuntimeUtility.LocalizationLanguage.GameVersion);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            WTGame.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId , OnWebRequestSuccess);
            WTGame.Event.Unsubscribe(WebRequestFailureEventArgs.EventId , OnWebRequestFailure);
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
                ChangeState<BuiltinProcedureUpdateVersion>(procedureOwner);
            }
            else
            {
                //进入检查资源流程
                ChangeState<BuiltinProcedureCheckResources>(procedureOwner);
            }
        }

        /// <summary>
        /// web请求成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebRequestSuccess(object sender , GameEventArgs e)
        {
            WebRequestSuccessEventArgs args = (WebRequestSuccessEventArgs)e;
            if(args.UserData != this)
            {
                return;
            }
            byte[] versionInofBytes = args.GetWebResponseBytes( );
            string versionInfoString = Utility.Converter.GetString(versionInofBytes);
            m_VersionInfo = Utility.Json.ToObject<VersionInfo>(versionInfoString);
            if(m_VersionInfo == null)
            {
                Log.Error("解析版本文件失败");
                return;
            }
            if(m_VersionInfo.ForceUpdateGame)
            {
                Log.Info("打开强制更新弹窗");
                return;
            }


            m_CheckVersionComplete = true;
            m_NeedUpdateVersion = WTGame.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion) == CheckVersionListResult.NeedUpdate;
        }
        /// <summary>
        /// web请求失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebRequestFailure(object sender , GameEventArgs e)
        {
            WebRequestFailureEventArgs args = (WebRequestFailureEventArgs)e;
            if(args.UserData != this)
            {
                return;
            }
            Log.Error("请求版本文件失败,错误信息为:'{0}'." , args.ErrorMessage);
        }
    }
}
