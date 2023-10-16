//using System.Collections.Generic;
//using System.IO;
//using UGHGame.HotfixLogic;
//using UnityEditor;
//using UnityEngine;

//namespace UGHGame.GameEditor
//{

//    /// <summary>
//    /// inspector界面重绘
//    /// </summary>
//    [CustomEditor(typeof(AppHotfixConfig))]
//    internal class AppHotfixConfigInspector:Editor
//    {
//        /// <summary>
//        /// 热更配置
//        /// </summary>
//        private AppHotfixConfig m_AppHotfixConfig;
//        /// <summary>
//        /// 未选中样式
//        /// </summary>
//        private GUIStyle m_NormalStyle;
//        /// <summary>
//        /// 选中样式
//        /// </summary>
//        private GUIStyle m_SelectedStyle;


//        private bool m_DrawDataTable;
//        private Vector2 m_DataTableScrollPos;
//        private bool m_DrawConfigTable;
//        private Vector2 m_ConfigTableScrollPos;
//        private bool m_DrawProcedures;
//        private Vector2 m_ProceduresScrollPos;
//        private bool m_DrawMetadataAot;
//        private Vector2 m_MetadataAotScrollPos;
//        private bool m_DrawHotfixFileList;
//        private Vector2 m_HotfixFileListScrollPos;
//        private void OnEnable( )
//        {
//            m_AppHotfixConfig = (AppHotfixConfig)target;
//            m_NormalStyle = new GUIStyle( );
//            m_NormalStyle.normal.textColor = Color.white;
//            m_SelectedStyle = new GUIStyle( );
//            m_SelectedStyle.normal.textColor = Color.green;
//            ReloadScrollView(m_AppHotfixConfig);
//        }

//        public override void OnInspectorGUI( )
//        {
//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawDataTable = EditorGUILayout.Foldout(m_DrawDataTable , "数据表:");
//                if(m_DrawDataTable)
//                {
//                    m_DataTableScrollPos = EditorGUILayout.BeginScrollView(m_DataTableScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {

//                        }
//                        EditorGUILayout.EndVertical( );
//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            if(GUILayout.Button("All"))
//                            {

//                            }
//                            if(GUILayout.Button("None"))
//                            {

//                            }
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//            EditorGUILayout.Space(2);

//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawConfigTable = EditorGUILayout.Foldout(m_DrawConfigTable , "配置表:");
//                if(m_DrawConfigTable)
//                {
//                    m_ConfigTableScrollPos = EditorGUILayout.BeginScrollView(m_ConfigTableScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {

//                        }
//                        EditorGUILayout.EndVertical( );
//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            if(GUILayout.Button("All"))
//                            {

//                            }
//                            if(GUILayout.Button("None"))
//                            {

//                            }
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//            EditorGUILayout.Space(2);

//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawProcedures = EditorGUILayout.Foldout(m_DrawProcedures , "流程:");
//                if(m_DrawProcedures)
//                {
//                    m_ProceduresScrollPos = EditorGUILayout.BeginScrollView(m_ProceduresScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {

//                        }
//                        EditorGUILayout.EndVertical( );
//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            if(GUILayout.Button("All"))
//                            {

//                            }
//                            if(GUILayout.Button("None"))
//                            {

//                            }
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//            EditorGUILayout.Space(2);

//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawMetadataAot = EditorGUILayout.Foldout(m_DrawMetadataAot , "AotOrMetadata");
//                if(m_DrawMetadataAot)
//                {
//                    m_MetadataAotScrollPos = EditorGUILayout.BeginScrollView(m_MetadataAotScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {

//                        }
//                        EditorGUILayout.EndVertical( );
//                        EditorGUILayout.BeginVertical( );
//                        {
//                            EditorGUILayout.BeginVertical( );
//                            {
//                                for(int i = 0; i < m_AppHotfixConfig.MetadataAotData.Length; i++)
//                                {
//                                    EditorGUILayout.LabelField(m_AppHotfixConfig.MetadataAotData[i]);
//                                }
//                            }
//                            EditorGUILayout.EndVertical( );
//                            if(GUILayout.Button("加载AotOrMetadata"))
//                            {
//                                List<string> aotPath = new List<string>( );
//                                string path = $"{Application.dataPath}/HotfixAssets/AotMetadata/";
//                                if(Directory.Exists(path))
//                                {
//                                    DirectoryInfo direction = new DirectoryInfo(path);
//                                    FileInfo[] files = direction.GetFiles("*" , SearchOption.AllDirectories);
//                                    for(int i = 0; i < files.Length; i++)
//                                    {
//                                        if(files[i].Name.EndsWith(".meta"))
//                                        {
//                                            continue;
//                                        }
//                                        aotPath.Add($"Assets/HotfixAssets/Aotmetadata/{files[i].Name}.bytes");
//                                    }
//                                    m_AppHotfixConfig.MetadataAotData = aotPath.ToArray( );
//                                }
//                            }
//                        }
//                        EditorGUILayout.EndVertical( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//            EditorGUILayout.Space(2);

//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawHotfixFileList = EditorGUILayout.Foldout(m_DrawHotfixFileList , "热补丁文件列表:");
//                if(m_DrawHotfixFileList)
//                {
//                    m_HotfixFileListScrollPos = EditorGUILayout.BeginScrollView(m_HotfixFileListScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {

//                        }
//                        EditorGUILayout.EndVertical( );
//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            if(GUILayout.Button("重新生成热补丁文件"))
//                            {

//                            }
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//            EditorGUILayout.Space(2);

//            EditorGUILayout.BeginHorizontal( );
//            {
//                if(GUILayout.Button("RefreshAll"))
//                {

//                }
//                if(GUILayout.Button("SaveAll"))
//                {

//                }
//            }
//            EditorGUILayout.EndHorizontal( );
//        }

//        /// <summary>
//        /// 刷新视图
//        /// </summary>
//        /// <param name="data"></param>
//        private void ReloadScrollView(AppHotfixConfig data)
//        {
//            if(data == null)
//            {
//                return;
//            }
//            ReloadProcedures(data);
//        }

//        /// <summary>
//        /// 刷新流程
//        /// </summary>
//        /// <param name="data"></param>
//        private void ReloadProcedures(AppHotfixConfig data)
//        {
//            if(data == null)
//            {
//                return;
//            }
//        }
//    }
//}
