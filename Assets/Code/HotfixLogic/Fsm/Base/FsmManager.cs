using GameFramework;
using System;
using System.Collections.Generic;
namespace UGHGame.HotfixLogic
{
    /// <summary>
    /// 有限状态机管理器
    /// </summary>
    public sealed class FsmManager
    {
        private readonly Dictionary<string , FsmBase> m_Fsms;
        private readonly List<FsmBase> m_FsmList;
        private readonly List<FsmBase> m_TempFsms;

        /// <summary>
        /// 初始化有限状态机管理器的新实例。
        /// </summary>
        public FsmManager( )
        {
            m_Fsms = new Dictionary<string , FsmBase>( );
            m_FsmList = new List<FsmBase>( );
            m_TempFsms = new List<FsmBase>( );
        }


        /// <summary>
        /// 获取有限状态机数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Fsms.Count;
            }
        }

        /// <summary>
        /// 有限状态机管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds , float realElapseSeconds)
        {
            m_TempFsms.Clear( );
            if(m_Fsms.Count <= 0)
            {
                return;
            }

            for(int i = 0; i < m_FsmList.Count; i++)
            {
                m_TempFsms.Add(m_FsmList[i]);
            }

            for(int i = 0; i < m_TempFsms.Count; i++)
            {
                FsmBase fsm = m_TempFsms[i];

                if(fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.Update(elapseSeconds , realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理有限状态机管理器。
        /// </summary>
        public void Shutdown( )
        {
            foreach(KeyValuePair<string , FsmBase> fsm in m_Fsms)
            {
                fsm.Value.Shutdown( );
            }

            m_Fsms.Clear( );
            m_FsmList.Clear( );
            m_TempFsms.Clear( );
        }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm(Type ownerType)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalHasFsm(GetFullName(ownerType , string.Empty));
        }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm(Type ownerType , string name)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalHasFsm(GetFullName(ownerType , name));
        }

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>要获取的有限状态机。</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalGetFsm(GetFullName(ownerType , string.Empty));
        }
        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>要获取的有限状态机。</returns>
        public FsmBase GetFsm(Type ownerType , string name)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalGetFsm(GetFullName(ownerType , name));
        }

        /// <summary>
        /// 获取所有有限状态机。
        /// </summary>
        /// <returns>所有有限状态机。</returns>
        public FsmBase[] GetAllFsms( )
        {
            int index = 0;
            FsmBase[] results = new FsmBase[m_Fsms.Count];
            foreach(KeyValuePair<string , FsmBase> fsm in m_Fsms)
            {
                results[index++] = fsm.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有有限状态机。
        /// </summary>
        /// <param name="results">所有有限状态机。</param>
        public void GetAllFsms(List<FsmBase> results)
        {
            if(results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear( );
            foreach(KeyValuePair<string , FsmBase> fsm in m_Fsms)
            {
                results.Add(fsm.Value);
            }
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm CreateFsm(object owner , params FsmState[] states)
        {
            return CreateFsm(string.Empty , owner , states);
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm CreateFsm(string name , object owner , params FsmState[] states)
        {
            if(HasFsm(owner.GetType( ) , name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist FSM '{0}'." , GetFullName(owner.GetType( ) , name)));
            }

            Fsm fsm = new Fsm(name , owner , states);
            m_FsmList.Add(fsm);
            m_Fsms.Add(GetFullName(owner.GetType( ) , name) , fsm);
            return fsm;
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(Type ownerType)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalDestroyFsm(GetFullName(ownerType , string.Empty));
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">要销毁的有限状态机名称。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(Type ownerType , string name)
        {
            if(ownerType == null)
            {
                throw new GameFrameworkException("Owner type is invalid.");
            }

            return InternalDestroyFsm(GetFullName(ownerType , name));
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(IFsm fsm)
        {
            if(fsm == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }

            return InternalDestroyFsm(GetFullName(fsm.Owner.GetType( ) , fsm.Name));
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            if(fsm == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }

            return InternalDestroyFsm(GetFullName(fsm.OwnerType , fsm.Name));
        }

        private bool InternalHasFsm(string fullName)
        {
            return m_Fsms.ContainsKey(fullName);
        }

        private FsmBase InternalGetFsm(string fullName)
        {
            FsmBase fsm = null;
            if(m_Fsms.TryGetValue(fullName , out fsm))
            {
                return fsm;
            }

            return null;
        }

        private bool InternalDestroyFsm(string fullName)
        {
            FsmBase fsm = null;
            if(m_Fsms.TryGetValue(fullName , out fsm))
            {
                fsm.Shutdown( );
                m_FsmList.Remove(fsm);
                return m_Fsms.Remove(fullName);
            }

            return false;
        }

        private string GetFullName(Type type , string name)
        {
            if(type == null)
            {
                throw new GameFrameworkException("Type is invalid.");
            }
            string fullName = type.FullName;
            if(!string.IsNullOrEmpty(name))
            {
                return Utility.Text.Format("{0}.{1}" , fullName , name);
            }
            return fullName;
        }

    }
}
