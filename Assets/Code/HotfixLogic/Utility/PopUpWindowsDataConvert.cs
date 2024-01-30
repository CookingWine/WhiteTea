namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 弹窗的数据转换
    /// </summary>
    public class PopUpWindowsDataConvert
    {
        /// <summary>
        /// 弹窗按钮的数量
        /// </summary>
        public int PopUpButtonCount
        {
            get;
        }
        /// <summary>
        /// 弹窗标题
        /// </summary>
        public string PopUpTitle
        {
            get;
        }
        /// <summary>
        /// 弹窗内容
        /// </summary>
        public string PopUpContent
        {
            get;
        }

        /// <summary>
        /// 设置弹窗数据
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public PopUpWindowsDataConvert(string title , string content)
        {
            PopUpButtonCount = -1;
            PopUpTitle = title;
            PopUpContent = content;
        }
    }
}
