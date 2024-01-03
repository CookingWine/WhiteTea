using System.IO;
namespace WhiteTea.GameEditor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        /// <summary>
        /// 数据处理器
        /// </summary>
        public abstract class DataProcessor
        {
            public abstract System.Type Type
            {
                get;
            }

            public abstract bool IsId
            {
                get;
            }
            public abstract bool IsComment
            {
                get;
            }

            public abstract bool IsSystem
            {
                get;
            }

            public abstract string LanguageKeyword
            {
                get;
            }

            public abstract string[] GetTypeStrings( );

            public abstract void WriteToStream(DataTableProcessor dataTableProcessor , BinaryWriter binaryWriter , string value);
        }
    }
}
