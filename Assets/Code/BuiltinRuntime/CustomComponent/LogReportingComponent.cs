using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 日志上报组件
    /// </summary>
    public class LogReportingComponent
    {
        private LogReportingComponent( ) { }

        private static LogReportingComponent m_Instance = null;
        /// <summary>
        /// log文件的路径
        /// </summary>
        public static string LogFilePath = string.Empty;

        private const string m_LogFile = "GameLog";
        readonly List<DebuggerComponent.LogNode> log = new List<DebuggerComponent.LogNode>( );
        public static LogReportingComponent Create( )
        {
            if(m_Instance == null)
            {
                LogFilePath = string.Empty;
                InitLogReporting( );
                m_Instance = new LogReportingComponent( );
            }
            return m_Instance;
        }

        public void Update( )
        {
            GameCollectionEntry.Debugger.GetRecentLogs(log);
            for(int i = 0; i < log.Count; i++)
            {
                if(log[i].LogType == LogType.Log)
                {
                    continue;
                }
                Debug.Log($"当前日志等级:{log[i].LogType}日志信息:{log[i].LogMessage}日志堆栈信息:{log[i].StackTrack}");
            }
        }

        /// <summary>
        /// 初始化日志上报
        /// </summary>
        private static void InitLogReporting( )
        {
            string date = DateTime.Now.ToString( ).Split(' ')[0].Replace('/' , '-');
            //目录路径
            string directoryPath = Application.persistentDataPath + "/" + m_LogFile + "/" + date;
            //时间
            string time = DateTime.Now.ToString( ).Split(' ')[1].Replace(':' , '-');
            LogFilePath = directoryPath + "/" + time + ".log";
            if(!Directory.Exists(directoryPath))
            {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
                DirectoryInfo di = Directory.CreateDirectory(directoryPath);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
#elif UNITY_ANDROID

#endif
            }
            FileInfo fileInfo = new FileInfo(LogFilePath);
            if(!fileInfo.Exists)
            {
                fileInfo.CreateText( );
            }

            Debug.Log(LogFilePath);
            //GameCollectionEntry.Debugger.
        }
    }
}
