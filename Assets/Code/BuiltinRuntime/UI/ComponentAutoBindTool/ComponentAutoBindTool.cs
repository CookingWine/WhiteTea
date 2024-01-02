using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// UI自动绑定组件
    /// </summary>
    public class ComponentAutoBindTool:MonoBehaviour
    {
#if UNITY_EDITOR
        [Serializable]
        public class BindData
        {
            public BindData( ) { }

            public BindData(string name , Component component)
            {
                Name = name;
                BindCom = component;
            }
            public string Name { get; private set; }

            public Component BindCom { get; private set; }
        }

        public List<BindData> BindDatas = new List<BindData>( );
#endif
        [SerializeField]
        private List<Component> m_BindComs = new List<Component>( );

        /// <summary>
        /// 获取绑定的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetBindComponent<T>(int index) where T : Component
        {
            if(index >= m_BindComs.Count)
            {
                Log.Error("Array Index Overflow.");
                return null;
            }
            T bind = m_BindComs[index] as T;
            if(bind == null)
            {
                Log.Error("Type invalid.");
                return null;
            }
            return bind;

        }
    }
}
