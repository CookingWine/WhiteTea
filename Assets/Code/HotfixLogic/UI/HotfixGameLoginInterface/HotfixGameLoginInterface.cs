using UnityEngine;
using WhiteTea.BuiltinRuntime;

namespace WhiteTea.HotfixLogic
{
    public class HotfixGameLoginInterface:BuiltinUGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            this.transform.SetLocalPositionAndRotation(Vector3.zero , Quaternion.identity);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            HotfixEntry.Timer.AddTimer(0.5f , WTGame.BuiltinData.EnableResourceUI);
        }

        protected override void OnUpdate(float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds , realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown , object userData)
        {
            base.OnClose(isShutdown , userData);
        }

    }
}
