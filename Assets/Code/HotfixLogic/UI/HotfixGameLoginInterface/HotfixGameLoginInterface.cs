using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

	public partial class HotfixGameLoginInterface : BuiltinUGuiForm
	{
		protected override void OnInit(object userdata)
		{
			base.OnInit(userdata);
			GetBindComponents(ComponentTool);
            HotfixEntry.Timer.AddTimer(0.2f , WTGame.BuiltinData.EnableResourceUI);
            InitGameEvent( );
        }

		protected override void OnOpen(object userdata)
		{
			base.OnOpen(userdata);
		}

		protected override void OnClose(bool isShutdown, object userdata)
		{
			base.OnClose(isShutdown, userdata);
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
		}

        private void InitGameEvent( )
        {
            m_Btn_Logout.onClick.AddListener(( ) =>
            {
                Log.Debug("点击登录了");
            });
        }
	}
}
