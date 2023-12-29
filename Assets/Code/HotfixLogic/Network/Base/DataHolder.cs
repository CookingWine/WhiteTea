using System;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 数据持有者
    /// </summary>
    public class DataHolder
    {
        /// <summary>
        /// 数据缓存
        /// </summary>
        private byte[] m_RecvDataCache;

        /// <summary>
        /// 数据
        /// </summary>
        private byte[] m_RecvData;

        /// <summary>
        /// 尾部
        /// </summary>
        private int m_Tail = -1;

        /// <summary>
        /// 包长度
        /// </summary>
        private int m_PackLength;

        /// <summary>
        /// 指示当前缓存中有多少数据(以字节为单位) 
        /// </summary>
        public int CacheCount
        {
            get
            {
                return m_Tail + 1;
            }
        }
        /// <summary>
        /// 缓存容量
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_RecvDataCache != null ? m_RecvDataCache.Length : 0;
            }
        }
        /// <summary>
        /// 推送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="length">长度</param>
        public void PushData(byte[] data , int length)
        {
            if(m_RecvDataCache == null)
            {
                m_RecvDataCache = new byte[length];
            }
            if(CacheCount + length > Capacity)
            {
                byte[] newArr = new byte[CacheCount + length];
                m_RecvDataCache.CopyTo(newArr , 0);
                m_RecvDataCache = newArr;
            }
            Array.Copy(data , 0 , m_RecvDataCache , m_Tail + 1 , length);
            m_Tail += length;
        }
        /// <summary>
        /// 是否结束
        /// </summary>
        /// <returns></returns>
        public bool IsFinished( )
        {
            if(CacheCount == 0)
            {
                //如果当前缓存中没有数据，则跳过
                return false;
            }
            if(CacheCount >= 4)
            {
                DataStream reader = new DataStream(m_RecvDataCache , true);
                m_PackLength = (int)reader.ReadInt32( );
                if(m_PackLength > 0)
                {
                    if(CacheCount - 4 >= m_PackLength)
                    {
                        m_RecvData = new byte[m_PackLength];
                        Array.Copy(m_RecvDataCache , 4 , m_RecvData , 0 , m_PackLength);
                        return true;
                    }

                    return false;
                }
                return false;
            }

            return false;
        }
        ///<summary>重置</summary>
        public void Reset( )
        {
            m_Tail = -1;
        }
        /// <summary>
        /// 从头删除
        /// </summary>
        public void RemoveFromHead( )
        {
            int countToRemove = m_PackLength + 4;
            if(countToRemove > 0 && CacheCount - countToRemove > 0)
            {
                Array.Copy(m_RecvDataCache , countToRemove , m_RecvDataCache , 0 , CacheCount - countToRemove);
            }
            m_Tail -= countToRemove;
        }
    }
}
