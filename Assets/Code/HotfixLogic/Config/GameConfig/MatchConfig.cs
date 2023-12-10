using UnityEngine;

namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 消消乐配置
    /// </summary>
    [CreateAssetMenu(fileName = "MatchConfig",menuName = "ScriptableObject/MatchConfig【消消乐】")]
    public class MatchConfig:GameAllConfigBase
    {
        /// <summary>
        /// 块之间的间隔
        /// </summary>
        [Header("块之间的间隔")]
        [SerializeField]
        private float m_BlockOffset = 0f;

        /// <summary>
        /// 块之间的间隔
        /// </summary>
        public float BLOCKOFFSET
        {
            get { return m_BlockOffset; }
        }

    }
}
