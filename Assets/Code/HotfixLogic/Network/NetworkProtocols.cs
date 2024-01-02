namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 网络协议
    /// </summary>
    public static class NetworkProtocols
    {
        //----------------------------------推送服务器---------------------------------------------------//
        /// <summary>
        /// 心跳
        /// </summary>
        public const int HeartBeat = 9999;


        //----------------------------------本地接收---------------------------------------------------//

        /// <summary>
        /// 服务器断开连接
        /// </summary>
        public const int SC_ServerDisconnect = -1;
    }
}
