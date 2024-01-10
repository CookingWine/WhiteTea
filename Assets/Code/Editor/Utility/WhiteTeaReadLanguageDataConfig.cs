using UnityEditor;
using UnityEngine;

namespace WhiteTea.GameEditor
{
    internal partial class WhiteTeaLocalizationConfigs
    {
        /// <summary>
        /// 本地化数据类型
        /// </summary>
        public enum LocalizationType
        {
            /// <summary>
            /// 所有
            /// </summary>
            All,
            /// <summary>
            /// 语言
            /// </summary>
            Language,
            /// <summary>
            /// 图片
            /// </summary>
            Sprite,
            /// <summary>
            /// 音效
            /// </summary>
            Audio
        }

        private GUIStyle TextFieldRoundEdge;
        private GUIStyle TextFieldRoundEdgeCancelButton;
        private GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
        private GUIStyle TransparentTextField;
        string m_InputSearchText;
        /// <summary>
        /// 绘制搜索框
        /// </summary>
        private void DrawSearchBox( )
        {
            if(TextFieldRoundEdge == null)
            {
                TextFieldRoundEdge = new GUIStyle("SearchTextField");
                TextFieldRoundEdgeCancelButton = new GUIStyle("SearchCancelButton");
                TextFieldRoundEdgeCancelButtonEmpty = new GUIStyle("SearchCancelButtonEmpty");
                TransparentTextField = new GUIStyle(EditorStyles.whiteLabel);
                TransparentTextField.normal.textColor = EditorStyles.textField.normal.textColor;
            }
            //获取当前输入框的Rect(位置大小)
            Rect position = EditorGUILayout.GetControlRect( );
            //设置圆角style的GUIStyle
            GUIStyle textFieldRoundEdge = TextFieldRoundEdge;
            //设置输入框的GUIStyle为透明，所以看到的“输入框”是TextFieldRoundEdge的风格
            GUIStyle transparentTextField = TransparentTextField;
            //选择取消按钮(x)的GUIStyle
            GUIStyle gUIStyle = ( m_InputSearchText != "" ) ? TextFieldRoundEdgeCancelButton : TextFieldRoundEdgeCancelButtonEmpty;

            //输入框的水平位置向左移动取消按钮宽度的距离
            position.width -= gUIStyle.fixedWidth;
            //如果面板重绘
            if(Event.current.type == EventType.Repaint)
            {
                textFieldRoundEdge.Draw(position , new GUIContent("") , 0);
            }
            Rect rect = position;
            //为了空出左边那个放大镜的位置
            float num = textFieldRoundEdge.CalcSize(new GUIContent("")).x - 2f;
            rect.width -= num;
            rect.x += num;
            rect.y += 1f;//为了和后面的style对其
            m_InputSearchText = EditorGUI.TextField(rect , m_InputSearchText , transparentTextField);
            //绘制取消按钮，位置要在输入框右边
            position.x += position.width;
            position.width = gUIStyle.fixedWidth;
            position.height = gUIStyle.fixedHeight;
            if(GUI.Button(position , GUIContent.none , gUIStyle) && m_InputSearchText != "")
            {
                m_InputSearchText = "";
                //用户是否做了输入
                GUI.changed = true;
                //把焦点移开输入框
                GUIUtility.keyboardControl = 0;
            }
        }
    }
}
