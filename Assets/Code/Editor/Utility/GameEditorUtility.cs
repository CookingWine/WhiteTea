using System.Collections.Generic;
using System.IO;
using UGHGame.BuiltinRuntime;
using UnityEngine;

namespace UGHGame.GameEditor
{
    /// <summary>
    /// 选择资源
    /// </summary>
    internal class SelectAssetsData
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable;

        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetsName { get; private set; }

        public SelectAssetsData(string assetsName , bool isEnable)
        {
            AssetsName = assetsName;
            IsEnable = isEnable;
        }
    }

    /// <summary>
    /// Editor路径工具
    /// </summary>
    internal class EidtorPathUtility
    {
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

    /// <summary>
    /// 编辑器内的配置项
    /// </summary>
    internal class ConstEditor
    {
        /// <summary>
        /// 新建脚本时自动修改脚本编码方式为utf-8以支持中文
        /// </summary>
        public const bool AutoScriptUTF8 = true;

        /// <summary>
        /// 内置运行的程序集
        /// </summary>
        public const string BuiltinRuntimeAssembly = "Assets/Code/BuiltinRuntime/UGHGame.BuiltinRuntime.asmdef";
        /// <summary>
        /// 热更程序集
        /// </summary>
        public const string HotfixLogicAssembly = "Assets/Code/HotfixLogic/UGHGame.HotfixLogic.asmdef";

        /// <summary>
        /// 数据表的目录
        /// </summary>
        public static string DataTableExcelPath
        {
            get
            {
                return AssetUtility.GetCombinePath(Directory.GetParent(Application.dataPath).FullName , "HotfixAssets/DataTables");
            }
        }
        /// <summary>
        /// 本地化语言表
        /// </summary>
        public static string LanguageExcelPath
        {
            get
            {
                return AssetUtility.GetCombinePath(Directory.GetParent(Application.dataPath).FullName , "HotfixAssets/Localization");
            }
        }

        /// <summary>
        /// 数据表的存放位置
        /// </summary>
        public const string DataTablePath = "Assets/HotfixAssets/DataTables";

        /// <summary>
        /// 语言配置表的存放位置
        /// </summary>
        public const string LanguageTablePath = "Assets/HotfixAssets/Localization";

        /// <summary>
        /// 数据表的生成代码位置
        /// </summary>
        public const string DataTableCodePath = "Assets/Code/HotfixLogic/DataTable/CodeGenerate";
    }
}
