using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 弹窗
    /// </summary>
    public partial class PopUpWindows:BuiltinUGuiForm
    {
        protected override void OnInit(object userdata)
        {
            base.OnInit(userdata);
            GetBindComponents(ComponentTool);

        }

        protected override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);
            PopUpWindowsDataConvert data = userdata as PopUpWindowsDataConvert;
            InitPopUpData(data);
        }

        protected override void OnClose(bool isShutdown , object userdata)
        {
            base.OnClose(isShutdown , userdata);
        }

        protected override void OnUpdate(float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds , realElapseSeconds);
        }


        private void InitPopUpData(PopUpWindowsDataConvert data)
        {
            if(data.PopUpButtonCount < 0)
            {
                m_Btn_ClosePopUp.gameObject.SetActive(true);
                m_Btn_ClosePopUp.onClick.AddListener(Close);
            }
            m_Txt_PopUpTilte.text = data.PopUpTitle;
            m_Txt_PopUpContent.text = data.PopUpContent;

        }
    }
}
