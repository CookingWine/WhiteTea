//using HybridCLR.Editor.Settings;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UGHGame.BuiltinRuntime;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//namespace UGHGame.GameEditor
//{
//    /// <summary>
//    /// AppBuiltinConfig inspector界面重绘
//    /// </summary>
//    [CustomEditor(typeof(AppBuiltinConfig))]
//    public class AppBuiltinConfigInspector:Editor
//    {
//        /// <summary>
//        /// 配置
//        /// </summary>
//        private AppBuiltinConfig m_AppConfig;

//        /// <summary>
//        /// hybridCLR配置
//        /// </summary>
//        private HybridCLRSettings m_HybridCLR;

//        /// <summary>
//        /// 热更新dlls文件列表
//        /// </summary>
//        private readonly List<string> m_HotfixFileName = new List<string>( );

//        private bool m_DrawHybridCLRSettings = false;
//        private Vector2 m_HybridCLRSettingsScrollPos;

//        private bool m_DrawAppConfig = true;
//        private Vector2 m_AppConfigScrollPos;
//        private bool m_UpdateAppConfig = true;
//        private void OnEnable( )
//        {
//            m_AppConfig = (AppBuiltinConfig)target;
//            m_HybridCLR = HybridCLRSettings.Instance;
//            LoadHybridHotAssembliesConfig( );
//        }

//        public override void OnInspectorGUI( )
//        {
//            //hybrildCLR设置
//            DrawHybridCLRConfig( );
//            EditorGUILayout.Space(5);

//            //app配置
//            DrawAppConfig( );
//            EditorGUILayout.Space(5);

//            EditorGUILayout.BeginHorizontal( );
//            {
//                EditorGUI.BeginDisabledGroup(true);
//                if(GUILayout.Button("生成APP配置文件【该文件是防止ScriptableObject找不到】"))
//                {

//                }
//                EditorGUI.EndDisabledGroup( );
//            }
//            EditorGUILayout.EndHorizontal( );
//        }

//        /// <summary>
//        /// 加载hybrid热更dll配置
//        /// </summary>
//        private void LoadHybridHotAssembliesConfig( )
//        {
//            m_HotfixFileName.Clear( );
//            //获取hybrid得配置
//            List<string> duplicateFiles = new List<string>( );
//            AssemblyDefinitionAsset[] hotUpdateAssemblyDefinitions = HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions;
//            if(hotUpdateAssemblyDefinitions != null)
//            {
//                for(int i = 0; i < hotUpdateAssemblyDefinitions.Length; i++)
//                {
//                    m_HotfixFileName.Add(hotUpdateAssemblyDefinitions[i].name);
//                }
//            }
//            string[] hotUpdateAssemblies = HybridCLRSettings.Instance.hotUpdateAssemblies;
//            if(hotUpdateAssemblies != null)
//            {
//                for(int i = 0; i < hotUpdateAssemblies.Length; i++)
//                {
//                    if(m_HotfixFileName.Contains(hotUpdateAssemblies[i]))
//                    {
//                        duplicateFiles.Add(hotUpdateAssemblies[i]);
//                    }
//                    else
//                    {
//                        m_HotfixFileName.Add(hotUpdateAssemblies[i]);
//                    }
//                }
//            }
//            if(duplicateFiles.Count > 0)
//            {
//                for(int i = 0; i < duplicateFiles.Count; i++)
//                {
//                    Debug.LogError($"重复文件:{duplicateFiles[i]}");
//                }
//            }
//        }

//        /// <summary>
//        /// 绘制hybrid配置
//        /// </summary>
//        private void DrawHybridCLRConfig( )
//        {
//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawHybridCLRSettings = EditorGUILayout.Foldout(m_DrawHybridCLRSettings , "HybridCLR设置");
//                if(m_DrawHybridCLRSettings)
//                {
//                    m_HybridCLRSettingsScrollPos = EditorGUILayout.BeginScrollView(m_HybridCLRSettingsScrollPos);
//                    {
//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            EditorGUILayout.LabelField("开启HybridCLR插件:");
//                            m_HybridCLR.enable = EditorGUILayout.Toggle(m_HybridCLR.enable);
//                        }
//                        EditorGUILayout.EndHorizontal( );

//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            EditorGUILayout.LabelField("使用全局安装的il2cpp:");
//                            m_HybridCLR.useGlobalIl2cpp = EditorGUILayout.Toggle(m_HybridCLR.useGlobalIl2cpp);
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                        EditorGUILayout.Space(2);

//                        EditorGUILayout.BeginVertical( );
//                        {
//                            EditorGUILayout.LabelField("热更新dlls【hotUpdateAssemblies】");
//                            for(int i = 0; i < m_HybridCLR.hotUpdateAssemblies.Length; i++)
//                            {
//                                EditorGUILayout.BeginHorizontal( );
//                                {
//                                    EditorGUILayout.LabelField(m_HybridCLR.hotUpdateAssemblies[i]);
//                                    if(GUILayout.Button("移除"))
//                                    {
//                                        lock(m_HybridCLR.hotUpdateAssemblies)
//                                        {
//                                            List<string> hot = m_HybridCLR.hotUpdateAssemblies.ToList( );
//                                            hot.RemoveAt(i);
//                                            Array.Clear(m_HybridCLR.hotUpdateAssemblies , 0 , m_HybridCLR.hotUpdateAssemblies.Length);
//                                            m_HybridCLR.hotUpdateAssemblies = hot.ToArray( );
//                                        }
//                                    }
//                                }
//                                EditorGUILayout.EndHorizontal( );
//                            }
//                        }
//                        EditorGUILayout.EndVertical( );

//                        EditorGUILayout.Space(2);

//                        EditorGUILayout.BeginVertical( );
//                        {
//                            EditorGUILayout.LabelField("补充元数据AOT dlls【patchAOTAssemblies】");
//                            EditorGUI.BeginDisabledGroup(true);
//                            for(int i = 0; i < m_HybridCLR.patchAOTAssemblies.Length; i++)
//                            {
//                                EditorGUILayout.LabelField(m_HybridCLR.patchAOTAssemblies[i]);
//                            }
//                            EditorGUI.EndDisabledGroup( );
//                        }
//                        EditorGUILayout.EndVertical( );

//                        EditorGUILayout.BeginHorizontal( );
//                        {
//                            if(GUILayout.Button("生成hotfix文件"))
//                            {
//                                HybridCLRExtend.CopyHotUpdateAssemblies( );
//                            }
//                            if(GUILayout.Button("生成aot文件"))
//                            {
//                                HybridCLRExtend.CopyAOTAssemblies( );
//                            }
//                        }
//                        EditorGUILayout.EndHorizontal( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//        }

//        /// <summary>
//        /// 绘制app配置
//        /// </summary>
//        private void DrawAppConfig( )
//        {
//            //APP配置
//            EditorGUILayout.BeginVertical("box");
//            {
//                m_DrawAppConfig = EditorGUILayout.Foldout(m_DrawAppConfig , "App配置");
//                if(m_DrawAppConfig)
//                {
//                    m_AppConfigScrollPos = EditorGUILayout.BeginScrollView(m_AppConfigScrollPos);
//                    {
//                        EditorGUILayout.BeginVertical( );
//                        {
//                            if(GUILayout.Button(m_UpdateAppConfig ? "启用修改配置" : "禁用修改配置"))
//                            {
//                                m_UpdateAppConfig = !m_UpdateAppConfig;
//                            }
//                            EditorGUI.BeginDisabledGroup(m_UpdateAppConfig);
//                            {
//                                EditorGUILayout.BeginVertical( );
//                                {
//                                    EditorGUILayout.BeginHorizontal( );
//                                    {
//                                        EditorGUILayout.LabelField("热更类:");
//                                        m_AppConfig.HotfixEntryClass = EditorGUILayout.TextField(m_AppConfig.HotfixEntryClass);
//                                    }
//                                    EditorGUILayout.EndHorizontal( );
//                                    EditorGUILayout.BeginHorizontal( );
//                                    {
//                                        EditorGUILayout.LabelField("热更Start方法:");
//                                        m_AppConfig.HotfixStartFuntion = EditorGUILayout.TextField(m_AppConfig.HotfixStartFuntion);
//                                    }
//                                    EditorGUILayout.EndHorizontal( );
//                                    EditorGUILayout.BeginHorizontal( );
//                                    {
//                                        EditorGUILayout.LabelField("热更Update方法:");
//                                        m_AppConfig.HotfixUpdate = EditorGUILayout.TextField(m_AppConfig.HotfixUpdate);
//                                    }
//                                    EditorGUILayout.EndHorizontal( );
//                                    EditorGUILayout.BeginHorizontal( );
//                                    {
//                                        EditorGUILayout.LabelField("热更Shutdown方法:");
//                                        m_AppConfig.HotfixShutdown = EditorGUILayout.TextField(m_AppConfig.HotfixShutdown);
//                                    }
//                                    EditorGUILayout.EndHorizontal( );
//                                }
//                                EditorGUILayout.EndVertical( );
//                            }
//                            EditorGUI.EndDisabledGroup( );
//                        }
//                        EditorGUILayout.EndVertical( );
//                    }
//                    EditorGUILayout.EndScrollView( );
//                }
//            }
//            EditorGUILayout.EndVertical( );
//        }
//    }
//}
