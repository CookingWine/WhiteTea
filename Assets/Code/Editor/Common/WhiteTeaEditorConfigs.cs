using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 编辑器配置
    /// </summary>
    internal sealed partial class WhiteTeaEditorConfigs
    {
        #region 宏定义
        /// <summary>
        /// beta版本
        /// </summary>
        public const string WHITEAGAME_BETA = "WHITEAGAME_BETA";

        #endregion

        #region Path

        /// <summary>
        /// 数据表的路径
        /// </summary>
        public const string DataTablePath = "Assets/GameConfigAssets/DataTables";
        /// <summary>
        /// 数据表生成的bytes文件存放位置
        /// </summary>
        public const string DataGeneratorPath = "Assets/HotfixAssets/DataTables";
        /// <summary>
        /// 生成配置表代码的模板文件
        /// </summary>
        public const string CSharpCodeTemplateFileName = "Assets/GameConfigAssets/GameFramework/DataTableCodeTemplate.txt";
        /// <summary>
        /// 代码生成路径
        /// </summary>
        public const string CSharpCodePath = "Assets/Code/HotfixLogic/Definition/DataTable";
        /// <summary>
        /// 语言配置表位置
        /// </summary>
        public const string LanguageBuiltinPath = "Assets/Resources/Language";
        /// <summary>
        /// 内置语言表的生成位置【该表通过Resource.Load进行加载】
        /// </summary>
        public const string LanguageBuiltinGeneratorPath = "Language";
        /// <summary>
        /// 热更语言表生成位置
        /// </summary>
        public const string LanguageHotfixGeneratorPath = "Assets/HotfixAssets/Localization/{0}/";

        /// <summary>
        /// Aot数据文件
        /// </summary>
        public const string AotFilePath = "/HotfixAssets/AotMetadata";

        #endregion
        /// <summary>
        /// 获取路径下的全部文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetAllFile(string path)
        {
            List<string> list = new List<string>( );
            if(Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles("*");
                for(int i = 0; i < files.Length; i++)
                {
                    if(files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    list.Add(files[i].Name);
                }
            }
            else
            {
                Debug.LogError($"不存在:【{path}】路径");
            }
            return list.ToArray( );
        }
    }
}
