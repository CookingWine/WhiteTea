using UnityEngine;
using UnityEngine.UI;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{

	public partial class TransitionInterface
	{

		private Image m_Img_TransitionBg;
		private Text m_Txt_Content;

		private void GetBindComponents(BuiltinComponentAutoBindTool autoBindTool)
		{
			m_Img_TransitionBg = autoBindTool.GetBindComponent<Image>(0);
			m_Txt_Content = autoBindTool.GetBindComponent<Text>(1);
		}

	}
}
