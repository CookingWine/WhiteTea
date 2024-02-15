using UnityEditor;
using UnityEngine;
using WhiteTea.BuiltinRuntime;
using WhiteTea.GameEditor.DataTableTools;
using WhiteTea.HotfixLogic;
namespace WhiteTea.GameEditor
{
    internal sealed partial class WhiteTeaEditorConfigs
    {
        #region Game Generator

        /// <summary>
        /// 生成app配置
        /// </summary>
        [MenuItem("白茶游戏配置/应用设置/生成App配置" , false , 11)]
        private static void GeneratorAppBuiltinSettings( )
        {
            if(AppBuiltinSettings.Instance == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AppBuiltinSettings>( ) , $"Assets/Resources/{BuiltinRuntimeUtility.AppBuiltinSettingsName}.asset");
            }
            else
            {
                EditorUtility.DisplayDialog("警告！" , "已经生成过APP配置了，请勿再次生成！" , "确定");
            }
        }

        /// <summary>
        /// 本地化语言编辑器
        /// </summary>
        [MenuItem("白茶游戏配置/应用设置/本地语言编辑器" , false , 13)]
        private static void LocalizationLanguageSetting( )
        {
            WhiteTeaLocalizationConfigs.LoadLocalizationLanguage( );
        }
        /// <summary>
        /// 游戏配置器
        /// </summary>
        [MenuItem("白茶游戏配置/游戏配置" , false , 1)]
        private static void WhiteTeaGameConfigs( )
        {
            GameConfigs.WhiteTeaGameConfigs.Instance.LoadWhiteTeaGameConfigsWindow( );
        }

        /// <summary>
        /// 生成数据表
        /// </summary>
        [MenuItem("白茶游戏配置/生成配置/生成数据表" , false , 121)]
        private static void GeneratorDataTables( )
        {
            DataTableGenerator.GeneratorDataTables( );
        }
        [MenuItem("白茶游戏配置/生成配置/实体与UI代码生成")]
        private static void OpenCodeGeneratorWindow( )
        {
            WhiteTeaEntityAndUIFormGenerator.OpenCodeGeneratorWindow( );
        }
        /// <summary>
        /// 生成热更dll
        /// </summary>
        [MenuItem("白茶游戏配置/生成配置/生成Hotfix配置" , false , 123)]
        private static void GeneratorHotfixDLL( )
        {
            WhiteTeaHybridCLRConfigs.CopyHotUpdateAssemblies( );
        }

        /// <summary>
        /// 补充aot数据
        /// </summary>
        [MenuItem("白茶游戏配置/生成配置/生成AOT数据" , false , 124)]
        private static void GeneratorAotData( )
        {
            WhiteTeaHybridCLRConfigs.CopyAOTAssemblies( );
        }
        /// <summary>
        /// 生成所有的数据
        /// </summary>
        [MenuItem("白茶游戏配置/生成配置/生成所有游戏配置" , false , 100)]
        private static void GeneratorAllGameFile( )
        {
            GeneratorDataTables( );
            WhiteTeaHybridCLRConfigs.BuildHotfixDll( );
        }

        #endregion

        [MenuItem("白茶游戏配置/切换场景/更新场景列表" , false , 140)]
        private static void UpdateSceneList( )
        {
            WhiteTeaScenesMenuBuild.UpdateList( );
        }

        #region Game Macro
#if WHITEAGAME_BETA
        /// <summary>
        /// 切换为正式版本
        /// </summary>
        [MenuItem("白茶游戏配置/宏配置/切换到正式版" , false , 52)]
        private static void BetaPackageSetting( )
        {
            WhiteTeaGameUtility.SetAppCompanyAndProductName("WhiteTea");
            WhiteTeaGameUtility.RemoveScriptingDefineSymbolsForGroup(WHITEAGAME_BETA);
        }
#else
        /// <summary>
        /// 切换Beta版本
        /// </summary>
        [MenuItem("白茶游戏配置/宏配置/切换到测试版" , false , 52)]
        private static void ReleasePackageSetting( )
        {
            WhiteTeaGameUtility.SetAppCompanyAndProductName("WhiteTea_Beta");
            WhiteTeaGameUtility.AddScriptingDefineSymbolsForGroup(WHITEAGAME_BETA);

        }
#endif

        #endregion

        #region Game Bulid


        #endregion
    }
}
