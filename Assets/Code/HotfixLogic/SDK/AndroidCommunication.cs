using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// android通信类
    /// </summary>
    public class AndroidCommunication:GameSDKBase
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool m_IsInitialized;

        public override bool Initialized
        {
            get
            {
                return m_IsInitialized;
            }
        }
#if UNITY_EDITOR
        private AndroidJavaClass m_AndroidJavaClass;

        private AndroidJavaObject m_AndroidJavaObject;
#endif
        private const string m_UnityPlayerPackger = "com.findwindpeople.witetea";
        /// <summary>
        /// 实例
        /// </summary>
        public static AndroidCommunication Instance { get; private set; } = null;
        public override void InitializedSDK(Action action)
        {
            if(m_IsInitialized)
            {
                return;
            }

#if UNITY_EDITOR
            m_AndroidJavaClass = new AndroidJavaClass(m_UnityPlayerPackger);
            if(m_AndroidJavaClass == null)
            {
                Log.Error("Faile to get unity palyer class,");
                return;
            }
            m_AndroidJavaObject = new AndroidJavaObject("currentActivity");
            if(m_AndroidJavaObject == null)
            {
                Log.Error("Failed to obtain Android Activity from Unity Player class.");
                return;
            }
#endif
            m_IsInitialized = true;
            Instance = this;
            Log.Debug("初始化Android sdk完成");
            action?.Invoke( );
        }
    }
}
