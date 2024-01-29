using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    public abstract class BuiltinUGuiForm:UIFormLogic
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;

        private Canvas m_CachedCanvas = null;
        public Canvas GetCanvas
        {
            get
            {
                return m_CachedCanvas;
            }
        }
        private CanvasGroup m_CanvasGroup = null;

        public BuiltinComponentAutoBindTool ComponentTool
        {
            get;
            private set;
        }
        public int OriginalDepth
        {
            get;
            protected set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public void Close( )
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines( );

            if(ignoreFade)
            {
                WTGame.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public static void SetMainFont(Font mainFont)
        {
            if(mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            GameObject go = new GameObject( );
            go.AddComponent<Text>( ).font = mainFont;
            Destroy(go);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>( );
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;
            ComponentTool = gameObject.GetOrAddComponent<BuiltinComponentAutoBindTool>( );
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>( );
            RectTransform transform = GetComponent<RectTransform>( );
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            this.transform.SetLocalPositionAndRotation(Vector3.zero , Quaternion.identity);
            gameObject.GetOrAddComponent<GraphicRaycaster>( );

            SetFormFont( );
        }

        protected virtual void SetFormFont( )
        {
            SetFont(s_MainFont);
        }

        protected void SetFont(Font font)
        {
            Text[] texts = GetComponentsInChildren<Text>(true);
            for(int i = 0; i < texts.Length; i++)
            {
                texts[i].font = font;
                BuiltinLocalizationKey item = texts[i].gameObject.GetComponent<BuiltinLocalizationKey>( );
                if(item != null)
                {
                    if(!string.IsNullOrEmpty(item.LocalizationKey))
                    {
                        texts[i].text = WTGame.Localization.GetString(item.LocalizationKey);
                    }
                    else
                    {
                        texts[i].text = item.Values;
                    }
                }
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_CanvasGroup.alpha = 0f;
            StopAllCoroutines( );
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f , FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown , object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown , userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause( )
#else
        protected internal override void OnPause()
#endif
        {
            base.OnPause( );
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume( )
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume( );

            m_CanvasGroup.alpha = 0f;
            StopAllCoroutines( );
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f , FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover( )
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover( );
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal( )
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal( );
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds , float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds , realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth , int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth , depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
            for(int i = 0; i < canvases.Length; i++)
            {
                canvases[i].sortingOrder += deltaDepth;
            }
        }

        private void OnDestroy( )
        {
            OnDestroyObject( );
        }
        /// <summary>
        /// 界面销毁
        /// </summary>
        protected virtual void OnDestroyObject( )
        {

        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f , duration);
            WTGame.UI.CloseUIForm(this);
        }

        protected void SetTextFont(Text text)
        {
            text.font = s_MainFont;
        }
    }
}
