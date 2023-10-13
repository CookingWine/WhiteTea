using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// UI界面辅助器
    /// </summary>
    public class UIGroupHelper:UIGroupHelperBase
    {
        /// <summary>
        /// 画布宽度
        /// </summary>
        public static float Screen_width = 1920;
        /// <summary>
        /// 画布高度
        /// </summary>
        public static float Screen_height = 1080;

        public const int MatchWidthOrHeight = 0;

        public const int DepthFactor = 10000;

        private int m_Depth = 0;
        private Canvas m_CachedCanvas = null;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            Log.Info("Initialize custom UI interface helper, interface depth:{0}." , depth);
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
