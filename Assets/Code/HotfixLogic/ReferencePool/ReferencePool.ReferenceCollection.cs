using GameFramework;
using System.Collections.Generic;
using System;

namespace WhiteTea.HotfixLogic
{
    public static partial class ReferencePool
    {
        /// <summary>
        /// 引用池集合
        /// </summary>
        private sealed class ReferenceCollection
        {
            /// <summary>
            /// 引用池队列
            /// </summary>
            private readonly Queue<IReference> m_References;
            /// <summary>
            /// 引用池类型
            /// </summary>
            private readonly Type m_ReferenceType;

            /// <summary>
            /// 正在使用引用数量
            /// </summary>
            private int m_UsingReferenceCount;
            /// <summary>
            /// 获取引用数量
            /// </summary>
            private int m_AcquireReferenceCount;
            /// <summary>
            /// 归还引用数量
            /// </summary>
            private int m_ReleaseReferenceCount;
            /// <summary>
            /// 增加引用数量
            /// </summary>
            private int m_AddReferenceCount;
            /// <summary>
            /// 移除引用数量
            /// </summary>
            private int m_RemoveReferenceCount;
            public ReferenceCollection(Type referenceType)
            {
                m_References = new Queue<IReference>( );
                m_ReferenceType = referenceType;
                m_UsingReferenceCount = 0;
                m_AcquireReferenceCount = 0;
                m_ReleaseReferenceCount = 0;
                m_AddReferenceCount = 0;
                m_RemoveReferenceCount = 0;
            }
            /// <summary>
            /// 获取引用池类型
            /// </summary>
            public Type ReferenceType
            {
                get
                {
                    return m_ReferenceType;
                }
            }
            /// <summary>
            /// 获取未使用引用计数
            /// </summary>
            public int UnusedReferenceCount
            {
                get
                {
                    return m_References.Count;
                }
            }

            /// <summary>
            /// 获取正在使用引用数量。
            /// </summary>
            public int UsingReferenceCount
            {
                get
                {
                    return m_UsingReferenceCount;
                }
            }

            /// <summary>
            /// 获取获取引用数量。
            /// </summary>
            public int AcquireReferenceCount
            {
                get
                {
                    return m_AcquireReferenceCount;
                }
            }

            /// <summary>
            /// 获取归还引用数量。
            /// </summary>
            public int ReleaseReferenceCount
            {
                get
                {
                    return m_ReleaseReferenceCount;
                }
            }

            /// <summary>
            /// 获取增加引用数量。
            /// </summary>
            public int AddReferenceCount
            {
                get
                {
                    return m_AddReferenceCount;
                }
            }

            /// <summary>
            /// 获取移除引用数量。
            /// </summary>
            public int RemoveReferenceCount
            {
                get
                {
                    return m_RemoveReferenceCount;
                }
            }
            /// <summary>
            /// 获取
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            /// <exception cref="GameFrameworkException"></exception>
            public T Acquire<T>( ) where T : class, IReference, new()
            {
                if(typeof(T) != m_ReferenceType)
                {
                    throw new GameFrameworkException("Type is invalid.");
                }

                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;

                if(m_References.Count > 0)
                {
                    return (T)m_References.Dequeue( );
                }


                m_AddReferenceCount++;
                return new T( );
            }
            /// <summary>
            /// 获取
            /// </summary>
            /// <returns></returns>
            public IReference Acquire( )
            {
                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;

                if(m_References.Count > 0)
                {
                    return m_References.Dequeue( );
                }


                m_AddReferenceCount++;
                return (IReference)Activator.CreateInstance(m_ReferenceType);
            }
            /// <summary>
            /// 释放
            /// </summary>
            /// <param name="reference"></param>
            /// <exception cref="GameFrameworkException"></exception>
            public void Release(IReference reference)
            {
                reference.Clear( );

                if(m_References.Contains(reference))
                {
                    throw new GameFrameworkException("The reference has been released.");
                }

                m_References.Enqueue(reference);


                m_ReleaseReferenceCount++;
                m_UsingReferenceCount--;
            }
            /// <summary>
            /// 添加
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="count"></param>
            /// <exception cref="GameFrameworkException"></exception>
            public void Add<T>(int count) where T : class, IReference, new()
            {
                if(typeof(T) != m_ReferenceType)
                {
                    throw new GameFrameworkException("Type is invalid.");
                }


                m_AddReferenceCount += count;
                while(count-- > 0)
                {
                    m_References.Enqueue(new T( ));
                }

            }
            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="count"></param>
            public void Add(int count)
            {

                m_AddReferenceCount += count;
                while(count-- > 0)
                {
                    m_References.Enqueue((IReference)Activator.CreateInstance(m_ReferenceType));
                }

            }
            /// <summary>
            /// 移除
            /// </summary>
            /// <param name="count"></param>
            public void Remove(int count)
            {

                if(count > m_References.Count)
                {
                    count = m_References.Count;
                }

                m_RemoveReferenceCount += count;
                while(count-- > 0)
                {
                    m_References.Dequeue( );
                }

            }
            /// <summary>
            /// 移除所有
            /// </summary>
            public void RemoveAll( )
            {

                m_RemoveReferenceCount += m_References.Count;
                m_References.Clear( );

            }
        }
    }
}
