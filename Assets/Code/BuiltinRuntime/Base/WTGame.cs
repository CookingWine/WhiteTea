using UnityEngine;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public partial class WTGame:MonoBehaviour
    {
        private void Start( )
        {
            InitBuiltinComponents( );
            InitCustomComponents( );
        }
    }
}
