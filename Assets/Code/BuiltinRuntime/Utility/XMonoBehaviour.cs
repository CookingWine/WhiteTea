using System.Collections;
using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    public class XMonoBehaviour:MonoBehaviour
    {
        protected CanvasGroup m_CanvasGroup = null;
        /// <summary>
        /// 渐变时间
        /// </summary>
        protected const float m_FadeTime = 0.3f;

        private void Awake( )
        {
            GetCanvaGroup( );
            OnAwake( );
        }

        private void Start( )
        {
            OnStart( );
            m_CanvasGroup.alpha = 0;
        }

        private void Update( )
        {
            UpdateFrame(Time.deltaTime);
        }

        //需重写
        //当一个脚本实例被载入
        protected virtual void OnAwake( ) { }
        protected virtual void OnEnable( ) { Open( ); }
        /// <summary>
        /// 仅在Update函数第一次被调用前调用
        /// </summary>
        protected virtual void OnStart( ) { }
        //物体被禁用时调用。
        protected virtual void OnDisable( ) { }

        /// <summary>
        /// 物体被删除时调用。
        /// </summary>
        protected virtual void OnDestroy( ) { }

        /// <summary>
        /// 每一帧被调用
        /// </summary>
        /// <param name="time"></param>
        protected virtual void UpdateFrame(float time) { }

        private void GetCanvaGroup( )
        {
            m_CanvasGroup = this.gameObject.GetOrAddComponent<CanvasGroup>( );
        }

        /// <summary>
        /// 物体启用时被调用。
        /// </summary>
        private void Open( )
        {
            StopAllCoroutines( );
            m_CanvasGroup.alpha = 0;
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f , m_FadeTime));
        }

        public void Close(bool isCo = true)
        {
            if(isCo)
                StartCoroutine(CloseCo(m_FadeTime));
            else
                gameObject.SetActive(false);
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f , duration);
            gameObject.SetActive(false);
        }
    }
}
