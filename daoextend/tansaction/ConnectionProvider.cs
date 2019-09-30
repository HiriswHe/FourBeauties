using System;
using System.Configuration;
using System.Data;
using System.Collections;

namespace daoextend.tansaction
{
	public class ConnectionProvider : IDisposable
	{
        #region 变量
            private	IDbConnection		_dBConnection;
			private	bool				_isTransactionPending, _isDisposed;
			private	IDbTransaction		_currentTransaction;
		#endregion

        /// <summary>
        /// 实例化
        /// </summary>
		public ConnectionProvider(IDbConnection dbConnection)
		{
            InitClass(dbConnection);
        }
        
        /// <summary>
        /// 垃圾回收
        /// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDisposing"></param>
		protected virtual void Dispose(bool isDisposing)
		{
			if(!_isDisposed)
			{
				if(isDisposing)
				{
					if(_currentTransaction != null)
					{
						_currentTransaction.Dispose();
						_currentTransaction = null;
					}
					if(_dBConnection != null)
					{
						_dBConnection.Close();
						_dBConnection.Dispose();
						_dBConnection = null;
					}
				}
			}
			_isDisposed = true;
		}

        /// <summary>
        /// 变量初始化
        /// </summary>
		private void InitClass(IDbConnection dbConnection)
		{
            _dBConnection = dbConnection;
			_isDisposed = false;
			_currentTransaction = null;
			_isTransactionPending = false;
		}


		/// <summary>
		/// 打开连接
		/// </summary>
		/// <returns>true</returns>
		public bool OpenConnection()
		{
			try
			{
				if((_dBConnection.State & ConnectionState.Open) > 0)
				{
					throw new Exception("OpenConnection::Connection is already open.");
				}
				_dBConnection.Open();
				_isTransactionPending = false;
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}




		/// <summary>
		/// 事务开始
		/// </summary>
		/// <param name="transactionName">事务名称</param>
		/// <returns>true</returns>
		public bool BeginTransaction()
		{
			try
			{
				if(_isTransactionPending)
				{
					throw new Exception("BeginTransaction::Already transaction pending. Nesting not allowed");
				}
				if((_dBConnection.State & ConnectionState.Open) == 0)
				{
					throw new Exception("BeginTransaction::Connection is not open.");
				}
				_currentTransaction = _dBConnection.BeginTransaction();
				_isTransactionPending = true;
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 事务提交
		/// </summary>
		/// <returns>true</returns>
		public bool CommitTransaction()
		{
			try
			{
				if(!_isTransactionPending)
				{
					throw new Exception("CommitTransaction::No transaction pending.");
				}
				if((_dBConnection.State & ConnectionState.Open) == 0)
				{
					throw new Exception("CommitTransaction::Connection is not open.");
				}
				_currentTransaction.Commit();
				_isTransactionPending = false;
				_currentTransaction.Dispose();
				_currentTransaction = null;
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 事务回滚
		/// </summary>
		/// <param name="transactionToRollback">事务名</param>
		/// <returns>true</returns>
		public bool RollbackTransaction()
		{
			try
			{
				if(!_isTransactionPending)
				{
					throw new Exception("RollbackTransaction::No transaction pending.");
				}
				if((_dBConnection.State & ConnectionState.Open) == 0)
				{
					throw new Exception("RollbackTransaction::Connection is not open.");
				}
				_currentTransaction.Rollback();
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 保存事务节点
		/// </summary>
		/// <param name="savePointName">事务节点名称</param>
		/// <returns>true</returns>
		public bool Commit()
		{
			try
			{
				if(!_isTransactionPending)
				{
					throw new Exception("SaveTransaction::No transaction pending.");
				}
				if((_dBConnection.State & ConnectionState.Open) == 0)
				{
					throw new Exception("SaveTransaction::Connection is not open.");
				}
				_currentTransaction.Commit();
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 关闭连接
		/// </summary>
		/// <param name="commitPendingTransaction">事务是否处理</param>
		/// <returns>true,</returns>
		public bool CloseConnection(bool commitPendingTransaction)
		{
			try
			{
				if((_dBConnection.State & ConnectionState.Open) == 0)
				{
					return false;
				}
				if(_isTransactionPending)
				{
					if(commitPendingTransaction)
					{
						_currentTransaction.Commit();
					}
					else
					{
						_currentTransaction.Rollback();
					}
					_isTransactionPending = false;
					_currentTransaction.Dispose();
					_currentTransaction = null;
				}
				_dBConnection.Close();
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}


		#region 属性
        /// <summary>
        /// 当前事务
        /// </summary>
		public IDbTransaction CurrentTransaction
		{
			get
			{
				return _currentTransaction;
			}
		}

        /// <summary>
        /// 事务挂起
        /// </summary>
		public bool IsTransactionPending
		{
			get
			{
				return _isTransactionPending;
			}
		}

        /// <summary>
        /// 连接
        /// </summary>
		public IDbConnection DBConnection
		{
			get
			{
				return _dBConnection;
			}
		}
		#endregion
	}
}
