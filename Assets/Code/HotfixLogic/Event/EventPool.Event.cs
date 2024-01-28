namespace WhiteTea.HotfixLogic
{
    public sealed partial class EventPool<T> where T : HotfixGameEventArgs
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event
        {
            /// <summary>
            /// 发送者
            /// </summary>
            private readonly object m_Sender;
            /// <summary>
            /// 事件参数
            /// </summary>
            private readonly T m_EventArgs;
            /// <summary>
            /// 事件节点
            /// </summary>
            /// <param name="sender">发送者</param>
            /// <param name="e">事件参数</param>
            public Event(object sender , T e)
            {
                m_Sender = sender;
                m_EventArgs = e;
            }
            /// <summary>
            /// 发送者
            /// </summary>
            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }
            /// <summary>
            /// 事件参数
            /// </summary>
            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }
        }
    }
}
