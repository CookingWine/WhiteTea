using System;
using System.IO;
using GameFramework;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 内置运行工具
    /// </summary>
    public static class BuiltinRuntimeUtility
    {
        /// <summary>
        /// app设置的名称
        /// </summary>
        public const string AppBuiltinSettingsName = "AppSettings";

        /// <summary>
        /// app热更内的配置
        /// </summary>
        public const string AppHotfixSettingsName = "HotifxAppConfigs";

        /// <summary>
        /// 设置
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// 语言
            /// </summary>
            public const string Language = "WTGame.Settings.Language";
        }

        /// <summary>
        /// 资源工具
        /// </summary>
        public static class AssetsUtility
        {
            /// <summary>
            /// 获取组合路径
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public static string GetCombinePath(params string[] args)
            {
                return Utility.Path.GetRegularPath(Path.Combine(args));
            }

            /// <summary>
            /// 获取语言配置文件
            /// </summary>
            /// <param name="language">当前语言</param>
            /// <param name="isHotfix">是否是热更资源</param>
            /// <returns></returns>
            public static string GetLanguageAssets(string language , bool isHotfix)
            {
                string path = isHotfix ? "Assets/HotfixAssets/Localization/Local_{0}.bytes" : "Language/Local_{0}";
                UnityEngine.Debug.Log($"加载文件路径为{Utility.Text.Format(path , language)}");
                return Utility.Text.Format(path , language);
            }
        }
        /// <summary>
        /// json工具
        /// </summary>
        public static class JsonUtility
        {

        }

        public static class ValuerUtility
        {
            public static string GetByteLengthString(long byteLength)
            {
                if(byteLength < 1024L) // 2 ^ 10
                {
                    return Utility.Text.Format("{0} Bytes" , byteLength);
                }

                if(byteLength < 1048576L) // 2 ^ 20
                {
                    return Utility.Text.Format("{0:F2} KB" , byteLength / 1024f);
                }

                if(byteLength < 1073741824L) // 2 ^ 30
                {
                    return Utility.Text.Format("{0:F2} MB" , byteLength / 1048576f);
                }

                if(byteLength < 1099511627776L) // 2 ^ 40
                {
                    return Utility.Text.Format("{0:F2} GB" , byteLength / 1073741824f);
                }

                if(byteLength < 1125899906842624L) // 2 ^ 50
                {
                    return Utility.Text.Format("{0:F2} TB" , byteLength / 1099511627776f);
                }

                if(byteLength < 1152921504606846976L) // 2 ^ 60
                {
                    return Utility.Text.Format("{0:F2} PB" , byteLength / 1125899906842624f);
                }

                return Utility.Text.Format("{0:F2} EB" , byteLength / 1152921504606846976f);
            }

            /// <summary>
            /// 转换成1K 1M
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public static string ToCoins(int num)
            {
                if(num >= 100000000)
                    return ( num / 1000000D ).ToString("0.#M");
                if(num >= 1000000)
                    return ( num / 1000000D ).ToString("0.##M");
                if(num >= 100000)
                    return ( num / 1000D ).ToString("0K");
                if(num >= 100000)
                    return ( num / 1000D ).ToString("0.#K");
                if(num >= 1000)
                    return ( num / 1000D ).ToString("0.##K");
                return num.ToString("#,0");
            }
            public static string ToCoins(float num)
            {
                return ToCoins(RoundToInt(num));
            }
            /// <summary>
            /// 转换成美元格式
            /// </summary>
            /// <param name="usd"></param>
            /// <returns></returns>
            public static string ToUsd(float usd)
            {
                return Utility.Text.Format("{0:N2}" , usd);
            }
            public static string ToUsd(long usdCent)
            {
                return ToUsd(usdCent * 0.01f);
            }
            public static string Float2String(float v , int dotNum)
            {
                if(v - (int)v != 0)
                {
                    return v.ToString(Utility.Text.Format("N{0}" , dotNum));
                }
                else
                {
                    return v.ToString( );
                }
            }
            /// <summary>
            /// 格式化秒为 00:00:00
            /// </summary>
            /// <param name="seconds">单位秒</param>
            /// <returns></returns>
            public static string ToTime(float seconds)
            {
                return System.TimeSpan.FromSeconds(seconds).ToString("hh\\:mm\\:ss");
            }

            public static int RoundToInt(float value)
            {
                return (int)Math.Round(value , MidpointRounding.AwayFromZero);
            }
        }
    }
}
