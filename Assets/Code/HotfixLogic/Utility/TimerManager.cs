using System.Collections.Generic;
using System;
using UnityGameFramework.Runtime;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager
    {
        /// <summary>
        /// 计时器列表
        /// </summary>
        private readonly List<GameTimer> m_TimerList = new List<GameTimer>( );

        /// <summary>
        /// 计时器个数
        /// </summary>
        public int Count
        {
            get
            {
                return m_TimerList.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位</param>
        public void UpdateTimer(float elapseSeconds , float realElapseSeconds)
        {
            for(int i = 0; i < m_TimerList.Count; i++)
            {
                m_TimerList[i].Update(elapseSeconds , realElapseSeconds);
            }
        }
        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="_duration">持续时间</param>
        /// <param name="_timeoutCallBack">结束回调</param>
        /// <returns>计时器</returns>
        public GameTimer AddTimer(float _duration , Action _timeoutCallBack)
        {
            return AddTimer(_duration , _timeoutCallBack , false , -1 , null);
        }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="_duration">持续时间</param>
        /// <param name="_timeoutCallBack">结束回调</param>
        /// <param name="_isIgnoreTime">是否忽略时间</param>
        /// <param name="_interval">重复间隔</param>
        /// <param name="_intervalCallBack">每次重复回调</param>
        /// <returns>计时器</returns>
        public GameTimer AddTimer(float _duration , Action _timeoutCallBack , bool _isIgnoreTime = false , float _interval = -1f , Action _intervalCallBack = null)
        {
            GameTimer timer = GetTimer( );
            timer.InITtimer(_duration , _timeoutCallBack , _isIgnoreTime , _interval , _intervalCallBack);
            return timer;
        }


        /// <summary>
        /// 清除所有计时器
        /// </summary>
        public void Shotdown( )
        {
            m_TimerList.Clear( );
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        /// <returns></returns>
        private GameTimer GetTimer( )
        {
            GameTimer timer = null;
            for(int i = 0; i < m_TimerList.Count; i++)
            {
                if(!m_TimerList[i].IsUsed)
                {
                    timer = m_TimerList[i];
                    break;
                }
            }
            if(timer == null)
            {
                timer = new GameTimer( );
                m_TimerList.Add(timer);
            }
            return timer;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        public class GameTimer
        {
            /// <summary>
            /// 持续时间
            /// </summary>
            private float m_Duration;

            /// <summary>
            /// 重复间隔时间
            /// </summary>
            private float m_Interval;

            /// <summary>
            /// 结束回调
            /// </summary>
            private Action m_TimeOutCallback;

            /// <summary>
            /// 每次重复回调
            /// </summary>
            private Action m_IntervalCallback;

            /// <summary>
            /// 是否忽略时间
            /// </summary>
            private bool m_IsIgnoreTime;

            /// <summary>
            /// 计时器
            /// </summary>
            private float m_RunTime;

            /// <summary>
            /// 间隔计时器
            /// </summary>
            private float m_RunIntervalTime;

            /// <summary>
            /// 是否使用
            /// </summary>
            public bool IsUsed { get; private set; }

            public GameTimer( )
            {
                IsUsed = false;
            }
            /// <summary>
            /// 初始化计时器
            /// </summary>
            /// <param name="_duration">持续时间</param>
            /// <param name="_timeoutCallBack">结束回调</param>
            /// <param name="_isIgnoreTime">是否忽略时间</param>
            /// <param name="_interval">重复间隔时间</param>
            /// <param name="_intervalCallBack">每次重复回调</param>
            public void InITtimer(float _duration , Action _timeoutCallBack , bool _isIgnoreTime = false , float _interval = -1f , Action _intervalCallBack = null)
            {
                m_Duration = _duration;
                m_Interval = _interval;
                m_TimeOutCallback = _timeoutCallBack;
                m_IntervalCallback = _intervalCallBack;
                m_IsIgnoreTime = _isIgnoreTime;
                m_RunTime = 0;
                m_RunIntervalTime = 0;
                IsUsed = true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位</param>
            public void Update(float elapseSeconds , float realElapseSeconds)
            {
                if(!IsUsed) return;
                float deltaTime = m_IsIgnoreTime ? realElapseSeconds : elapseSeconds;

                m_RunTime += deltaTime;

                //用于重复间隔调用时
                if(m_IntervalCallback != null && m_Interval > 0)
                {
                    m_RunIntervalTime += deltaTime;
                    if(m_RunIntervalTime >= m_Interval)
                    {
                        m_RunIntervalTime -= m_Interval;
                        m_IntervalCallback?.Invoke( );
                    }
                }

                //结束
                if(m_RunTime >= m_Duration)
                {
                    IsUsed = false;
                    m_RunTime = 0;
                    try
                    {
                        m_TimeOutCallback?.Invoke( );
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Timer is Error: {0}" , ex);
                    }
                }
            }

            /// <summary>
            /// 清除计时器
            /// </summary>
            public void Clear( )
            {
                m_TimeOutCallback = null;
                m_IntervalCallback = null;
                IsUsed = false;
                m_RunTime = 0;
                m_RunIntervalTime = 0;
            }
        }
    }
}
