namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 单例基类。基于反射,该线程安全。
    /// </summary>
    public abstract class Singleton<T>:ISingleton where T : Singleton<T>
    {
        protected Singleton( ) { }

        /// <summary>
        /// 实例
        /// </summary>
        private static T m_Instance = null;
        /// <summary>
        /// 
        /// </summary>
        private static readonly object m_SysLock = new object( );
        /// <summary>
		/// 获取一个实例
		/// </summary>
		public static T Instance
        {
            get
            {
                lock(m_SysLock)
                {
                    if(m_Instance == null)
                    {
                        m_Instance = SingletonCreator.CreateSingleton<T>( );
                    }
                }
                return m_Instance;
            }
        }
        /// <summary>
		/// 初始化单例
		/// </summary>
		public virtual void OnSingletonInit( ) { }
    }
}
