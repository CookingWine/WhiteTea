using UnityEngine;

namespace WhiteTea.GameEditor.GameConfigs
{
    internal partial class WhiteTeaGameConfigs
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        private const string m_WindowName = "游戏配置器";
        /// <summary>
        /// 窗口最小的大小
        /// </summary>
        private static Vector2 m_WindowMinSize = new Vector2(800f , 600f);
        /// <summary>
        /// 左边区域的滚动视图
        /// </summary>
        private Vector2 m_LeftAreaSlider;
        /// <summary>
        /// 右边区域的滚动视图
        /// </summary>
        private Vector2 m_RightAreaSlider;
    }
}
