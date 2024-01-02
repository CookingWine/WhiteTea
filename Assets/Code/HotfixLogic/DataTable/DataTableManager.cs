using System;
using System.Collections.Generic;
using UGHGame.BuiltinRuntime;
using UnityGameFramework.Runtime;
namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 数据表管理器
    /// </summary>
    public class DataTableManager
    {
        private static readonly string DataRowClassPrefixName = $"{AppBuiltinConfig.HotfixEntryClass}.DR";

        /// <summary>
        /// 数据表缓存
        /// </summary>
        private readonly Dictionary<Type , DataRowBase[]> m_CacheDataTables = new Dictionary<Type , DataRowBase[]>( );
        
        /// <summary>
        /// 缓存数据表的个数
        /// </summary>
        public int Count
        {
            get
            {
                return m_CacheDataTables.Count;
            }
        }

        /// <summary>
        /// 数据表管理器
        /// </summary>
        public DataTableManager()
        {
            m_CacheDataTables.Clear( );
        }

        public void Shutdown( )
        {
            m_CacheDataTables.Clear( );
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetDataTable<T>( ) where T : DataRowBase
        {
            Type type = typeof(T);
            if(m_CacheDataTables.ContainsKey(type))
            {
                int index = 0;
                T[] results = new T[m_CacheDataTables[type].Length];
                foreach(DataRowBase dataRow in m_CacheDataTables[type])
                {
                    results[index++] = (T)dataRow;
                }

                return results;
            }
            return default(T[]);
        }

        /// <summary>
        /// 获取数据表内的一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public T GetDataTableRow<T>(int rowId) where T : DataRowBase
        {
            Type type = typeof(T);
            if(m_CacheDataTables.ContainsKey(type))
            {
                DataRowBase[] dataRows = m_CacheDataTables[type];
                for(int i = 0; i < dataRows.Length; i++)
                {
                    if(dataRows[i].Id == rowId)
                    {
                        return (T)dataRows[i];
                    }
                }
            }
            return default(T);
        }
    }
}
