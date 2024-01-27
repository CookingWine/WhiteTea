using GameFramework.Resource;
using System.Threading.Tasks;
using System;
using UnityGameFramework.Runtime;
using GameFramework;
using System.Reflection;
using UnityEngine;
using System.Collections;
using HybridCLR;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// hybridclr热更组件
    /// </summary>
    public class HybridclrComponent:GameFrameworkComponent
    {
        private Action m_SuccessComplate = null;
        private Action<float , float> m_UpdateCallback = null;
        private Action m_ShutdownCallback = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks;
        private TaskCompletionSource<object> s_LoadAssetTcs;
        /// <summary>
        /// Hybridclr元数据模式
        /// <para>Consistent模式:即补充的dll与打包时裁剪后的dll精确一致。因此必须使用build过程中生成的裁剪后的dll，则不能直接复制原始dll</para>
        /// <para>SuperSet模式:即补充的dll是打包时裁剪后的dll的超集。这个模式放松对了AOT dll的要求，你既可以用裁剪后的AOT dll，也可以用原始AOT dll</para>
        /// </summary>
        [Header("元数据模式【默认使用SuperSet模式】")]
        [SerializeField]
        private HomologousImageMode m_HomologousImageMode = HomologousImageMode.SuperSet;

        /// <summary>
        /// 进入热更新
        /// </summary>
        public async void HotfixEntry(Action complate)
        {
            m_SuccessComplate = complate;

            await AssemblyLoad(WTGame.AppBuiltinConfigs.HotfixAssembliy);

            StartCoroutine(LoadHotfixEntry( ));
        }

        /// <summary>
        /// <param name="dllBytes">dll文件</param>
        /// <returns>是否加载成功</returns>
        /// </summary>
        public bool LoadMetadataForAOTAssembly(byte[] dllBytes)
        {
            return LoadMetadataForAOT(dllBytes) == LoadImageErrorCode.OK;
        }
        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        private LoadImageErrorCode LoadMetadataForAOT(byte[] dllBytes)
        {
            return RuntimeApi.LoadMetadataForAOTAssembly(dllBytes , m_HomologousImageMode);
        }

        private void Start( )
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess , OnLoadAssetFailure);
        }

        private void Update( )
        {
            if(m_UpdateCallback == null)
            {
                return;
            }
            m_UpdateCallback(Time.deltaTime , Time.unscaledDeltaTime);
        }

        private void OnDestroy( )
        {
            m_SuccessComplate = null;
            if(m_ShutdownCallback == null)
            {
                return;
            }
            m_ShutdownCallback( );
        }

        /// <summary>
        /// 开始加载热更入口
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadHotfixEntry( )
        {
            yield return new WaitForEndOfFrame( );

            //加载热更程序入口类
            Type logic = Utility.Assembly.GetType(WTGame.AppBuiltinConfigs.HotfixEntryClass);
            if(logic == null)
            {
                Log.Error($"加载失败,未找到{WTGame.AppBuiltinConfigs.HotfixEntryClass}");
                yield break;
            }
            //通过反射创造出Delegate后运行
            MethodInfo start = logic.GetMethod(WTGame.AppBuiltinConfigs.HotfixStartFuntion , BindingFlags.Public | BindingFlags.Static);
            MethodInfo update = logic.GetMethod(WTGame.AppBuiltinConfigs.HotfixUpdate , BindingFlags.Public | BindingFlags.Static);
            MethodInfo shutdown = logic.GetMethod(WTGame.AppBuiltinConfigs.HotfixShutdown , BindingFlags.Public | BindingFlags.Static);
            yield return new WaitForEndOfFrame( );
            Log.Debug("Hotfix main entry loaded, wait to enter the game!");
            m_SuccessComplate?.Invoke( );
            start.Invoke(null , null);
            m_UpdateCallback = (Action<float , float>)Delegate.CreateDelegate(typeof(Action<float , float>) , null , update);
            m_ShutdownCallback = (Action)Delegate.CreateDelegate(typeof(Action) , null , shutdown);
        }

        private async Task<Assembly> AssemblyLoad(string assembliyName)
        {
            string assetPath = BuiltinRuntimeUtility.AssetsUtility.GetHotfixDllAsset(assembliyName);
            TextAsset dll = await AwaitLoadAsset<TextAsset>(assetPath);
            byte[] dllBytes = dll.bytes;
            Assembly hotfix = Assembly.Load(dllBytes);
            return hotfix;
        }
        /// <summary>
        /// 加载资源（可等待）
        /// </summary>
        public async Task<T> AwaitLoadAsset<T>(string assetName)
        {
            object asset = await AwaitInternalLoadAsset<T>(assetName);
            return (T)asset;
        }

        private Task<object> AwaitInternalLoadAsset<T>(string assetName)
        {
            s_LoadAssetTcs = new TaskCompletionSource<object>( );

            WTGame.Resource.LoadAsset(assetName , typeof(T) , m_LoadAssetCallbacks);

            return s_LoadAssetTcs.Task;
        }

        private void OnLoadAssetSuccess(string assetName , object asset , float duration , object userData)
        {
            s_LoadAssetTcs.SetResult(asset);
        }

        private void OnLoadAssetFailure(string assetName , LoadResourceStatus status , string errorMessage , object userData)
        {
            s_LoadAssetTcs.SetException(new GameFrameworkException(errorMessage));
        }
    }
}
