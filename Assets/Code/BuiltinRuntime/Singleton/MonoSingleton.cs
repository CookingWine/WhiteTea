using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
	/// MonoBehaviour单例基类。线程安全。
	/// </summary>
	public class MonoSingleton<T>:MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_Instance;

        private static readonly object m_SysLock = new object( );

        ///<summary>
        /// 创建一个mono的单例
        ///</summary>
        public static T Instance
        {
            get
            {
                lock(m_SysLock)
                {
                    if(m_Instance == null)
                    {
                        m_Instance = SingletonCreator.CreateMonoSingleton<T>( );
                    }
                }
                return m_Instance;
            }
        }

        /// <summary>
        /// MonoSingleton对象不会在加载新场景时自动销毁。
        /// </summary>
        protected void DontDestroyOnLoad( )
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// 调用此函数。MonoSingleton对象被销毁。
        /// <para></para>
        /// </summary>
        protected virtual void OnDestroy( )
        {
            m_Instance = null;
        }
    }
}
