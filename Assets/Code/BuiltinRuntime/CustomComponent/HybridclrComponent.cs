using GameFramework;
using HybridCLR;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// hybridclr热更组件
    /// </summary>
    public class HybridclrComponent:GameFrameworkComponent
    {
        /// <summary>
        /// Hybridclr元数据模式
        /// <para>Consistent模式:即补充的dll与打包时裁剪后的dll精确一致。因此必须使用build过程中生成的裁剪后的dll，则不能直接复制原始dll</para>
        /// <para>SuperSet模式:即补充的dll是打包时裁剪后的dll的超集。这个模式放松对了AOT dll的要求，你既可以用裁剪后的AOT dll，也可以用原始AOT dll</para>
        /// </summary>
        [Header("元数据模式【默认使用SuperSet模式】")]
        [SerializeField]
        private HomologousImageMode m_HomologousImageMode = HomologousImageMode.SuperSet;

        private Action m_SuccessComplate = null;
        private Action<float , float> m_UpdateCallback = null;
        private Action m_ShutdownCallback = null;

        /// <summary>
        /// 进入热更新【注意:进入热更前,需要先把AOT元数据加载完毕】
        /// </summary>
        public void HotfixEntry( )
        {
            StartCoroutine(LoadHotfixEntry( ));
        }

        /// <summary>
        /// 为AOT程序集加载元数据
        /// </summary>
        /// <param name="dllBytes">dll文件</param>
        /// <returns>是否加载成功</returns>
        public bool LoadMetadataForAOTAssembly(byte[] dllBytes)
        {
            return LoadMetadataForAOT(dllBytes) == LoadImageErrorCode.OK;
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
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        private LoadImageErrorCode LoadMetadataForAOT(byte[] dllBytes)
        {
            return RuntimeApi.LoadMetadataForAOTAssembly(dllBytes , m_HomologousImageMode);
        }

        /// <summary>
        /// 开始加载热更入口
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadHotfixEntry( )
        {
            yield return new WaitForEndOfFrame( );

            //加载热更程序集
            Type logic = Utility.Assembly.GetType("UGHGame.HotfixLogic");
            if(logic == null)
            {
                yield break;
            }
            //通过反射创造出Delegate后运行
            MethodInfo start = logic.GetMethod("Start" , BindingFlags.Public | BindingFlags.Static);
            MethodInfo update = logic.GetMethod("Update" , BindingFlags.Public | BindingFlags.Static);
            MethodInfo shutdown = logic.GetMethod("Shutdown" , BindingFlags.Public | BindingFlags.Static);

            yield return new WaitForEndOfFrame( );
            Log.Info("Hotfix main entry loaded, wait to enter the game!");
            m_SuccessComplate?.Invoke( );
            start.Invoke(null , null);
            m_UpdateCallback = (Action<float , float>)Delegate.CreateDelegate(typeof(Action<float , float>) , null , update);
            m_ShutdownCallback = (Action)Delegate.CreateDelegate(typeof(Action) , null , shutdown);
        }
    }
}
