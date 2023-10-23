using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGHGame.BuiltinRuntime;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 热更入口
    /// </summary>
    public class HotfixEntry
    {
        /// <summary>
        /// 有限状态机
        /// </summary>
        public static FsmManager Fsm
        {
            get;
            private set;
        }

        ///<summary>
        ///流程管理器
        ///</summary>
        public static ProcedureManager Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用热更配置
        /// </summary>
        public AppHotfixConfig AppHotfixConfigs
        {
            get;
            private set;
        }

        /// <summary>
        /// 加载AOT进度
        /// </summary>
        private static int m_CurrentProcess;
        /// <summary>
        /// 加载元数据完成
        /// </summary>
        private static bool m_LoadMetadataForAOTAssembliesFlage = false;
        /// <summary>
        /// 不可调用,供给HybridclrComponent使用【相当于Mono.Start】
        /// </summary>
        public static void Start( )
        {
            LoadMetadataForAOTData( );
            LoadingHotSwappingComponents( );
        }
        /// <summary>
        /// 不可调用,供给HybridclrComponent使用【相当于Mono.Update】
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public static void Update(float elapseSeconds , float realElapseSeconds)
        {
            if(!m_LoadMetadataForAOTAssembliesFlage)
            {
                return;
            }
            Fsm.Update(elapseSeconds , realElapseSeconds);
        }

        /// <summary>
        /// 不可调用,供给HybridclrComponent使用
        /// </summary>
        public static void Shutdown( )
        {

        }
        /// <summary>
        /// 加载AOT元数据
        /// </summary>
        private static void LoadMetadataForAOTData( )
        {
            m_LoadMetadataForAOTAssembliesFlage = false;
            GameCollectionEntry.BuiltinData.GameMainInterface.SetProgressInfo(0 , "补充AOT数据");
            for(int i = 0; i < AppHotfixConfig.AotFileList.Length; i++)
            {
                GameCollectionEntry.Resource.LoadAsset(AssetUtility.GetAotMetadataAsset(AppHotfixConfig.AotFileList[i]) , new LoadAssetCallbacks((assetName , asset , duration , userData) =>
                {
                    byte[] bytes = ( asset as TextAsset ).bytes;
                    bool state = GameCollectionEntry.Hybridclr.LoadMetadataForAOTAssembly(bytes);
                    Log.Info($"LoadMetadataForAOTAssembly:{userData}.加载状态:{state}");
                    GameCollectionEntry.BuiltinData.GameMainInterface.SetGameUpdateProgress(++m_CurrentProcess * 1.0f / AppHotfixConfig.AotFileList.Length);
                } , (assetName , status , errorMessage , userData) =>
                {
                    Debug.LogError($"Can not load aot dll '{assetName}' error message '{errorMessage}'.");
                }) , AppHotfixConfig.AotFileList[i]);
            }
        }

        /// <summary>
        /// 初始化加载组件
        /// </summary>
        private static void LoadingHotSwappingComponents( )
        {
            Fsm = new FsmManager( );
            Procedure = new ProcedureManager( );

            Procedure.Initialize(Fsm , GetHotfixGameProduce( ));

            Procedure.StartProcedure<ProcedureHotfixEntry>( );
        }

        /// <summary>
        /// 获取热更游戏的流程
        /// </summary>
        /// <returns></returns>
        private static ProcedureBase[] GetHotfixGameProduce( )
        {
            List<ProcedureBase> procedures = new List<ProcedureBase>( );
            Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies( ).Where(x => x.GetName( ).Name.Equals(AppBuiltinConfig.HotfixAssembliy)).ToArray( );
            for(int i = 0; i < assembly.Length; i++)
            {
                Type[] types = assembly[i].GetTypes( );
                for(int j = 0; j < types.Length; j++)
                {
                    if(types[j].BaseType.Equals(typeof(ProcedureBase)))
                    {
                        ProcedureBase _base = (ProcedureBase)Activator.CreateInstance(types[j]);
                        if(_base == null)
                        {
                            Log.Error("Can not create procedure instance '{0}'." , types[j].Name);
                            continue;
                        }
                        procedures.Add(_base);
                    }
                }
            }
            return procedures.ToArray( );

            //ProcedureBase[] procedures = new ProcedureBase[AppHotfixConfig.Procedures.Length];
            //for(int i = 0; i < AppHotfixConfig.Procedures.Length; i++)
            //{
            //    Type t = Type.GetType(AppHotfixConfig.Procedures[i]);
            //    if(t == null)
            //    {
            //        Log.Error("无法获取{0}类型" , AppHotfixConfig.Procedures[i]);
            //        continue;
            //    }
            //    procedures[i] = Activator.CreateInstance(t) as ProcedureBase;
            //}
            //Log.Info(procedures == null);
            //return procedures;

        }
    }
}
