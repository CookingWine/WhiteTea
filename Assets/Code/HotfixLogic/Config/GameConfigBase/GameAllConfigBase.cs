using UnityEngine;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameAllConfigBase:ScriptableObject
    {
        /// <summary>
        /// 游戏ID
        /// </summary>
        [Header("游戏ID")]
        [SerializeField]
        private int m_GameID;
        /// <summary>
        /// 游戏ID
        /// </summary>
        public int GameID
        {
            get { return m_GameID; }
        }

        /// <summary>
        /// 游戏名称
        /// </summary>
        [Header("游戏名称")]
        [SerializeField]
        private string m_GameName;

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName
        {
            get { return m_GameName; }
        }
    }
}
