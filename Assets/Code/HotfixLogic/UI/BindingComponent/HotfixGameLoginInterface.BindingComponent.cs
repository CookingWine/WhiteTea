using UnityEngine;
using UnityEngine.UI;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

	public partial class HotfixGameLoginInterface
	{

		private Button m_Btn_Logout;
		private Button m_Btn_Help;
		private Button m_Btn_StandAlone;
		private Button m_Btn_Sound;
		private Button m_Btn_StartGame;
		private Toggle m_Tog_Toggle;

		private void GetBindComponents(BuiltinComponentAutoBindTool autoBindTool)
		{
			m_Btn_Logout = autoBindTool.GetBindComponent<Button>(0);
			m_Btn_Help = autoBindTool.GetBindComponent<Button>(1);
			m_Btn_StandAlone = autoBindTool.GetBindComponent<Button>(2);
			m_Btn_Sound = autoBindTool.GetBindComponent<Button>(3);
			m_Btn_StartGame = autoBindTool.GetBindComponent<Button>(4);
			m_Tog_Toggle = autoBindTool.GetBindComponent<Toggle>(5);
		}

	}
}
