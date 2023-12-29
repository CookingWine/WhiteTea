using GameFramework;
using System;
using UnityEngine;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 实体数据基类
    /// </summary>
    [Serializable]
    public abstract class EntityData:IReference
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        [SerializeField]
        private int m_EntityID = 0;

        /// <summary>
        ///  实体类型编号
        /// </summary>
        [SerializeField]
        private int m_EntityTypeID = 0;

        /// <summary>
        /// 实体位置
        /// </summary>
        [SerializeField]
        private Vector3 m_EntityPosition = Vector3.zero;

        /// <summary>
        /// 实体旋转角度
        /// </summary>
        [SerializeField]
        private Quaternion m_EntityRotation = Quaternion.identity;

        /// <summary>
        /// 实体ID
        /// </summary>
        public int EntityID
        {
            get
            {
                return m_EntityID;
            }
        }

        /// <summary>
        /// 实体类型ID
        /// </summary>
        public int EntityTypeID
        {
            get
            {
                return m_EntityTypeID;
            }
        }

        /// <summary>
        /// 实体位置
        /// </summary>
        public Vector3 EntityPosition
        {
            get
            {
                return m_EntityPosition;
            }
            set
            {
                m_EntityPosition = value;
            }
        }
        /// <summary>
        /// 实体角度
        /// </summary>
        public Quaternion EntityRotation
        {
            get
            {
                return m_EntityRotation;
            }
            set
            {
                m_EntityRotation = value;
            }
        }

        /// <summary>
        /// 实体数据
        /// </summary>
        public EntityData( )
        {
            m_EntityID = default(int);
            m_EntityTypeID = default(int);
        }

        /// <summary>
        /// 填充实体数据
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="typeid">实体类型ID</param>
        protected void Fill(int id , int typeid)
        {
            m_EntityID = id;
            m_EntityTypeID = typeid;
        }

        public virtual void Clear( )
        {
            m_EntityID = 0;
            m_EntityTypeID = 0;
            m_EntityPosition = default(Vector3);
            m_EntityRotation = default(Quaternion);
        }
    }
}
