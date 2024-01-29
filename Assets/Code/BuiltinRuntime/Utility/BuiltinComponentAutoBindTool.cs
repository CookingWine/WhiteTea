using System.Collections.Generic;
using UnityEngine;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 组件自动绑定工具
    /// </summary>
    public class BuiltinComponentAutoBindTool:MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        public class BindData
        {
            public BindData( )
            {
            }

            public BindData(string name , Component bindCom)
            {
                Name = name;
                BindCom = bindCom;
            }

            public string Name;
            public Component BindCom;
        }

        public List<BindData> BindDatas = new List<BindData>( );

        public string ScriptSpaceName = string.Empty;
        public string ScriptTypeName = string.Empty;

#endif

        [SerializeField]
        private List<Component> m_BindComs = new List<Component>( );

        /// <summary>
        /// 获取绑定的组件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>物体</returns>
        public T GetBindComponent<T>(int index) where T : Component
        {
            if(index > m_BindComs.Count)
            {
                Debug.LogError("索引无效");
                return null;
            }
            T binCom = m_BindComs[index] as T;
            if(binCom == null)
            {
                Debug.LogError("类型无效");
                return null;
            }
            return binCom;
        }
    }
}
