using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;
using System.IO;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Settings;
using System.Collections.Generic;
using System;
using WhiteTea.BuiltinRuntime;
using GameFramework;
using System.Linq;
namespace WhiteTea.GameEditor
{
    internal static class WhiteTeaHybridCLRConfigs
    {
        public static void BuildHotfixDll( )
        {
            CompileDllCommand.CompileDll(EditorUserBuildSettings.activeBuildTarget);
            CopyHotUpdateAssembliesToHotfixFile( );
            CopyAotAssemblies( );
            AssetDatabase.Refresh( );
        }

        public static void CopyHotUpdateAssemblies( )
        {
            CompileDllCommand.CompileDll(EditorUserBuildSettings.activeBuildTarget);
            CopyHotUpdateAssembliesToHotfixFile( );
            AssetDatabase.Refresh( );
        }
        public static void CopyAOTAssemblies( )
        {
            CompileDllCommand.CompileDll(EditorUserBuildSettings.activeBuildTarget);
            CopyAotAssemblies( );
            AssetDatabase.Refresh( );
        }

        /// <summary>
        /// copy热更dll文件
        /// </summary>
        private static void CopyHotUpdateAssembliesToHotfixFile( )
        {

            string hotUpdateDll = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
            foreach(var dll in SettingsUtil.HotUpdateAssemblyFilesIncludePreserved)
            {
                string dllPath = $"{hotUpdateDll}/{dll}";
                string dllBytesPath = $"Assets/HotfixAssets/HotfixDLL/{dll}.bytes";
                File.Copy(dllPath , dllBytesPath , true);
                Debug.Log($"Copy <Hotfix> assembly dll: name {dll} {dllPath} -> {dllBytesPath} over!");
            }

        }
        /// <summary>
        /// copy aot文件
        /// </summary>
        private static void CopyAotAssemblies( )
        {
            string aotDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);

            foreach(var dll in SettingsUtil.AOTAssemblyNames)
            {
                string srcDllPath = $"{aotDir}/{dll}.dll";
                if(!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = $"Assets/HotfixAssets/AotMetadata/{dll}.dll.bytes";
                File.Copy(srcDllPath , dllBytesPath , true);
                Debug.Log($"Copy <AOT> assembly dll: name {dll}  {srcDllPath} -> {dllBytesPath}");
            }
        }
    }
}
