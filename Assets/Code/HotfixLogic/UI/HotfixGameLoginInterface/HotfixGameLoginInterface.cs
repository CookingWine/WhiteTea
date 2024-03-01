using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 热更登录界面
    /// </summary>
    public partial class HotfixGameLoginInterface:BuiltinUGuiForm
    {
        private ProcedureLogin m_LoginPorcedure = null;
        protected override void OnInit(object userdata)
        {
            base.OnInit(userdata);
            GetBindComponents(ComponentTool);
            m_LoginPorcedure = userdata as ProcedureLogin;
            HotfixEntry.Timer.AddTimer(0.2f , WTGame.BuiltinData.CloseGameMainInterface);
            InitLoginInterfaceEvent( );
        }

        protected override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);
        }

        protected override void OnClose(bool isShutdown , object userdata)
        {
            base.OnClose(isShutdown , userdata);
        }

        protected override void OnUpdate(float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds , realElapseSeconds);
        }

        /// <summary>
        /// 初始化登录界面事件
        /// </summary>
        private void InitLoginInterfaceEvent( )
        {
            //先进行事件绑定
            EventDataDinding( );
            //根据本地是否有用户数据进行更新显示
            UpdateInterfaceExhibition( );
        }


        /// <summary>
        /// 点击开始游戏执行事件
        /// </summary>
        private void StartGameClickCallback( )
        {
            if(!m_Tog_Toggle.isOn)
            {
                OpenPopUpWindwos("提示" , "请先同意用户条款");
                return;
            }
            WTGame.Sound.StopMusic( );
            m_LoginPorcedure?.NextProcedure( );

        }
        /// <summary>
        /// 点击注销后执行
        /// </summary>
        private void LogoutClickCallback( )
        {

            OpenPopUpWindwos("提示" , "暂时无法注销");
        }
        /// <summary>
        /// 点击帮助执行
        /// </summary>
        private void HelpClickCallback( )
        {
            OpenPopUpWindwos("提示" , "我不愿意帮助你，谢谢!");
        }
        /// <summary>
        /// 点击单机执行
        /// </summary>
        private void StandAloneCallback( )
        {
            OpenPopUpWindwos("提示" , "暂时不想做单机，谢谢！");
        }
        /// <summary>
        /// 点击静音执行
        /// </summary>
        private void SoundCallback( )
        {
            SystemSettings.Instance.ChangeTotalGameVolumeState( );
        }

        /// <summary>
        /// 打开弹窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        private void OpenPopUpWindwos(string title , string content)
        {
            WTGame.UI.OpenUIForm(UIFormId.PopUpWindows , new PopUpWindowsDataConvert(title , content));
        }
        
        /// <summary>
        /// 初始化数据与事件得绑定
        /// </summary>
        private void EventDataDinding( )
        {
            m_Tog_Toggle.onValueChanged.AddListener((ison) =>
            {
                SystemSettings.Instance.GameUser.IsAgreeToUserTerms = ison;
            });
            m_Btn_Logout.onClick.AddListener(LogoutClickCallback);
            m_Btn_Help.onClick.AddListener(HelpClickCallback);
            m_Btn_StandAlone.onClick.AddListener(StandAloneCallback);
            m_Btn_Sound.onClick.AddListener(SoundCallback);
            m_Btn_StartGame.onClick.AddListener(StartGameClickCallback);
        }

        /// <summary>
        /// 更新界面展示
        /// </summary>
        private void UpdateInterfaceExhibition( )
        {
            m_Tog_Toggle.isOn = SystemSettings.Instance.GameUser.UserDataExistsLocally && SystemSettings.Instance.GameUser.IsAgreeToUserTerms;
        }
    }
}
