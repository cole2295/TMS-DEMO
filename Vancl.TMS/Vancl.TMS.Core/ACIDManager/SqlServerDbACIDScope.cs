using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Vancl.TMS.Model.Common;
using System.Data.Common;
using Vancl.TMS.Core.Base;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using Vancl.TMS.Core.Pool;
using System.Threading;

namespace Vancl.TMS.Core.ACIDManager
{
    internal class SqlServerDbACIDScope : IACID
    {
        /// <summary>
        /// 是否当前事务完成
        /// </summary>
        public bool IsCompleted
        {
            get;
            private set;
        }

        /// <summary>
        /// 事务等级
        /// </summary>
        internal IsolationLevel IsolationLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 事务实体对象
        /// </summary>
        internal VanclObjectInPool<SqlConnection> ACIDObject
        {
            get;
            private set;
        }

        /// <summary>
        /// 事务Scope
        /// </summary>
        internal DbTransaction Scop
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否最外层ACID
        /// </summary>
        internal bool IsOutterACID
        {
            get;
            private set;
        }

        public SqlServerDbACIDScope(VanclObjectInPool<SqlConnection> VanclConn)
            : this(IsolationLevel.ReadCommitted, VanclConn)
        {
        }

        public SqlServerDbACIDScope(IsolationLevel transLevel, VanclObjectInPool<SqlConnection> VanclConn)
        {
            if (VanclConn == null) throw new ArgumentNullException("VanclConn");
            IsolationLevel = transLevel;
            ACIDObject = VanclConn;
            ACIDObject.IsUsingEnduring = true;
            //
            Init();
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        protected virtual void Init()
        {
            //ConnectionPool<SqlConnection>.AddConnectionCache(Thread.CurrentThread.ManagedThreadId, ACIDObject);
            //first use Transaction
            if (ACIDObject.DataContext == null)
            {
                AttachTransaction();
                ACIDObject.DataContext = new ACIDDataContext() { IsHasTransaction = true, Transaction = Scop };
                return;
            }
            //没有事务则附加事务
            if (!(ACIDObject.DataContext as ACIDDataContext).IsHasTransaction)
            {
                AttachTransaction();
                (ACIDObject.DataContext as ACIDDataContext).IsHasTransaction = true;
                (ACIDObject.DataContext as ACIDDataContext).Transaction = Scop;
                return;
            }
        }

        /// <summary>
        /// 附加事务
        /// </summary>
        protected virtual void AttachTransaction()
        {
            SqlConnection conn = ACIDObject.PoolObject;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            Scop = conn.BeginTransaction(IsolationLevel);
            IsOutterACID = true;
        }


        #region IACID 成员

        public void Complete()
        {
            //inner objects default success
            if (!IsOutterACID)
            {
                IsCompleted = true;
                return;
            }
            Scop.Commit();
            IsCompleted = true;
        }

        #endregion

        /// <summary>
        /// 清空状态并且关闭连接
        /// </summary>
        private void ClearUp()
        {
            if (null == ACIDObject) throw new Exception("ACIDObject Object Is null, Please check.");
            if (ACIDObject.PoolObject != null)
            {
                if (ACIDObject.PoolObject.State == ConnectionState.Open)
                {
                    ACIDObject.PoolObject.Close();
                }
            }
            ACIDObject.DataContext = null;
            ACIDObject.IsUsingEnduring = false;
            if (null != Scop) Scop.Dispose();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (IsOutterACID)
            {
                //如果最外层事务并且没有Completed成功
                try
                {
                    if (!IsCompleted)
                    {
                        Scop.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ClearUp();
                }
            }
            ConnectionPool<OracleConnection>.ReMoveConnectionCache(Thread.CurrentThread.ManagedThreadId);
        }

        #endregion
    }
}
