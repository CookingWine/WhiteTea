using UnityEditor;
using UnityEngine;

namespace WhiteTea.GameEditor.GameConfigs
{
    /// <summary>
    /// 游戏配置器
    /// </summary>
    [SerializeField]
    internal partial class WhiteTeaGameConfigs:EditorWindow
    {
        private WhiteTeaGameConfigs( ) { }
        private static WhiteTeaGameConfigs m_Instance;
        public static WhiteTeaGameConfigs Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = (WhiteTeaGameConfigs)GetWindow(typeof(WhiteTeaGameConfigs) , true , m_WindowName , true);
                    m_Instance.minSize = m_WindowMinSize;
                }
                return m_Instance;
            }
        }
        /// <summary>
        /// 加载配置器窗口
        /// </summary>
        public void LoadWhiteTeaGameConfigsWindow( )
        {
            m_Instance?.Show( );
        }

        protected virtual void OnEnable( )
        {

        }

        private void OnGUI( )
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width) , GUILayout.Height(position.height));
            {
                //左边区域
                EditorGUILayout.BeginVertical(GUILayout.Width(200f));
                {
                    m_LeftAreaSlider = EditorGUILayout.BeginScrollView(m_LeftAreaSlider);
                    {
                        EditorGUILayout.BeginVertical("box");
                        {

                        }
                        EditorGUILayout.EndVertical( );
                    }
                    EditorGUILayout.EndScrollView( );
                }
                EditorGUILayout.EndVertical( );

                GUILayout.Space(20f);
                //右边区域
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 220f));
                {
                    m_RightAreaSlider = EditorGUILayout.BeginScrollView(m_RightAreaSlider);
                    {

                    }
                    EditorGUILayout.EndScrollView( );
                }
                EditorGUILayout.EndVertical( );
            }
        }
    }
}
