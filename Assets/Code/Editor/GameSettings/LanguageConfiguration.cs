using UnityEditor;
using UnityEngine;
namespace UGHGame.GameEditor
{
    /// <summary>
    /// 语言配置
    /// </summary>
    public class LanguageConfiguration:EditorWindow
    {
        [MenuItem("UGHGame Settings/Language Configuration" , false , 30)]
        private static void OpenLanguageWindow( )
        {
            var temp = GetWindow<LanguageConfiguration>( );
            temp.position = new Rect(600 , 600 , 600 , 600);
        }
    }
}
