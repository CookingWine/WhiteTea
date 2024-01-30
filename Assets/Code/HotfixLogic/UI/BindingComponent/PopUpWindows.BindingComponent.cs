using UnityEngine;
using UnityEngine.UI;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

	public partial class PopUpWindows
	{

		private Image m_Img_PopUpBackground;
		private Text m_Txt_PopUpTilte;
		private Text m_Txt_PopUpContent;
		private Button m_Btn_ClosePopUp;

		private void GetBindComponents(BuiltinComponentAutoBindTool autoBindTool)
		{
			m_Img_PopUpBackground = autoBindTool.GetBindComponent<Image>(0);
			m_Txt_PopUpTilte = autoBindTool.GetBindComponent<Text>(1);
			m_Txt_PopUpContent = autoBindTool.GetBindComponent<Text>(2);
			m_Btn_ClosePopUp = autoBindTool.GetBindComponent<Button>(3);
		}

	}
}
