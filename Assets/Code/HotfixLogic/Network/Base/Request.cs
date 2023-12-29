using GameFramework;
using System.Text;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 请求
    /// </summary>
    public abstract class Request
    {
        /// <summary>
        /// 协议
        /// </summary>
        protected abstract int Protocol { get; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public virtual void Serialize(DataStream writer)
        {
            writer.WriteSInt32(Protocol);
            writer.WriteString16(ToJson( ));
        }

        /// <summary>
        /// 序列化json
        /// </summary>
        /// <returns></returns>
        public virtual byte[] SerializeJson( )
        {
            byte[] bytes = Encoding.UTF8.GetBytes(ToJson( ));
            return bytes;
        }

        public virtual string ToJson( )
        {
            RequestData netStruct = RequestData.Create(Protocol , this);
            string json = Utility.Json.ToJson(netStruct);
            ReferencePool.Release(netStruct);
            return json;
        }

        ///<summary>发送</summary>
        public void Send( )
        {

        }
    }
}
