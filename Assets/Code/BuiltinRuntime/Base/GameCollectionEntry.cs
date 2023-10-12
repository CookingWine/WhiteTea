using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public partial class GameCollectionEntry:MonoBehaviour
    {
        private void Start( )
        {
            InitBuiltinComponents( );

            InitCustomComponents( );
        }
    }
}
