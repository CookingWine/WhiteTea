using System;
using System.IO;
using System.Text;
namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 数据流
    /// </summary>
    public class DataStream
    {
        /// <summary>
        /// 读
        /// </summary>
        private BinaryReader m_BinaryReader;

        /// <summary>
        /// 写
        /// </summary>
        private BinaryWriter m_BinaryWriter;

        /// <summary>
        /// 内存流
        /// </summary>
        private MemoryStream m_MemoryStream;

        /// <summary>
        /// 优先模式
        /// </summary>
        private bool m_BEMode;

        /// <summary>
        /// UTF8编码
        /// </summary>
        private UTF8Encoding m_UTF8Eencoding = new UTF8Encoding( );
        /// <summary>
        /// 位置
        /// </summary>
        public long Position
        {
            get
            {
                return m_MemoryStream.Position;
            }
            set
            {
                m_MemoryStream.Position = value;
            }
        }
        /// <summary>
        /// 内存流的长度
        /// </summary>
        public long Length
        {
            get
            {
                return m_MemoryStream.Length;
            }
        }

        /// <summary>
        /// 初始化数据流
        /// </summary>
        /// <param name="isBigEndian"></param>
        public DataStream(bool isBigEndian)
        {
            m_MemoryStream = new MemoryStream( );
            InitWithMemoryStream(m_MemoryStream , isBigEndian);
        }

        /// <summary>
        /// 初始化数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isBigEndian"></param>
        public DataStream(byte[] buffer , bool isBigEndian)
        {
            m_MemoryStream = new MemoryStream(buffer);
            InitWithMemoryStream(m_MemoryStream , isBigEndian);
        }

        /// <summary>
        /// 初始化数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="isBigEndian"></param>
        public DataStream(byte[] buffer , int index , int count , bool isBigEndian)
        {
            m_MemoryStream = new MemoryStream(buffer , index , count);
            InitWithMemoryStream(m_MemoryStream , isBigEndian);
        }
        /// <summary>
        /// 初始化内存流
        /// </summary>
        /// <param name="memoryStream">内存流</param>
        /// <param name="isBigEndian">优先模式</param>
        private void InitWithMemoryStream(MemoryStream memoryStream , bool isBigEndian)
        {
            m_BinaryReader = new BinaryReader(memoryStream);
            m_BinaryWriter = new BinaryWriter(memoryStream);
            m_BEMode = isBigEndian;
        }

        /// <summary>
        /// 设置优先模式
        /// </summary>
        /// <param name="bigEndian"></param>
        public void SetBigEndian(bool bigEndian)
        {
            m_BEMode = bigEndian;
        }
        /// <summary>
        /// 是否是优先模式
        /// </summary>
        /// <returns>优先模式</returns>
        public bool IsBigEndian( )
        {
            return m_BEMode;
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes( )
        {
            return m_MemoryStream.ToArray( );
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public long Seek(long offset , SeekOrigin origin)
        {
            return m_MemoryStream.Seek(offset , origin);
        }

        #region 读写操作
        /// <summary>
        /// 写格式
        /// </summary>
        /// <param name="bytes"></param>
        public void WriteRaw(byte[] bytes)
        {
            m_BinaryWriter.Write(bytes);
        }
        /// <summary>
        /// 写格式
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void WriteRaw(byte[] bytes , int offset , int count)
        {
            m_BinaryWriter.Write(bytes , offset , count);
        }
        /// <summary>
        /// 写字节
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            m_BinaryWriter.Write(value);
        }
        /// <summary>
        /// 读取字节
        /// </summary>
        /// <returns></returns>
        public byte ReadByte( )
        {
            return m_BinaryReader.ReadByte( );
        }

        public void WriteInt16(UInt16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt16 ReadInt16( )
        {
            UInt16 val = m_BinaryReader.ReadUInt16( );
            if(m_BEMode)
                return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteInt32(UInt32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt32 ReadInt32( )
        {
            UInt32 val = m_BinaryReader.ReadUInt32( );
            if(m_BEMode)
                return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteInt64(UInt64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt64 ReadInt64( )
        {
            UInt64 val = m_BinaryReader.ReadUInt64( );
            if(m_BEMode)
                return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteString8(string value)
        {
            byte[] bytes = m_UTF8Eencoding.GetBytes(value);
            m_BinaryWriter.Write((byte)bytes.Length);
            m_BinaryWriter.Write(bytes);
        }

        public string ReadString8( )
        {
            int len = ReadByte( );
            byte[] bytes = m_BinaryReader.ReadBytes(len);
            return m_UTF8Eencoding.GetString(bytes);
        }

        public void WriteString16(string value)
        {
            byte[] data = m_UTF8Eencoding.GetBytes(value);
            WriteInteger(BitConverter.GetBytes((Int16)data.Length));
            m_BinaryWriter.Write(data);
        }

        public string ReadString16( )
        {
            ushort len = ReadInt16( );
            byte[] bytes = m_BinaryReader.ReadBytes(len);
            return m_UTF8Eencoding.GetString(bytes);
        }
        /// <summary>
        /// 写入整数
        /// </summary>
        /// <param name="bytes"></param>
        private void WriteInteger(byte[] bytes)
        {
            if(m_BEMode)
                FlipBytes(bytes);
            m_BinaryWriter.Write(bytes);
        }
        /// <summary>
        /// 翻转字节
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private byte[] FlipBytes(byte[] bytes)
        {
            for(int i = 0, j = bytes.Length - 1; i < j; ++i, --j)
            {
                byte temp = bytes[i];
                bytes[i] = bytes[j];
                bytes[j] = temp;
            }
            return bytes;
        }

        /// <summary>
        /// signed型数据读写
        /// </summary>
        public void WriteSByte(sbyte value)
        {
            m_BinaryWriter.Write(value);
        }

        public sbyte ReadSByte( )
        {
            return m_BinaryReader.ReadSByte( );
        }

        public void WriteSInt16(Int16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int16 ReadSInt16( )
        {
            Int16 val = m_BinaryReader.ReadInt16( );
            if(m_BEMode)
                return BitConverter.ToInt16(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteSInt32(Int32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int32 ReadSInt32( )
        {
            Int32 val = m_BinaryReader.ReadInt32( );
            if(m_BEMode)
                return BitConverter.ToInt32(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteSInt64(Int64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int64 ReadSInt64( )
        {
            Int64 val = m_BinaryReader.ReadInt64( );
            if(m_BEMode)
                return BitConverter.ToInt64(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        /// <summary>
        /// Unsigned型数据读写
        /// </summary>
        public void WriteUByte(byte value)
        {
            m_BinaryWriter.Write(value);
        }

        public byte ReadUByte( )
        {
            return m_BinaryReader.ReadByte( );
        }

        public void WriteUInt16(UInt16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt16 ReadUInt16( )
        {
            UInt16 val = m_BinaryReader.ReadUInt16( );
            if(m_BEMode)
                return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteUInt32(UInt32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt32 ReadUInt32( )
        {
            UInt32 val = m_BinaryReader.ReadUInt32( );
            if(m_BEMode)
                return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }

        public void WriteUInt64(UInt64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt64 ReadUInt64( )
        {
            UInt64 val = m_BinaryReader.ReadUInt64( );
            if(m_BEMode)
                return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)) , 0);
            return val;
        }
        #endregion

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear( )
        {
            m_MemoryStream.Position = 0;
            m_MemoryStream.SetLength(0);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close( )
        {
            m_MemoryStream.Close( );
            m_BinaryReader.Close( );
            m_BinaryWriter.Close( );
        }

    }
}
