using GameFramework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WhiteTea.HotfixLogic;

namespace WhiteTea.GameEditor
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
        /// aot元数据文件
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

        /// <summary>
        /// 绘制数据表
        /// </summary>
        private List<SelectAssetsData> m_DataTable;
        private bool m_DrawDataTableFile;
        private Vector2 m_DataTableScrollPos;

        private void OnEnable( )
        {
            m_AppHotfixConfig = (AppHotfixConfig)target;
            InitInfo( );
        }
        private void InitInfo( )
        {
            m_NormalStyle = new GUIStyle( );
            m_NormalStyle.normal.textColor = Color.white;
            m_SelectedStyle = new GUIStyle( );
            m_SelectedStyle.normal.textColor = Color.green;
            m_DrawProcedures = true;
            m_DrawAotFile = true;
            m_DrawDataTableFile = true;
            if(m_AppHotfixConfig != null)
            {
                LoadAppHotfixConfigData(m_AppHotfixConfig);
            }
        }

        public override void OnInspectorGUI( )
        {
            //绘制AOT界面
            EditorGUILayout.BeginVertical("box");
            {
                DrawAotFileList( );
            }
            EditorGUILayout.EndVertical( );

            EditorGUILayout.Space(2);

            //绘制流程界面
            EditorGUILayout.BeginVertical("box");
            {
                DrawProcedures( );
            }
            EditorGUILayout.EndVertical( );

            EditorGUILayout.Space(2);
            //绘制数据表
            EditorGUILayout.BeginVertical("box");
            {
                DrawDataTable( );
            }
            EditorGUILayout.EndVertical( );

            EditorGUILayout.Space(2);
            if(GUILayout.Button("save"))
            {
                SaveConfigs(m_AppHotfixConfig);
            }
            serializedObject.ApplyModifiedProperties( );
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
                    EditorGUI.BeginChangeCheck( );
                    {
                        foreach(var item in m_AotFileList)
                        {
                            item.IsEnable = EditorGUILayout.ToggleLeft(item.AssetsName , item.IsEnable , item.IsEnable ? m_SelectedStyle : m_NormalStyle);
                        }
                    }
                    if(EditorGUI.EndChangeCheck( ))
                    {
                        SaveConfigs(m_AppHotfixConfig);
                    }
                    //if(GUILayout.Button("all select"))
                    //{
                    //    foreach(var item in m_AotFileList)
                    //    {
                    //        item.IsEnable = true;
                    //    }
                    //}
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
                        SaveConfigs(m_AppHotfixConfig);
                    }
                    //if(GUILayout.Button("all select"))
                    //{
                    //    foreach(var item in m_Procedures)
                    //    {
                    //        item.IsEnable = true;
                    //    }
                    //}
                }
                GUILayout.EndScrollView( );
            }
        }

        private void DrawDataTable( )
        {
            m_DrawDataTableFile = EditorGUILayout.Foldout(m_DrawDataTableFile , "数据表:");
            if(m_DrawDataTableFile)
            {
                m_DataTableScrollPos = GUILayout.BeginScrollView(m_DataTableScrollPos);
                {
                    EditorGUI.BeginChangeCheck( );
                    foreach(var item in m_DataTable)
                    {
                        item.IsEnable = EditorGUILayout.ToggleLeft(item.AssetsName , item.IsEnable , item.IsEnable ? m_SelectedStyle : m_NormalStyle);
                    }
                    if(EditorGUI.EndChangeCheck( ))
                    {
                        SaveConfigs(m_AppHotfixConfig);
                    }
                }
                GUILayout.EndScrollView( );
            }
        }

        /// <summary>
        /// 加载游戏热更配置数据
        /// </summary>
        /// <param name="config"></param>
        private void LoadAppHotfixConfigData(AppHotfixConfig config)
        {
            LoadAotFile(config);
            LoadHotfixProcedure(config);
            LoadDataTable(config);
        }

        private void SaveConfigs(AppHotfixConfig config)
        {
            #region AOT
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

            #region DataTable
            List<string> dataTemp = new List<string>( );
            foreach(var item in m_DataTable)
            {
                if(item.IsEnable)
                {
                    string temp = Path.GetFileNameWithoutExtension(item.AssetsName);
                    dataTemp.Add(temp);
                }
            }
            config.GetType( ).GetField("m_DataTables" , BindingFlags.Instance | BindingFlags.NonPublic).SetValue(config , dataTemp.ToArray());

            #endregion
            EditorUtility.SetDirty(config);
        }

        /// <summary>
        /// 加载aot数据文件
        /// </summary>
        /// <param name="config"></param>
        private void LoadAotFile(AppHotfixConfig config)
        {
            m_AotFileList ??= new SelectAssetsData[0];
            ArrayUtility.Clear(ref m_AotFileList);
            string path = Application.dataPath + WhiteTeaEditorConfigs.AotFilePath;
            string[] assetsName = WhiteTeaEditorConfigs.GetAllFile(path);
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
        /// 加载热更流程
        /// </summary>
        /// <param name="config"></param>
        private void LoadHotfixProcedure(AppHotfixConfig config)
        {
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
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        private void LoadDataTable(AppHotfixConfig config)
        {
            string dataPath = WhiteTeaEditorConfigs.DataTablePath;
            if(!Directory.Exists(dataPath) || config == null)
            {
                return;
            }
            var data = Directory.GetFiles(dataPath , "*" , SearchOption.AllDirectories);

            data = data.Where(name =>
            {
                var ext = Path.GetExtension(name).ToLower( );
                return ext.CompareTo(".txt") == 0;
            }).ToArray( );
            m_DataTable ??= new List<SelectAssetsData>( );
            m_DataTable.Clear( );
            foreach(var item in data)
            {
                var dataName = Path.GetFileNameWithoutExtension(item);

                var isOn = ArrayUtility.Contains(config.DataTables , dataName);
                m_DataTable.Add(new SelectAssetsData(dataName , isOn));
            }
        }
    }
}
