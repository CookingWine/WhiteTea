using GameFramework.DataTable;
using UnityEditor;
using WhiteTea.GameEditor.DataTableTools;
namespace WhiteTea.GameEditor
{
    internal sealed partial class WhiteTeaEditorConfigs
    {
        #region Game Generator

        /// <summary>
        /// 生成app配置
        /// </summary>
        [MenuItem("White Tea Game/App Setting/Builting Config" , false , 11)]
        private static void GeneratorAppBuiltinSettings( )
        {

        }

        /// <summary>
        /// 生成hotfix配置
        /// </summary>
        [MenuItem("White Tea Game/App Setting/Hotfix Config" , false , 12)]
        private static void GeneratorAppHotfixSettings( )
        {

        }
        /// <summary>
        /// 本地化语言编辑器
        /// </summary>
        [MenuItem("White Tea Game/App Setting/Localization Languages Editor" , false , 13)]
        private static void LocalizationLanguageSetting( )
        {
            WhiteTeaLocalizationConfigs.LoadLocalizationLanguage( );
        }

        /// <summary>
        /// 生成数据表
        /// </summary>
        [MenuItem("White Tea Game/Generator/ Data Table" , false , 121)]
        private static void GeneratorDataTables( )
        {
            DataTableGenerator.GeneratorDataTables( );
        }

        /// <summary>
        /// 生成热更dll
        /// </summary>
        [MenuItem("White Tea Game/Generator/Hotfix Dll" , false , 123)]
        private static void GeneratorHotfixDLL( )
        {

        }

        /// <summary>
        /// 补充aot数据
        /// </summary>
        [MenuItem("White Tea Game/Generator/Aot Data" , false , 124)]
        private static void GeneratorAotData( )
        {

        }
        /// <summary>
        /// 生成所有的数据
        /// </summary>
        [MenuItem("White Tea Game/Generator/Generator All Game Data" , false , 100)]
        private static void GeneratorAllGameFile( )
        {
            GeneratorDataTables( );
            GeneratorHotfixDLL( );
            GeneratorAotData( );
        }

        #endregion

        #region Game Macro
#if WHITEAGAME_BETA
        /// <summary>
        /// 切换为正式版本
        /// </summary>
        [MenuItem("White Tea Game/Game Macro/Switch Release" , false , 52)]
        private static void BetaPackageSetting( )
        {
            WhiteTeaGameUtility.SetAppCompanyAndProductName("WhiteTea");
            WhiteTeaGameUtility.RemoveScriptingDefineSymbolsForGroup(WHITEAGAME_BETA);
        }
#endif
#if !WHITEAGAME_BETA
        /// <summary>
        /// 切换Beta版本
        /// </summary>
        [MenuItem("White Tea Game/Game Macro/Switch Beta" , false , 52)]
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
