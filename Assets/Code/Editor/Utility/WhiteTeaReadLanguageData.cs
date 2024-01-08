using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 本地化语言编辑器
    /// </summary>
    internal class WhiteTeaReadLanguageData:EditorWindow
    {
        private static WhiteTeaReadLanguageData m_Instance;
        private WhiteTeaReadLanguageData( )
        {

        }

        /// <summary>
        /// 窗口最小的大小
        /// </summary>
        private static Vector2 m_WindowSize = new Vector2(800f , 600f);
        /// <summary>
        /// 滚动视图
        /// </summary>
        private Vector2 m_DataSlider;
        /// <summary>
        /// 标题栏最大高度
        /// </summary>
        private const float m_TitleMaxHeight = 30f;
        /// <summary>
        /// 是否绘制热更语言配置
        /// </summary>
        private bool m_DrawHofixLanguage;

        /// <summary>
        /// 是否绘制内置语言配置
        /// </summary>
        private bool m_DrawBuilitionLanguage;

        /// <summary>
        /// 本地语言配置列表
        /// </summary>
        private static readonly Dictionary<string , LocalizationLanguageConfig> m_LocalizationLanguageConfig = new Dictionary<string , LocalizationLanguageConfig>( );

        /// <summary>
        /// 加载本地语言配置
        /// </summary>
        public static void LoadLocalizationLanguage( )
        {
            if(m_Instance == null)
            {
                m_Instance = GetWindow<WhiteTeaReadLanguageData>("本地化语言配置器" , true);
                m_Instance.minSize = m_WindowSize;
                LoadLocalizationFile( );
            }
            m_Instance.Show( );
        }

        /// <summary>
        /// 绘制语言编辑界面
        /// </summary>
        private void DrawLocalizationLanguageInterface( )
        {
            m_DataSlider = EditorGUILayout.BeginScrollView(m_DataSlider);
            {
                EditorGUILayout.BeginHorizontal("box" , GUILayout.MaxHeight(m_TitleMaxHeight));
                {
                    EditorGUILayout.LabelField("编号" , GUILayout.MinWidth(60));
                    EditorGUILayout.LabelField("资源类型" , GUILayout.MinWidth(100));
                    EditorGUILayout.LabelField("语言Key值" , GUILayout.MinWidth(100));
                    DrawLanguageInterfaceTitle( );
                }
                EditorGUILayout.EndHorizontal( );

                m_DrawBuilitionLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(m_DrawBuilitionLanguage , "内置语言配置");
                {
                    if(m_DrawBuilitionLanguage)
                    {
                        DrawLanguageBuiltinContent( );
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup( );

                m_DrawHofixLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(m_DrawHofixLanguage , "热更语言配置");
                {
                    if(m_DrawHofixLanguage)
                    {
                        DrawLanguageHotfixContent( );
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup( );
            }
            EditorGUILayout.EndScrollView( );
        }

        /// <summary>
        /// 加载语言配置文件
        /// </summary>
        private static void LoadLocalizationFile( )
        {
           
        }

      

        /// <summary>
        /// 绘制标题
        /// </summary>
        private void DrawLanguageInterfaceTitle( )
        {
            foreach(var item in m_LocalizationLanguageConfig)
            {
                EditorGUILayout.LabelField(item.Key , GUILayout.MinWidth(100));
            }
        }
        /// <summary>
        /// 绘制内置语言配置界面
        /// </summary>
        private void DrawLanguageBuiltinContent( )
        {

        }
        /// <summary>
        /// 绘制热更语言配置界面
        /// </summary>
        private void DrawLanguageHotfixContent( )
        {

        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private void Save( )
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument( );
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0" , "UTF-8" , null));
                XmlElement xmlRoot = xmlDocument.CreateElement("Dictionaries");
                xmlDocument.AppendChild(xmlRoot);
                XmlElement xmlLanguage = xmlDocument.CreateElement($"Dictionary Language=");


            }
            catch
            {
                if(File.Exists(""))
                {
                    File.Delete("");
                }
            }
        }

        /// <summary>
        /// 获取路径下所以文件的信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static FileInfo[] GetFilesFullName(string path)
        {
            List<FileInfo> info = new List<FileInfo>( );
            if(Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] files = directory.GetFiles("*");
                for(int i = 0; i < files.Length; i++)
                {
                    if(files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    info.Add(files[i]);
                }
            }
            return info.ToArray( );
        }

        private Vector2 m_Scrollpos;
        private void OnGUI( )
        {
            //m_Scrollpos = EditorGUILayout.BeginScrollView(m_Scrollpos);
            //{
            //    EditorGUILayout.BeginHorizontal("box" , GUILayout.MaxHeight(30f));
            //    {
            //        EditorGUILayout.LabelField("索引" , GUILayout.Width(60f));
            //        EditorGUILayout.LabelField(" " , GUILayout.Width(100));
            //        EditorGUILayout.LabelField("语言Key" , GUILayout.Width(100));
            //        for(int i = 0; i < 50; i++)
            //        {
            //            EditorGUILayout.LabelField($"语言类型{i + 1}" , GUILayout.Width(150));
            //        }
            //    }
            //    EditorGUILayout.EndHorizontal( );
            //    m_DrawBuilitionLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(m_DrawBuilitionLanguage , "内置语言配置");
            //    {
            //        if(m_DrawBuilitionLanguage)
            //        {
            //            for(int i = 0; i < 50; i++)
            //            {
            //                EditorGUILayout.BeginHorizontal( );
            //                {
            //                    DrawLanguage(i , 50 , "移动热更内");
            //                }
            //                EditorGUILayout.EndHorizontal( );
            //            }
            //        }
            //    }
            //    EditorGUILayout.EndFoldoutHeaderGroup( );

            //    m_DrawHofixLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(m_DrawHofixLanguage , "热更语言配置");
            //    {
            //        if(m_DrawHofixLanguage)
            //        {
            //            for(int i = 0; i < 50; i++)
            //            {
            //                EditorGUILayout.BeginHorizontal( );
            //                {
            //                    DrawLanguage(i , 50 , "移动非热更中");
            //                }
            //                EditorGUILayout.EndHorizontal( );
            //            }
            //        }
            //    }
            //    EditorGUILayout.EndFoldoutHeaderGroup( );
            //}
            //EditorGUILayout.EndScrollView( );


            EditorGUILayout.BeginHorizontal( );
            {

                if(GUILayout.Button("添加"))
                {
                    if(!m_LocalizationLanguageConfig.ContainsKey("中文"))
                    {
                        m_LocalizationLanguageConfig.Add("中文" , new LocalizationLanguageConfig("中文"));
                    }
                }
                if(GUILayout.Button("保存"))
                {
                    Save( );
                }
            }
            EditorGUILayout.EndHorizontal( );

            DrawLocalizationLanguageInterface( );
        }
        private void DrawLanguage(int index , int counts , string buttonName)
        {
            string[] languageTmp = new string[counts];
            string key = string.Empty;

            EditorGUILayout.BeginHorizontal( );
            {
                EditorGUILayout.LabelField($"{index}" , GUILayout.Width(60f));
                if(GUILayout.Button(buttonName , GUILayout.Width(100)))
                {

                }
                key = EditorGUILayout.TextField(key , GUILayout.Width(100f));
                for(int i = 0; i < languageTmp.Length; i++)
                {
                    languageTmp[i] = EditorGUILayout.TextField(languageTmp[i] , GUILayout.Width(150f));
                }
            }
            EditorGUILayout.EndHorizontal( );
        }

        /// <summary>
        /// 语言配置
        /// </summary>
        private class LocalizationLanguageConfig
        {
            /// <summary>
            /// 语言
            /// </summary>
            public string Language { get; private set; }

            /// <summary>
            /// 文件路径
            /// </summary>
            public string Path;

            /// <summary>
            /// 语言key与value
            /// </summary>
            public Dictionary<string , string> LanguageKeyOrValue { get; private set; }

            public LocalizationLanguageConfig(string language)
            {
                Language = language;
                LanguageKeyOrValue = new Dictionary<string , string>( );
            }
            public LocalizationLanguageConfig(string language , Dictionary<string , string> config)
            {
                Language = language;
                LanguageKeyOrValue = config;
            }

            public string[] GetKey( )
            {
                List<string> key = new List<string>( );
                foreach(var item in LanguageKeyOrValue)
                {
                    key.Add(item.Key);
                }
                return key.ToArray( );
            }
            public string[] GetValue( )
            {
                List<string> values = new List<string>( );
                foreach(var item in LanguageKeyOrValue)
                {
                    values.Add(item.Value);
                }
                return values.ToArray( );
            }
        }


    }
}
