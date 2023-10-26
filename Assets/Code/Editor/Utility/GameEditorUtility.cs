using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UGHGame.GameEditor
{
    /// <summary>
    /// 选择资源
    /// </summary>
    internal class SelectAssetsData
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable;

        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetsName { get; private set; }

        public SelectAssetsData(string assetsName , bool isEnable)
        {
            AssetsName = assetsName;
            IsEnable = isEnable;
        }
    }
    internal class EidtorPathUtility
    {
        public static string[] GetAllFile(string path)
        {
            List<string> list = new List<string>( );
            if(Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles("*");
                for(int i = 0; i < files.Length; i++)
                {
                    if(files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    list.Add(files[i].Name);
                }
            }
            else
            {
                Debug.LogError($"不存在:【{path}】路径");
            }
            return list.ToArray( );
        }
    }

    /// <summary>
    /// 目录视图
    /// </summary>
    internal abstract class ScrollViewData<T>
    {
        /// <summary>
        /// 记录列表位置
        /// </summary>
        public Vector2 ScrollPos;

        public List<SelectAssetsData> AssetsData { get; protected set; }

        /// <summary>
        /// 重新加载
        /// </summary>
        /// <param name="data"></param>
        protected abstract void Reload(T data);

        /// <summary>
        /// 获取选择的资源名
        /// </summary>
        /// <returns></returns>
        protected abstract string[] GetSelectedAsset( );

        /// <summary>
        /// 设置所有选择资源
        /// </summary>
        internal void SetSelectAll(bool enable)
        {
            if(AssetsData == null)
            {
                return;
            }
            foreach(SelectAssetsData data in AssetsData)
            {
                data.IsEnable = enable;
            }
        }
    }

}
