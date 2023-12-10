using GameFramework;
using System;
using System.Linq;
using System.Reflection;
using UGHGame.HotfixLogic;
using UnityEditor;
using UnityEngine;

namespace UGHGame.GameEditor
{
    /// <summary>
    /// appHotfixCinfig inspector界面重绘
    /// </summary>
    [CustomEditor(typeof(AppHotfixConfig))]
    internal class AppHotfixConfigInspector:Editor
    {
        /// <summary>
        /// app热更配置
        /// </summary>
        private AppHotfixConfig m_AppHotfixConfig;

        /// <summary>
        /// 未选中样式
        /// </summary>
        private GUIStyle m_NormalStyle;

        /// <summary>
        /// 选中样式
        /// </summary>
        private GUIStyle m_SelectedStyle;

        /// <summary>
        /// Aot文件路径
        /// </summary>
        private readonly string m_AotFilePath = "/HotfixAssets/AotMetadata";
        /// <summary>
        /// Aot元数据文件列表
        /// </summary>
        private SelectAssetsData[] m_AotFileList;
        private bool m_DrawAotFile;
        private Vector2 m_AotFileScrollPos;

        /// <summary>
        /// 游戏流程
        /// </summary>
        private SelectAssetsData[] m_Procedures;
        private bool m_DrawProcedures;
        private Vector2 m_ProceduresScrollPos;

        private bool m_DrawDataTables;
        private Vector2 m_DataTableScrollpos;

        private void OnEnable( )
        {
            m_AppHotfixConfig = target as AppHotfixConfig;
            InitInfo( );
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void InitInfo( )
        {
            m_NormalStyle = new GUIStyle( );
            m_NormalStyle.normal.textColor = Color.white;
            m_SelectedStyle = new GUIStyle( );
            m_SelectedStyle.normal.textColor = Color.green;
            LoadAppHotfixConfigData(m_AppHotfixConfig);
        }
        public override void OnInspectorGUI( )
        {
            EditorGUILayout.BeginVertical("box");
            {
                DrawAotFileList( );
            }
            EditorGUILayout.EndVertical( );

            EditorGUILayout.Space(2);

            EditorGUILayout.BeginVertical("box");
            {
                DrawDataTables( );
            }
            EditorGUILayout.EndVertical( );

            EditorGUILayout.Space(2);

            //绘制流程界面
            EditorGUILayout.BeginVertical("box");
            {
                DrawProcedures( );
            }
            EditorGUILayout.EndVertical( );
        }
        /// <summary>
        /// 绘制AOT文件列表
        /// </summary>
        private void DrawAotFileList( )
        {
            m_DrawAotFile = EditorGUILayout.Foldout(m_DrawAotFile , "AOT数据:");
            if(m_DrawAotFile)
            {
                m_AotFileScrollPos = GUILayout.BeginScrollView(m_AotFileScrollPos);
                {
                    if(GUILayout.Button("加载AOT文件"))
                    {
                        LoadAotFile(m_AppHotfixConfig);
                    }
                    EditorGUI.BeginChangeCheck( );
                    {
                        foreach(var item in m_AotFileList)
                        {
                            item.IsEnable = EditorGUILayout.ToggleLeft(item.AssetsName , item.IsEnable , item.IsEnable ? m_SelectedStyle : m_NormalStyle);
                        }
                    }
                    if(EditorGUI.EndChangeCheck( ))
                    {
                        SaveConfig(m_AppHotfixConfig);
                    }
                }
                GUILayout.EndScrollView( );
            }
        }

        /// <summary>
        /// 绘制Procedures界面
        /// </summary>
        private void DrawProcedures( )
        {
            m_DrawProcedures = EditorGUILayout.Foldout(m_DrawProcedures , "游戏流程:");
            if(m_DrawProcedures)
            {
                m_ProceduresScrollPos = GUILayout.BeginScrollView(m_ProceduresScrollPos);
                {
                    EditorGUI.BeginChangeCheck( );
                    foreach(var item in m_Procedures)
                    {
                        item.IsEnable = EditorGUILayout.ToggleLeft(item.AssetsName , item.IsEnable , item.IsEnable ? m_SelectedStyle : m_NormalStyle);
                    }
                    if(EditorGUI.EndChangeCheck( ))
                    {
                        SaveConfig(m_AppHotfixConfig);
                    }
                }
                GUILayout.EndScrollView( );
            }
        }

        /// <summary>
        /// 绘制数据表界面
        /// </summary>
        private void DrawDataTables( )
        {
            m_DrawDataTables = EditorGUILayout.Foldout(m_DrawDataTables , "数据表:");
            if(m_DrawDataTables)
            {
                m_DataTableScrollpos = GUILayout.BeginScrollView(m_DataTableScrollpos);
                {
                    EditorGUI.BeginChangeCheck( );
                    {

                    }
                    if(EditorGUI.EndChangeCheck( ))
                    {
                        SaveConfig(m_AppHotfixConfig);
                    }

                }
                GUILayout.EndScrollView( );
            }
        }
        /// <summary>
        /// 加载游戏流程
        /// </summary>
        /// <param name="config"></param>
        private void LoadAppHotfixConfigData(AppHotfixConfig config)
        {
            #region AotFile

            LoadAotFile(config);
            #endregion

            #region Procedures
            m_Procedures ??= new SelectAssetsData[0];
            ArrayUtility.Clear(ref m_Procedures);
            var hotfixDlls = Utility.Assembly.GetAssemblies( ).Where(dll => HybridCLR.Editor.SettingsUtil.HotUpdateAssemblyNamesIncludePreserved.Contains(dll.GetName( ).Name)).ToArray( );

            foreach(var item in hotfixDlls)
            {
                var proceClassArr = item.GetTypes( ).Where(tp => tp.BaseType == typeof(ProcedureBase)).ToArray( );
                foreach(var proceClass in proceClassArr)
                {
                    var proceName = proceClass.FullName;
                    ArrayUtility.Add(ref m_Procedures , new SelectAssetsData(proceName , config.HotfixProcedure.Contains(proceName)));
                }
            }
            #endregion
        }

        /// <summary>
        /// 加载aot数据文件
        /// </summary>
        /// <param name="config"></param>
        private void LoadAotFile(AppHotfixConfig config)
        {
            m_AotFileList ??= new SelectAssetsData[0];
            ArrayUtility.Clear(ref m_AotFileList);
            string path = Application.dataPath + m_AotFilePath;
            string[] assetsName = EidtorPathUtility.GetAllFile(path);
            for(int i = 0; i < assetsName.Length; i++)
            {
                assetsName[i] = assetsName[i].Split(".dll")[0];
            }
            foreach(var item in assetsName)
            {

                ArrayUtility.Add(ref m_AotFileList , new SelectAssetsData(item , config.AotFileList.Contains(item)));
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config"></param>
        private void SaveConfig(AppHotfixConfig config)
        {
            #region AOTFile
            string[] aot = new string[0];
            foreach(var item in m_AotFileList)
            {
                if(item.IsEnable)
                {
                    ArrayUtility.Add(ref aot , item.AssetsName);
                }
            }
            config.GetType( ).GetField("m_AotFileList" , BindingFlags.Instance | BindingFlags.NonPublic).SetValue(config , aot);
            #endregion

            #region DataTables

            #endregion

            #region Procedures

            string[] procedures = new string[0];
            foreach(var item in m_Procedures)
            {
                if(item.IsEnable)
                {
                    ArrayUtility.Add(ref procedures , item.AssetsName);
                }
            }
            config.GetType( ).GetField("m_HotfixProcedures" , BindingFlags.Instance | BindingFlags.NonPublic).SetValue(config , procedures);
            #endregion


            EditorUtility.SetDirty(config);
        }
    }
}
