using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
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
        /// 初始化构建信息
        /// </summary>
        public void InitBuildInfo( )
        {
           
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        public void InitResourceUI( )
        {
            if(GameCollectionEntry.UI.HasUIGroup("Overlay"))
            {
                UIGroupHelper group = (UIGroupHelper)GameCollectionEntry.UI.GetUIGroup("Overlay").Helper;
                if(group == null)
                {
                    Log.Error("加载UI界面失败");
                    GameCollectionEntry.ShutdownGameFramework(ShutdownType.Restart);
                    return;
                }
                GameMainInterface = Instantiate(Resources.Load<GameObject>("Builtin/UIPrefabs/LoadingInterface") , group.transform).GetComponent<LoadingInterface>( );
                GameMainInterface.transform.SetLocalPositionAndRotation(Vector3.one , Quaternion.identity);
                GameMainInterface.transform.localScale = Vector3.one;
            }
        }
    }
}
