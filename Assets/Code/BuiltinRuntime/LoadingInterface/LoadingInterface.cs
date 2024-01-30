using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteTea.BuiltinRuntime
{
    public class LoadingInterface:MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Image UpdateProgressBar;
        public Text ProgressBarText;
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

        /// <summary>
        /// 设置更新进度
        /// </summary>
        /// <param name="schedule">进度</param>
        /// <param name="currentUpdateLength">已更新大小</param>
        /// <param name="totalUpdateLength">总大小</param>
        /// <param name="currentSpeed">当前下载速度</param>
        public void SetUpdateSchedule(float schedule , string currentUpdateLength , string totalUpdateLength , string currentSpeed)
        {
            UpdateProgressBar.fillAmount = schedule;
            ProgressBarText.text = schedule * 100 + "%";
        }
    }
}
