using System;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
using ProcedureOwner = WhiteTea.HotfixLogic.IFsm;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 验证SDK
    /// </summary>
    public class ProcedureVerificationSDK:ProcedureBase
    {
        private float m_current;
        private int m_AllSDKLenth;
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_current = 0;
            Log.Debug("<color=line>开始加载SDK</color>");
            //FacebookSDK.InitFacebookSDK( );
            GameSDKBase[] sdk = GetHotfixGameSDK( );
            m_AllSDKLenth = sdk.Length;
            for(int i = 0; i < sdk.Length; i++)
            {
                sdk[i].InitializedSDK(( ) => { m_current++; });
            }
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            if(m_current < m_AllSDKLenth)
            {
                return;
            }
            WTGame.BuiltinData.GameMainInterface.SetUpdateSchedule("加载SDK" , m_current / m_AllSDKLenth);
            if(m_current >= m_AllSDKLenth)
            {
                //procedureOwner.SetData<VarInt32>(HotfixConstantUtility.NextSceneID , (int)ScenesId.HotfixEntryScenes);
                ChangeState<ProcedureLogin>(procedureOwner);
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner , bool isShutdown)
        {
            base.OnLeave(procedureOwner , isShutdown);
        }

        private GameSDKBase[] GetHotfixGameSDK( )
        {
            GameSDKBase[] sdk = new GameSDKBase[HotfixEntry.AppRuntimeConfig.HotfixGameSDK.Length];
            for(int i = 0; i < HotfixEntry.AppRuntimeConfig.HotfixGameSDK.Length; i++)
            {
                Type t = Type.GetType(HotfixEntry.AppRuntimeConfig.HotfixGameSDK[i]);
                if(t == null)
                {
                    Log.Fatal("无法获取{0}类型" , HotfixEntry.AppRuntimeConfig.HotfixGameSDK[i]);
                    continue;
                }
                sdk[i] = Activator.CreateInstance(t) as GameSDKBase;
            }
            return sdk;
        }
    }
}
