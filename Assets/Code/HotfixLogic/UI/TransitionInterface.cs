using GameFramework;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

//自动生成于：
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 过渡界面
    /// </summary>
    public partial class TransitionInterface:BuiltinUGuiForm
    {
        /// <summary>
        /// 随机显示文字的key最大值
        /// </summary>
        private readonly int m_RandomContentMaxCount = 10;
        /// <summary>
        /// 随机背景图片的最大个数
        /// </summary>
        private readonly int m_TransitionBGMaxCount = 10;
        /// <summary>
        /// 随机显示的文字key
        /// </summary>
        private readonly string m_RandomShowContentText = "TransitionKey_";
        /// <summary>
        /// 过渡背景的加载路径
        /// </summary>
        private readonly string m_TransitionInterfaceBgGoup = "BuiltinRuntimes/TransitionInterface/BG/TransitionInterface_";
        /// <summary>
        /// 缓存的图片
        /// </summary>
        private List<Sprite> m_Cache = new List<Sprite>( );
        protected override void OnInit(object userdata)
        {
            base.OnInit(userdata);
            GetBindComponents(ComponentTool);
            if(m_Img_TransitionBg.sprite != null)
            {
                m_Cache.Add(m_Img_TransitionBg.sprite);
            }
            InitTransitionInterfaceBg( );
        }

        protected override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);
            string key = m_RandomShowContentText + Utility.Random.GetRandom(0 , m_RandomContentMaxCount);
            string value = WTGame.Localization.GetString(key);
            if(!value.Equals("NoKey"))
            {
                m_Txt_Content.text = value;
            }
            if(m_Cache.Count > 0)
            {
                int randomSpriteIndex = Utility.Random.GetRandom(0 , m_Cache.Count);
                m_Img_TransitionBg.sprite = m_Cache[randomSpriteIndex];
            }
        }

        protected override void OnClose(bool isShutdown , object userdata)
        {
            base.OnClose(isShutdown , userdata);
        }

        protected override void OnUpdate(float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds , realElapseSeconds);
        }

        /// <summary>
        /// 初始化过渡界面的背景
        /// </summary>
        private void InitTransitionInterfaceBg( )
        {
            for(int i = 2; i < m_TransitionBGMaxCount; i++)
            {
                string path = BuiltinRuntimeUtility.AssetsUtility.GetHotfixUISpritesAssets(m_TransitionInterfaceBgGoup + i + ".png");
                WTGame.Resource.LoadAsset(path , new LoadAssetCallbacks(LoadSpriteSuccess , LoadSpriteFailed));

            }
        }
        /// <summary>
        /// 加载图片成功回调
        /// </summary>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="asset">已加载的资源。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        private void LoadSpriteSuccess(string assetName , object asset , float duration , object userData)
        {
            Texture2D texture2D = asset as Texture2D;
            Sprite sprite = texture2D.Texture2DToSprite( );
            m_Cache.Add(sprite);

        }
        /// <summary>
        /// 加载图片资源失败回调
        /// </summary>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="status">加载资源状态。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        private void LoadSpriteFailed(string assetName , LoadResourceStatus status , string errorMessage , object userData)
        {
            Log.Error("Can not load sprite '{0}' from '{1}' with error message '{2}'" , assetName , assetName , errorMessage);
        }
    }
}
