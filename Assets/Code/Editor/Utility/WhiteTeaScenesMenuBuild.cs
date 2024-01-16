using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace WhiteTea.GameEditor
{
    public static class WhiteTeaScenesMenuBuild
    {
        static readonly string ScenesMenuPath = "Code/Editor/Utility/WhiteTeaScenesMenu.cs";
        public static void UpdateList( )
        {
            string scenesMenuPath = Path.Combine(Application.dataPath , ScenesMenuPath);
            var stringBuilder = new StringBuilder( );
            stringBuilder.AppendLine("using UnityEditor;");
            stringBuilder.AppendLine("using UnityEditor.SceneManagement;");
            stringBuilder.AppendLine("namespace WhiteTea.GameEditor");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("  public static class WhiteTeaScenesMenu");
            stringBuilder.AppendLine("  {");
            foreach(string sceneGuid in AssetDatabase.FindAssets("t:Scene" , new string[] { "Assets" }))
            {
                string sceneFilename = AssetDatabase.GUIDToAssetPath(sceneGuid);
                string sceneName = Path.GetFileNameWithoutExtension(sceneFilename);
                string methodName = sceneFilename.Replace('/' , '_').Replace('\\' , '_').Replace('.' , '_').Replace('-' , '_');
                stringBuilder.AppendLine(string.Format("        [MenuItem(\"White Tea Game/Scenes/{0}\", priority = 10)]" , sceneName));
                stringBuilder.AppendLine(string.Format("        public static void {0}()" , methodName ));
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine(string.Format("          OpenScene(\"{0}\"); ", sceneFilename));
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine( );
            }
            stringBuilder.AppendLine("      private static void OpenScene(string filename)");
            stringBuilder.AppendLine("      {");
            stringBuilder.AppendLine("          if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo( ))");
            stringBuilder.AppendLine("          {");
            stringBuilder.AppendLine("              EditorSceneManager.OpenScene(filename);");
            stringBuilder.AppendLine("          }");
            stringBuilder.AppendLine("      }");
            stringBuilder.AppendLine("  }");
            stringBuilder.AppendLine("}");
            Directory.CreateDirectory(Path.GetDirectoryName(scenesMenuPath));
            File.WriteAllText(scenesMenuPath , stringBuilder.ToString( ));
            AssetDatabase.Refresh( );
        }
    }
}
