using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// UGUI界面辅助器
    /// </summary>
    public class UGuiGroupHelper:UIGroupHelperBase
    {
        /// <summary>
        /// 画布宽
        /// </summary>
        public static int Screen_width
        {
            get
            {
                return 1080;
            }
        }
        /// <summary>
        /// 画布高
        /// </summary>
        public static int Screen_height
        {
            get
            {
                return 2400;
            }
        }

        /// <summary>
        /// 匹配宽度或高度
        /// </summary>
        public const int MatchWidthOrHeight = 0;

        /// <summary>
        /// 深度系数
        /// </summary>
        public const int DepthFactor = 10000;

        /// <summary>
        /// 深度
        /// </summary>
        private int m_Depth = 0;

        /// <summary>
        /// 缓存画布
        /// </summary>
        private Canvas m_CachedCanvas = null;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            m_Depth = depth;
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * depth;
        }

        private void Awake( )
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>( );
            gameObject.GetOrAddComponent<GraphicRaycaster>( );
        }
        private void Start( )
        {
            InitCanvas( );
            RectTransform transform = GetComponent<RectTransform>( );
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }
        private void InitCanvas( )
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>( );
            if(m_Depth == 0)
            {
                m_CachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                m_CachedCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>( );
                m_CachedCanvas.planeDistance = m_CachedCanvas.worldCamera.farClipPlane / 2;
                m_CachedCanvas.sortingLayerName = name;
            }
            else
            {
                m_CachedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                m_CachedCanvas.overrideSorting = true;
                m_CachedCanvas.sortingOrder = DepthFactor * m_Depth;
            }
            CanvasScaler csc = gameObject.GetOrAddComponent<CanvasScaler>( );
            csc.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            csc.referenceResolution = new Vector2(Screen_width , Screen_height);
            csc.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            csc.matchWidthOrHeight = MatchWidthOrHeight;
            gameObject.GetOrAddComponent<GraphicRaycaster>( );
        }

        public void SetLayer(string nameLayer)
        {
            gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
    }
}
