using System.Collections.Generic;
using UnityEngine;

namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// 字体管理器
    /// </summary>
    public class FontManager
    {
        public FontManager( )
        {
            m_CacheFonts = new Dictionary<string , Font>( );
        }
        /// <summary>
        /// 需要加载的字体
        /// </summary>
        public readonly string[] FontAssets = new string[]
        {
            "DongFangDaKai.ttf",
        };
        /// <summary>
        /// 缓存的字体
        /// </summary>
        private readonly Dictionary<string , Font> m_CacheFonts;

        /// <summary>
        /// 添加热更字体到缓存中
        /// </summary>
        /// <param name="fontName"></param>
        /// <param name="font"></param>
        public void AddHotfixFontToCache(string fontName , Font font)
        {
            if(!m_CacheFonts.ContainsKey(fontName))
            {
                m_CacheFonts.Add(fontName , font);
            }
        }
        /// <summary>
        /// 从缓存的字体中加载
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public Font GetFont(string fontName)
        {
            if(m_CacheFonts.ContainsKey(fontName))
            {
                return m_CacheFonts[fontName];
            }
            throw new GameFramework.GameFrameworkException("加载字体为空");
        }
    }
}
