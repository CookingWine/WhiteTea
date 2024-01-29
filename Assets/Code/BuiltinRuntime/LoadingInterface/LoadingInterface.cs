using System.Collections;
using UnityEngine;

namespace WhiteTea.BuiltinRuntime
{
    public class LoadingInterface:MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;

        public void Close( )
        {
            StartCoroutine(FadeToAlpha(m_CanvasGroup , 0 , 0.5f));
        }

        private IEnumerator FadeToAlpha(CanvasGroup canvasGroup , float alpha , float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while(time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha , alpha , time / duration);
                yield return new WaitForEndOfFrame( );
            }
            canvasGroup.alpha = alpha;
            yield return new WaitForEndOfFrame( );
            gameObject.SetActive(false);
        }
    }
}
