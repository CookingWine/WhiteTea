namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 请求数据
    /// </summary>
    public class RequestData:IReference
    {
        public int cmd { get; set; }

        public Request data { get; set; }

        public RequestData( )
        {
            cmd = 0;
            data = null;
        }
        /// <summary>
        /// 创建一个请求数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RequestData Create(int cmd , Request data)
        {
            RequestData netStruct = ReferencePool.Acquire<RequestData>( );
            netStruct.cmd = cmd;
            netStruct.data = data;
            return netStruct;
        }
        public void Clear( )
        {

        }
    }
    /// <summary>
    /// 响应数据
    /// </summary>
    public class ResponseData
    {
        public int cmd { get; set; }
        public Resp data { get; set; }
        public bool status { get; set; }
    }
}
