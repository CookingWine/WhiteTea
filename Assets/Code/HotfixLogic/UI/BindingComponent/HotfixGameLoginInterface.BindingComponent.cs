using UnityEngine;
using UnityEngine.UI;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

	public partial class HotfixGameLoginInterface
	{

		private Button m_Btn_Help;
		private Button m_Btn_StandAlone;
		private Button m_Btn_Sound;
		private CanvasGroup m_Group_RegisterInterface;
		private RectTransform m_Trans_WX;
		private RectTransform m_Trans_QQ;
		private CanvasGroup m_Group_LoginOverInterface;
		private Button m_Btn_StartGame;
		private Toggle m_Tog_Toggle;
		private Button m_Btn_Logout;

		private void GetBindComponents(BuiltinComponentAutoBindTool autoBindTool)
		{
			m_Btn_Help = autoBindTool.GetBindComponent<Button>(0);
			m_Btn_StandAlone = autoBindTool.GetBindComponent<Button>(1);
			m_Btn_Sound = autoBindTool.GetBindComponent<Button>(2);
			m_Group_RegisterInterface = autoBindTool.GetBindComponent<CanvasGroup>(3);
			m_Trans_WX = autoBindTool.GetBindComponent<RectTransform>(4);
			m_Trans_QQ = autoBindTool.GetBindComponent<RectTransform>(5);
			m_Group_LoginOverInterface = autoBindTool.GetBindComponent<CanvasGroup>(6);
			m_Btn_StartGame = autoBindTool.GetBindComponent<Button>(7);
			m_Tog_Toggle = autoBindTool.GetBindComponent<Toggle>(8);
			m_Btn_Logout = autoBindTool.GetBindComponent<Button>(9);
		}

	}
}
