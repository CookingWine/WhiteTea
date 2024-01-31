using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

    public partial class HotfixGameLoginInterface:BuiltinUGuiForm
    {
        protected override void OnInit(object userdata)
        {
            base.OnInit(userdata);
            GetBindComponents(ComponentTool);
            HotfixEntry.Timer.AddTimer(0.2f , WTGame.BuiltinData.EnableResourceUI);
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
            m_Tog_Toggle.isOn = false;
            m_Btn_Logout.onClick.AddListener(LogoutClickCallback);
            m_Btn_Help.onClick.AddListener(HelpClickCallback);
            m_Btn_StandAlone.onClick.AddListener(StandAloneCallback);
            m_Btn_Sound.onClick.AddListener(SoundCallback);
            m_Btn_StartGame.onClick.AddListener(StartGameClickCallback);
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
            WTGame.UI.OpenUIForm(UIFormId.TransitionInterface);
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
            OpenPopUpWindwos("提示" , "等待系统设置开发，谢谢！");
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

    }
}
