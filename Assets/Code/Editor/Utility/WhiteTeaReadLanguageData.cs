using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 本地化配置器
    /// </summary>
    internal partial class WhiteTeaLocalizationConfigs:EditorWindow
    {
        private static WhiteTeaLocalizationConfigs m_Instance;
        private WhiteTeaLocalizationConfigs( )
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

        private LocalizationType m_CurrentLoadType;

        private static readonly List<string> m_Language = new List<string>( );
        private static readonly Dictionary<string , List<string>> m_LoacalizationLanguageConten = new Dictionary<string , List<string>>( );
        /// <summary>
        /// 加载本地语言配置
        /// </summary>
        public static void LoadLocalizationLanguage( )
        {
            if(m_Instance == null)
            {
                m_Instance = (WhiteTeaLocalizationConfigs)GetWindow(typeof(WhiteTeaLocalizationConfigs) , true , "本地化配置器" , true);
                m_Instance.minSize = m_WindowSize;
                LoadLocalizationFile( );
            }
            m_Instance.Show( );
        }

        /// <summary>
        /// 绘制工具栏
        /// </summary>
        private void DrawLocalizationToolbar( )
        {
            DrawSearchBox( );
            

            if(GUILayout.Button("生成数据文件"))
            {

            }
            if(GUILayout.Button("重新加载数据文件"))
            {
                LoadLocalizationFile( );
            }
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
                    EditorGUILayout.LabelField("编号" , GUILayout.Width(60f));
                    EditorGUILayout.LabelField("资源类型" , GUILayout.Width(100f));
                    EditorGUILayout.LabelField("语言Key值" , GUILayout.Width(300f));
                    DrawLanguageInterfaceTitle( );
                }
                EditorGUILayout.EndHorizontal( );

                m_DrawBuilitionLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(m_DrawBuilitionLanguage , "内置语言配置");
                {
                    if(m_DrawBuilitionLanguage)
                    {
                        EditorGUILayout.BeginVertical("box" , GUILayout.MaxHeight(m_TitleMaxHeight));
                        {
                            DrawLanguageBuiltinContent( );
                        }
                        EditorGUILayout.EndVertical( );
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
            m_Language.Clear( );
            m_LoacalizationLanguageConten.Clear( );
            TextAsset[] textAsset = Resources.LoadAll<TextAsset>("Language");
            for(int i = 0; i < textAsset.Length; i++)
            {
                ParseData(textAsset[i].text);
            }

        }
        private static bool ParseData(string dictionaryString)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument( );
                xmlDocument.LoadXml(dictionaryString);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("Dictionaries");
                XmlNodeList xmlNodeDictionaryList = xmlRoot.ChildNodes;
                for(int i = 0; i < xmlNodeDictionaryList.Count; i++)
                {
                    XmlNode xmlNodeDictionary = xmlNodeDictionaryList.Item(i);
                    if(xmlNodeDictionary.Name != "Dictionary")
                    {
                        continue;
                    }
                    string language = xmlNodeDictionary.Attributes.GetNamedItem("Language").Value;
                    m_Language.Add(language);
                    XmlNodeList xmlNodeStringList = xmlNodeDictionary.ChildNodes;
                    for(int j = 0; j < xmlNodeStringList.Count; j++)
                    {
                        XmlNode xmlNodeString = xmlNodeStringList.Item(j);
                        if(xmlNodeString.Name != "String")
                        {
                            continue;
                        }

                        string key = xmlNodeString.Attributes.GetNamedItem("Key").Value;
                        string value = xmlNodeString.Attributes.GetNamedItem("Value").Value;
                        if(m_LoacalizationLanguageConten.ContainsKey(key))
                        {
                            m_LoacalizationLanguageConten.TryGetValue(key , out List<string> values);
                            values.Add(value);
                        }
                        else
                        {
                            List<string> temp = new List<string>( );
                            temp.Add(value);
                            m_LoacalizationLanguageConten.Add(key , temp);
                        }
                    }
                    string languages = GetLanguage(language);
                }
                return true;
            }
            catch(Exception exception)
            {
                Log.Warning("Can not parse dictionary data with exception '{0}'." , exception.ToString( ));
                return false;
            }
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        private void DrawLanguageInterfaceTitle( )
        {
            for(int i = 0; i < m_Language.Count; i++)
            {
                EditorGUILayout.LabelField(GetLanguage(m_Language[i]) , GUILayout.Width(200));
            }
        }
        /// <summary>
        /// 绘制内置语言配置界面
        /// </summary>
        private void DrawLanguageBuiltinContent( )
        {
            int index = 0;
            foreach(var item in m_LoacalizationLanguageConten)
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    EditorGUILayout.LabelField($"{index}" , GUILayout.Width(60f));
                    EditorGUILayout.LabelField("Language" , GUILayout.Width(100f));
                    EditorGUILayout.TextField($"{item.Key}" , GUILayout.Width(300f));
                    for(int i = 0; i < item.Value.Count; i++)
                    {
                        item.Value[i] = EditorGUILayout.TextField(item.Value[i] , GUILayout.Width(200f));
                    }
                    index++;
                }
                EditorGUILayout.EndHorizontal( );
            }
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
        private static string GetLanguage(string language)
        {
            switch(language)
            {
                case "English":
                    return "英语";
                case "ChineseSimplified":
                    return "简体中文";
                case "ChineseTraditional":
                    return "繁体中文";
                default:
                    return "简体中文";
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
        private void OnGUI( )
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width) , GUILayout.Height(position.height));
            {
                GUILayout.Space(2f);
                EditorGUILayout.BeginVertical(GUILayout.Width(150f));
                {
                    DrawLocalizationToolbar( );
                }
                EditorGUILayout.EndVertical( );
                GUILayout.Space(20f);
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 170f));
                {
                    DrawLocalizationLanguageInterface( );
                }
                EditorGUILayout.EndVertical( );
            }

            EditorGUILayout.EndHorizontal( );


            //EditorGUILayout.BeginHorizontal( );
            //{

            //    if(GUILayout.Button("添加"))
            //    {

            //    }
            //    if(GUILayout.Button("重新加载"))
            //    {
            //        LoadLocalizationFile( );
            //    }
            //}
            //EditorGUILayout.EndHorizontal( );


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
    }
}
