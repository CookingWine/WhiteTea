namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 事件参数
    /// </summary>
    public abstract class HotfixGameEventArgs:IReference
    {
        /// <summary>
        /// 获取类型编号。
        /// </summary>
        public abstract int Id
        {
            get;
        }
        public abstract void Clear( );
    }
}
