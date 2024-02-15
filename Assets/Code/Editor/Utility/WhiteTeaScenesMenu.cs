using UnityEditor;
using UnityEditor.SceneManagement;
namespace WhiteTea.GameEditor
{
  public static class WhiteTeaScenesMenu
  {
        [MenuItem("白茶游戏配置/切换场景/HotfixEntryScenes", priority = 10)]
        public static void Assets_HotfixAssets_Scenes_HotfixEntryScenes_unity()
        {
          OpenScene("Assets/HotfixAssets/Scenes/HotfixEntryScenes.unity"); 
        }

        [MenuItem("白茶游戏配置/切换场景/GameEntry", priority = 10)]
        public static void Assets_Scenes_GameEntry_unity()
        {
          OpenScene("Assets/Scenes/GameEntry.unity"); 
        }

        [MenuItem("白茶游戏配置/切换场景/UIEditorScene", priority = 10)]
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
