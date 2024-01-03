using System;
namespace WhiteTea.GameEditor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        public abstract class GenericDataProcessor<T>:DataProcessor
        {
            public override Type Type
            {
                get
                {
                    return typeof(T);
                }
            }

            public override bool IsId
            {
                get
                {
                    return false;
                }
            }
            public override bool IsComment
            {
                get
                {
                    return false;
                }
            }

            public abstract T Parse(string value);
        }
    }
}
