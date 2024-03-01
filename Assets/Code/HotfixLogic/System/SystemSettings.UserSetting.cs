using UnityEngine;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;

namespace WhiteTea.HotfixLogic
{
    public partial class SystemSettings
    {
        /// <summary>
        /// 用户配置
        /// </summary>
        public class UserSettings
        {
            /// <summary>
            /// 最大未登录的天数【正常来说这个应该由策划去配置，而不是由程序这边固定写死】
            /// </summary>
            private const int m_MaximumNumberOfDaysWithoutLogin = 15;

            /// <summary>
            /// 用户名称
            /// </summary>
            public string UserName { get; private set; }

            /// <summary>
            /// 游戏名称
            /// </summary>
            public string GameName { get; private set; }

            /// <summary>
            /// 用户头像
            /// </summary>
            public Sprite UserIcon { get; private set; }


            /// <summary>
            /// 用户经验
            /// </summary>
            public long UserExperience { get; private set; }

            /// <summary>
            /// 注册时间
            /// </summary>
            public long UserRegistrationDate { get; private set; }

            /// <summary>
            /// 最后一次登录时间
            /// </summary>
            public long LastLoginTime { get; private set; }

            /// <summary>
            /// 本地是否有用户数据
            /// </summary>
            public bool UserDataExistsLocally { get; private set; }

            /// <summary>
            /// 是否同意用户条款
            /// </summary>
            public bool IsAgreeToUserTerms { get; set; }



            public UserSettings( )
            {
                LoadLocalUserSetting( );
            }

            /// <summary>
            /// 加载本地用户数据
            /// </summary>
            private void LoadLocalUserSetting( )
            {
                //TODO:先从本地读取用户登录的数据


                //如果加载到用户数据，先判断是否超过15天没有登录游戏
                if(UserDataExistsLocally)
                {
                    int lastTime = (int)BuiltinRuntimeDateTimeExtend.GetTimeLongAgo(LastLoginTime);
                    if(lastTime > m_MaximumNumberOfDaysWithoutLogin)
                    {
                        //TODO:清除本地用户数据


                        //读取本地数据失败
                        UserDataExistsLocally = false;
                    }
                    else
                    {
                        //未超过最大天数,更新时间戳
                        LastLoginTime = BuiltinRuntimeDateTimeExtend.GetTimeSwap( );
                        //TODO:这里最好根据保留的用户信息，再去请求一下用户名称
                        UserName = "";

                    }
                }
                else
                {

                }
            }

            /// <summary>
            /// 注册用户数据
            /// <para>这里应该是通过加载的登录SDK去获取信息</para>
            /// <paramref name="userName">根据授权的信息获取用户名</paramref>
            /// </summary>
            public void RegisterLocalUserSetting(string userName)
            {
                UserName = userName;
            }

            /// <summary>
            /// 保存用户数据
            /// </summary>
            public void SaveLocalUserSetting( )
            {

            }
        }
    }
}
