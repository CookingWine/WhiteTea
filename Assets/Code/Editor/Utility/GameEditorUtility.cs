using System.Collections.Generic;
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
        internal void SetSelectAll(bool enable )
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
