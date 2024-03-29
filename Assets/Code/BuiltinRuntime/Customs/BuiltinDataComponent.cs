using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 自定义组件
    /// </summary>
    public class BuiltinDataComponent:GameFrameworkComponent
    {

        /// <summary>
        /// 游戏加载界面【主界面】
        /// </summary>
        public LoadingInterface GameMainInterface
        {
            get;
            private set;
        }

        /// <summary>
        /// UI相机
        /// </summary>
        private Camera m_UICamera;
        /// <summary>
        /// 加载默认界面UI
        /// </summary>
        public void InitDefalutResourceUI( )
        {
            if(WTGame.UI.HasUIGroup("Overlay"))
            {
                UGuiGroupHelper group = (UGuiGroupHelper)WTGame.UI.GetUIGroup("Overlay").Helper;
                if(group == null)
                {
                    Log.Error("加载UI界面失败");
                    WTGame.Shutdown(ShutdownType.Restart);
                    return;
                }
                GameMainInterface = Instantiate(Resources.Load<GameObject>(WTGame.AppBuiltinConfigs.LoadingInterfacePath) , group.transform).GetComponent<LoadingInterface>( );
                GameMainInterface.transform.SetLocalPositionAndRotation(Vector3.one , Quaternion.identity);
                GameMainInterface.transform.localScale = Vector3.one;
            }
            InitGameBuiltinData( );
        }

        public void CloseGameMainInterface( )
        {
            GameMainInterface.Close( );
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitGameBuiltinData( )
        {
            if(GameObject.Find("UICamera").TryGetComponent(out m_UICamera))
            {
                m_UICamera.transform.localPosition = new Vector3(99999.0f , 99999.0f , 99999.0f);
            }
        }
    }
}
