using System.Collections.Generic;
using UGHGame.BuiltinRuntime;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 响应
    /// </summary>
    public abstract class Resp
    {
        public string JsonData { get; private set; }

        public abstract int Protocol( );

        protected Dictionary<string , object> m_JsonKeyValue = null;

        public virtual void Deserialize(DataStream stream)
        {
            JsonData = stream.ReadString16( );
            Dictionary<string , object> json = MiniJson.Deserialize(JsonData) as Dictionary<string , object>;
            m_JsonKeyValue = json["data"] as Dictionary<string , object>;
        }
    }
}
