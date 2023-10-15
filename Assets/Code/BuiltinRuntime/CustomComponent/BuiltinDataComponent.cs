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
        /// 应用运行时配置【内置配置非热更资源】
        /// </summary>
        public AppBuiltinConfig AppRutimeConfig
        {
            get;
            private set;
        }

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
            //appRuntimeConfig资源为内置资源,就直接使用resources.load进行加载
            AppRutimeConfig = Resources.Load<AppBuiltinConfig>("Builtin/ScriptableAssets/AppBuiltinConfig");
            if(AppRutimeConfig == null)
            {
                Log.Error("APP基础配置加载失败....自动退出游戏");
                GameCollectionEntry.ShutdownGameFramework( );
                return;
            }
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
