using UnityEditor;
using UnityEditor.SceneManagement;
namespace WhiteTea.GameEditor
{
  public static class WhiteTeaScenesMenu
  {
        [MenuItem("White Tea Game/Scenes/GameEntry", priority = 10)]
        public static void Assets_Scenes_GameEntry_unity()
        {
          OpenScene("Assets/Scenes/GameEntry.unity"); 
        }

        [MenuItem("White Tea Game/Scenes/UIEditorScene", priority = 10)]
        public static void Assets_Scenes_UIEditorScene_unity()
        {
          OpenScene("Assets/Scenes/UIEditorScene.unity"); 
        }

      private static void OpenScene(string filename)
      {
          if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo( ))
          {
              EditorSceneManager.OpenScene(filename);
          }
      }
  }
}
