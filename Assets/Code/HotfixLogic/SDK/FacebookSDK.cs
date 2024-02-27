using Facebook.Unity;
using UnityGameFramework.Runtime;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// facebookSDK
    /// </summary>
    public class FacebookSDK
    {
        public static string FacebookAppID
        {
            get
            {
                return FB.AppId;
            }
        }
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public static bool Initialized
        {
            get
            {
                return FB.IsInitialized;
            }
        }
        /// <summary>
        /// 初始化FacebookSDK
        /// </summary>
        public static void InitFacebookSDK( )
        {
            if(FB.IsInitialized)
            {
                return;
            }
            FB.Init(( ) =>
            {
                Log.Debug("初始化完成回调,是否登录{0},是否初始化{1}" , FB.IsLoggedIn , FB.IsInitialized);

            } , (isUnityShutDown) =>
            {
                Log.Debug("Hide unity:{0}" , isUnityShutDown);
            });
        }
    }
}
