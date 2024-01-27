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
            Log.Info("<color=lime>进入【检查版本】流程</color>");
            m_CheckVersionComplete = false;
            m_NeedUpdateVersion = false;
            m_VersionInfo = null;
            WTGame.Event.Subscribe(WebRequestSuccessEventArgs.EventId , OnWebRequestSuccess);
            WTGame.Event.Subscribe(WebRequestFailureEventArgs.EventId , OnWebRequestFailure);
            if(string.IsNullOrEmpty(WTGame.AppBuiltinConfigs.CheckVersionUrl))
            {
                Log.Error("检查版本url丢失.");
                WTGame.Shutdown(ShutdownType.Quit);
                return;
            }
            //请求检查版本
            WTGame.WebRequest.AddWebRequest(WTGame.AppBuiltinConfigs.CheckVersionUrl , this);
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
                procedureOwner.SetData<VarInt32>("VersionListLength" , m_VersionInfo.VersionListLength);
                procedureOwner.SetData<VarInt32>("VersionListHashCode" , m_VersionInfo.VersionListHashCode);
                procedureOwner.SetData<VarInt32>("VersionListCompressedLength" , m_VersionInfo.VersionListCompressedLength);
                procedureOwner.SetData<VarInt32>("VersionListCompressedHashCode" , m_VersionInfo.VersionListCompressedHashCode);
                WTGame.Setting.SetBool("VersionBulletinboard" , m_NeedUpdateVersion);
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
            m_VersionInfo = LitJson.JsonMapper.ToObject<VersionInfo>(versionInfoString);
            if(m_VersionInfo == null)
            {
                Log.Error("解析版本文件失败");
                return;
            }
            string info = $"<color=lime>" +
                $"是否需要强制更新:{m_VersionInfo.ForceUpdateGame}\n" +
                $"最新得游戏版本:{m_VersionInfo.LatestGameVersion}\n" +
                $"最新的游戏内部版本号:{m_VersionInfo.InternalGameVersion}\n" +
                $"最新的资源内部版本号:{m_VersionInfo.InternalResourceVersion}\n" +
                $"资源更新下载地址:{m_VersionInfo.UpdatePrefixUri} \n" +
                $"资源版本列表长度:{m_VersionInfo.VersionListLength} \n" +
                $"资源版本列表哈希值:{m_VersionInfo.VersionListHashCode} \n" +
                $"资源版本列表压缩后长度:{m_VersionInfo.VersionListCompressedLength}\n" +
                $"资源版本列表压缩后哈希值:{m_VersionInfo.VersionListCompressedHashCode}" +
                $"</color>";

            Log.Info(info);
            WTGame.Resource.UpdatePrefixUri = m_VersionInfo.UpdatePrefixUri;
            Log.Debug("当前获取得版本号为{0}更新地址为{1}" , m_VersionInfo.InternalGameVersion , m_VersionInfo.UpdatePrefixUri);
            m_NeedUpdateVersion = WTGame.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion) == CheckVersionListResult.NeedUpdate;
            m_CheckVersionComplete = true;
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
