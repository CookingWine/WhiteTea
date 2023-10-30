using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 游戏加载界面
    /// </summary>
    public class LoadingInterface:XMonoBehaviour
    {
        #region Component
        /// <summary>
        /// 主背景
        /// </summary>
        [Header("背景界面")]
        [SerializeField]
        private CanvasGroup m_GameBG;

        /// <summary>
        /// 进度条背景
        /// </summary>
        [Header("进度条背景")]
        [SerializeField]
        private Image m_ProgressBarBG;
        /// <summary>
        /// 进度条
        /// </summary>
        [Header("进度条")]
        [SerializeField]
        private Image m_ProgressBar;
        /// <summary>
        /// 显示进度的txt
        /// </summary>
        [Header("当前进度")]
        [SerializeField]
        private Text m_CurrentProgress;

        [Header("视频画布")]
        [SerializeField]
        private CanvasGroup m_VideoCanvas;

        [Header("视频界面")]
        [SerializeField]
        private MediaPlayer m_VideoInterface;

        [Header("弹窗")]
        [SerializeField]
        private GameObject m_PopUpNotificationBG;

        [Header("弹窗内容")]
        [SerializeField]
        private Text m_NotificationContent;

        [Header("弹窗确认按钮1")]
        [SerializeField]
        private Button m_LeftNotificationConfirm;

        [Header("弹窗确认按钮内容1")]
        [SerializeField]
        private Text m_LeftNotificationConfirmContent;

        [Header("弹窗确认按钮2")]
        [SerializeField]
        private Button m_RightNotificationConfirm;

        [Header("弹窗确认按钮内容2")]
        [SerializeField]
        private Text m_RightNotificationConfirmContent;

        /// <summary>
        /// 按钮的位置
        /// </summary>
        private readonly Vector2[] m_NotificationConfirmPosition = new Vector2[3] { new Vector2(0 , -250) , new Vector2(-190 , -250) , new Vector2(190 , -250) , };
        #endregion

        /// <summary>
        /// 视频是否播放完毕
        /// </summary>
        public bool IsPlayVideoOver
        {
            get;
            private set;
        }
        protected override void OnAwake( )
        {
            InitComponent( );
            InitEvent( );
            InitData( );
        }
        /// <summary>
        /// 设置进度条信息
        /// </summary>
        /// <param name="_value">进度【取值范围0~1】</param>
        /// <param name="info">需要显示的信息</param>
        public void SetProgressInfo(float _value , string info)
        {
            m_ProgressBar.fillAmount = _value;
            m_CurrentProgress.text = info;
        }

        /// <summary>
        /// 设置进度条信息
        /// </summary>
        /// <param name="_current">当前进度</param>
        /// <param name="_total">总进度</param>
        /// <param name="info">显示信息</param>
        public void SetProgressInfo(float _current , float _total , string info)
        {
            m_ProgressBar.fillAmount = _current / _total;
            m_CurrentProgress.text = $"{info}{(int)(  _current / _total  * 100 )}%";
        }

        /// <summary>
        /// 打开弹窗
        /// </summary>
        /// <param name="notificationContent">弹窗内容</param>
        /// <param name="leftConfirmContent">按钮1的显示内容</param>
        /// <param name="leftCall">按钮1的点击事件</param>
        /// <param name="rightConfirmContent">按钮2的显示内容</param>
        /// <param name="rightCall">按钮2的点击事件</param>
        public void OpenPopUpNotification(string notificationContent , string leftConfirmContent = "" , UnityAction leftCall = null , string rightConfirmContent = "" , UnityAction rightCall = null)
        {
            m_NotificationContent.text = notificationContent;
            m_LeftNotificationConfirm.onClick.RemoveAllListeners( );
            m_RightNotificationConfirm.onClick.RemoveAllListeners( );
            if(leftCall == null && rightCall == null)
            {
                m_LeftNotificationConfirm.SetActive(false);
                m_RightNotificationConfirm.SetActive(false);
            }
            else if(leftCall != null && rightCall != null)
            {
                m_LeftNotificationConfirmContent.text = leftConfirmContent;
                m_RightNotificationConfirmContent.text = rightConfirmContent;
                m_LeftNotificationConfirm.transform.localPosition = m_NotificationConfirmPosition[1];
                m_RightNotificationConfirm.transform.localPosition = m_NotificationConfirmPosition[2];
                m_LeftNotificationConfirm.onClick.AddListener(leftCall);
                m_RightNotificationConfirm.onClick.AddListener(rightCall);
                m_LeftNotificationConfirm.SetActive(true);
                m_RightNotificationConfirm.SetActive(true);

            }
            else if(leftCall != null || rightCall != null)
            {
                string content = string.Empty;
                if(!rightConfirmContent.IsNullOrEmpty( ))
                {
                    content = rightConfirmContent;
                }
                if(!leftConfirmContent.IsNullOrEmpty( ))
                {
                    content = leftConfirmContent;
                }
                m_LeftNotificationConfirmContent.text = content;
                UnityAction call = null;
                if(rightCall != null)
                {
                    call = rightCall;
                }
                if(leftCall != null)
                {
                    call = leftCall;
                }
                m_LeftNotificationConfirm.onClick.AddListener(call);
                m_LeftNotificationConfirm.transform.localPosition = m_NotificationConfirmPosition[0];
                m_LeftNotificationConfirm.SetActive(true);
            }
            m_PopUpNotificationBG.SetActive(true);
        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public void ClosePopUpNotification( )
        {
            m_PopUpNotificationBG.SetActive(false);
            m_NotificationContent.text = string.Empty;
            m_LeftNotificationConfirmContent.text = string.Empty;
            m_RightNotificationConfirmContent.text = string.Empty;
            m_LeftNotificationConfirm.onClick.RemoveAllListeners( );
            m_RightNotificationConfirm.onClick.RemoveAllListeners( );
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitComponent( )
        {
            m_VideoCanvas.SetActive(true);
            m_GameBG.SetActive(false);
            m_PopUpNotificationBG.SetActive(false);
        }
        private void InitData( )
        {
            IsPlayVideoOver = false;
        }
        private void InitEvent( )
        {
            if(m_VideoInterface != null)
            {
                m_VideoInterface.Events.AddListener(OnMediaPlayerEvent);
            }
        }

        /// <summary>
        /// 视频播放完成回调
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="et"></param>
        /// <param name="errorCode"></param>
        private void OnMediaPlayerEvent(MediaPlayer mp , MediaPlayerEvent.EventType et , ErrorCode errorCode)
        {
            if(et == MediaPlayerEvent.EventType.FinishedPlaying)
            {
                IsPlayVideoOver = true;
                EnterMainInterface( );
            }
        }
        /// <summary>
        /// 进入主界面
        /// </summary>
        private void EnterMainInterface( )
        {
            m_VideoCanvas.alpha = 1.0f;
            m_GameBG.SetActive(true);
            m_GameBG.alpha = 0f;
            m_VideoCanvas.DOFade(0 , 0.5f).SetEase(Ease.Linear).OnComplete(( ) =>
            {
                m_VideoCanvas.SetActive(false);
            });
            m_GameBG.DOFade(1 , 1).SetEase(Ease.Linear).OnComplete(( ) =>
            {
                if(!(Application.internetReachability != NetworkReachability.NotReachable))
                {
                    OpenPopUpNotification("无法连接网络.." , "退出游戏" , ( ) => { GameCollectionEntry.ShutdownGameFramework( ); });
                }
            });
        }
    }
}
