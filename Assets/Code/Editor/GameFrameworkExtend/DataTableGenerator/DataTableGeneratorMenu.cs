using UnityEditor;
using UnityEngine;

namespace UGHGame.GameEditor
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Tools/Generate DataTables")]
        public static void GenerateDataTables( )
        {
            Debug.Log("准备生成数据表");
        }
    }
}
