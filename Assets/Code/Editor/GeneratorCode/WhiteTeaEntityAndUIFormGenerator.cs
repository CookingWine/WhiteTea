using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WhiteTea.BuiltinRuntime;
using BindData = WhiteTea.BuiltinRuntime.BuiltinComponentAutoBindTool.BindData;
namespace WhiteTea.GameEditor
{
    /// <summary>
    /// 实体与界面代码生成器
    /// </summary>
    public class WhiteTeaEntityAndUIFormGenerator:EditorWindow
    {
        private enum GenCodeType
        {
            Entity,
            UIForm
        }
        [SerializeField]
        private List<GameObject> m_GameObjects = new List<GameObject>( );

        private SerializedObject m_SerializedObject;
        private SerializedProperty m_SerializedProperty;

        private GenCodeType m_GenCodeType = GenCodeType.UIForm;

        /// <summary>
        /// 是否生成主体逻辑代码
        /// </summary>
        private bool m_IsGenMainLogicCode = true;

        /// <summary>
        /// 是否生成自动绑定组件代码
        /// </summary>
        private bool m_IsGenAutoBindCode = true;

        /// <summary>
        /// 是否生成实体数据代码
        /// </summary>
        private bool m_IsGenEntityDataCode = true;

        /// <summary>
        /// 是否生成显示实体代码
        /// </summary>
        private bool m_IsGenShowEntityCode = true;

        public static void OpenCodeGeneratorWindow( )
        {
            WhiteTeaEntityAndUIFormGenerator window = GetWindow<WhiteTeaEntityAndUIFormGenerator>(true , "实体与界面代码生成器");
            window.minSize = new Vector2(300f , 300f);
        }
        private void OnEnable( )
        {
            m_SerializedObject = new SerializedObject(this);
            m_SerializedProperty = m_SerializedObject.FindProperty("m_GameObjects");
        }

        private void OnGUI( )
        {
            EditorGUI.BeginChangeCheck( );
            EditorGUILayout.PropertyField(m_SerializedProperty , true);
            if(EditorGUI.EndChangeCheck( ))
            {
                m_SerializedObject.ApplyModifiedProperties( );
            }

            //绘制自动生成代码类型的弹窗
            EditorGUILayout.BeginHorizontal( );
            {
                EditorGUILayout.LabelField("自动生成的代码类型：" , GUILayout.Width(140f));
                m_GenCodeType = (GenCodeType)EditorGUILayout.EnumPopup(m_GenCodeType , GUILayout.Width(100f));
                EditorGUILayout.EndHorizontal( );

                //绘制代码生成路径文本
                EditorGUILayout.BeginHorizontal( );
                EditorGUILayout.LabelField("自动生成的代码路径：" , GUILayout.Width(140f));
                switch(m_GenCodeType)
                {
                    case GenCodeType.Entity:
                        EditorGUILayout.LabelField(WhiteTeaEditorConfigs.EntityCodePath);
                        break;
                    case GenCodeType.UIForm:
                        EditorGUILayout.LabelField(WhiteTeaEditorConfigs.UIFormCodePath);
                        break;
                }
            }
            EditorGUILayout.EndHorizontal( );

            //绘制各个选项
            m_IsGenMainLogicCode = GUILayout.Toggle(m_IsGenMainLogicCode , "生成主体逻辑代码");


            EditorGUILayout.BeginHorizontal( );
            {
                m_IsGenAutoBindCode = GUILayout.Toggle(m_IsGenAutoBindCode , "生成自动绑定组件代码" , GUILayout.Width(150f));
            }

            EditorGUILayout.EndHorizontal( );

            if(m_GenCodeType == GenCodeType.Entity)
            {
                m_IsGenEntityDataCode = GUILayout.Toggle(m_IsGenEntityDataCode , "生成实体数据代码");
                m_IsGenShowEntityCode = GUILayout.Toggle(m_IsGenShowEntityCode , "生成快捷显示实体代码");
            }

            //绘制生成代码的按钮
            if(GUILayout.Button("生成代码" , GUILayout.Width(100f)))
            {
                if(m_GameObjects.Count == 0)
                {
                    EditorUtility.DisplayDialog("警告" , "请选择实体或界面的游戏物体" , "OK");
                    return;
                }

                if(m_GenCodeType == GenCodeType.Entity)
                {
                    GenEntityCode( );
                }
                else
                {
                    GenUIFormCode( );
                }

                AssetDatabase.Refresh( );
                EditorUtility.DisplayDialog("提示" , "代码生成完毕" , "OK");

            }
        }
        /// <summary>
        /// 生成实体代码
        /// </summary>
        private void GenEntityCode( )
        {

        }
        /// <summary>
        /// 生成UIform代码
        /// </summary>
        private void GenUIFormCode( )
        {
            string codepath = WhiteTeaEditorConfigs.UIFormCodePath;
            string nameSpace = "WhiteTea.HotfixLogic";
            string logicBaseClass = "BuiltinUGuiForm";
            foreach(GameObject go in m_GameObjects)
            {
                if(m_IsGenMainLogicCode)
                {
                    GenUIFormMainLogicCode(codepath , go , nameSpace , logicBaseClass);
                }

                if(m_IsGenAutoBindCode)
                {
                    GenAutoBindCode(codepath , go , nameSpace);
                }
            }
        }

        /// <summary>
        /// 生成主体逻辑代码
        /// </summary>
        /// <param name="codePath"></param>
        /// <param name="go"></param>
        /// <param name="nameSpace"></param>
        /// <param name="logicBaseClass"></param>
        private void GenUIFormMainLogicCode(string codePath , GameObject go , string nameSpace , string logicBaseClass)
        {
            string initParam = string.Empty;
            string baseInitParam = string.Empty;
            string accessModifier = "protected";

            if(!Directory.Exists($"{codePath}/"))
            {
                Directory.CreateDirectory($"{codePath}/");
            }

            using(StreamWriter sw = new StreamWriter($"{codePath}/{go.name}.cs"))
            {
                sw.WriteLine("using WhiteTea.BuiltinRuntime;");

                sw.WriteLine("");
                sw.WriteLine("//自动生成于："/* + DateTime.Now*/);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {go.name} : {logicBaseClass}");
                sw.WriteLine("\t{");

                //OnInit
                sw.WriteLine($"\t\t{accessModifier} override void OnInit({initParam}object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnInit({baseInitParam}userdata);");
                sw.WriteLine($"\t\t\tGetBindComponents(ComponentTool);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");


                //OnOpen
                sw.WriteLine($"\t\t{accessModifier} override void OnOpen(object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnOpen(userdata);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnClose
                sw.WriteLine($"\t\t{accessModifier} override void OnClose(bool isShutdown, object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnClose(isShutdown, userdata);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnUpdate
                sw.WriteLine($"\t\t{accessModifier} override void OnUpdate(float elapseSeconds, float realElapseSeconds)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnUpdate(elapseSeconds, realElapseSeconds);");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //end
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }
        /// <summary>
        /// 生成自动绑定代码
        /// </summary>
        /// <param name="codePath"></param>
        /// <param name="go"></param>
        /// <param name="nameSpace"></param>
        /// <param name="nameEx"></param>
        private void GenAutoBindCode(string codePath , GameObject go , string nameSpace , string nameEx = "")
        {
            BuiltinComponentAutoBindTool bindTool = go.GetComponent<BuiltinComponentAutoBindTool>( );
            if(bindTool == null)
            {
                return;
            }
            bindTool.ScriptSpaceName = nameSpace;
            bindTool.ScriptTypeName = go.name;
            if(!Directory.Exists($"{codePath}/BindingComponent/"))
            {
                Directory.CreateDirectory($"{codePath}/BindingComponent/");
            }

            using(StreamWriter sw = new StreamWriter($"{codePath}/BindingComponent/{go.name}{nameEx}.BindingComponent.cs"))
            {
                sw.WriteLine("using UnityEngine;");
                if(m_GenCodeType == GenCodeType.UIForm)
                {
                    sw.WriteLine("using UnityEngine.UI;");
                }
                sw.WriteLine("using WhiteTea.BuiltinRuntime;");
                sw.WriteLine("");
                sw.WriteLine("//自动生成于：" /*+ DateTime.Now*/);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {go.name}{nameEx}");
                sw.WriteLine("\t{");
                sw.WriteLine("");


                foreach(BindData data in bindTool.BindDatas)
                {
                    sw.WriteLine($"\t\tprivate {data.BindCom.GetType( ).Name} m_{data.Name};");
                }
                sw.WriteLine("");

                sw.WriteLine("\t\tprivate void GetBindComponents(BuiltinComponentAutoBindTool autoBindTool)");
                sw.WriteLine("\t\t{");

                //根据索引获取

                for(int i = 0; i < bindTool.BindDatas.Count; i++)
                {
                    BindData data = bindTool.BindDatas[i];
                    string filedName = $"m_{data.Name}";
                    sw.WriteLine($"\t\t\t{filedName} = autoBindTool.GetBindComponent<{data.BindCom.GetType( ).Name}>({i});");
                }

                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }
        }
    }
}
