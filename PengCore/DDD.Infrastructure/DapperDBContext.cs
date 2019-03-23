using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DDD.Infrastructure
{
    /// <summary>
    /// Dapper 数据上下文
    /// </summary>
    public abstract class DapperDBContext : Interfaces.IContext
    {
        public IDbConnection _connection;
        public IDbTransaction _transaction;

        private readonly DapperDBContextOptions _options;

        public bool IsTransactionStarted { get; private set; }
        protected abstract IDbConnection CreateConnection(string connectionString);

        #region 构造方法

        protected DapperDBContext(IOptions<DapperDBContextOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            _connection = CreateConnection(_options.Configuration);
            _connection.Open();

            DebugPrint("Connection started.");
        }

        #endregion



        #region Transaction

        public void BeginTransaction()
        {
            if (IsTransactionStarted)
                throw new InvalidOperationException("Transaction is already started.");

            _transaction = _connection.BeginTransaction();
            IsTransactionStarted = true;

            DebugPrint("Transaction started.");
        }

        public void Commit()
        {
            if (!IsTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Commit();
            _transaction = null;

            IsTransactionStarted = false;

            DebugPrint("Transaction committed.");
        }

        public void Dispose()
        {
            if (IsTransactionStarted)
                Rollback();

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            DebugPrint("Connection closed and disposed.");
        }

        public void Rollback()
        {
            if (!IsTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            IsTransactionStarted = false;

            DebugPrint("Transaction rollbacked and disposed.");
        }

        public async Task<int> RegisterNew<TEntity>(TEntity obj) where TEntity : class
        {
            return await _connection.InsertAsync<TEntity>(obj, _transaction);
        }

        public async Task<bool> RegisterModified<TEntity>(TEntity obj) where TEntity : class
        {
            return await _connection.UpdateAsync<TEntity>(obj, _transaction);
        }

        public async Task<bool> RegisterDeleted<TEntity>(TEntity obj) where TEntity : class
        {
            return await _connection.DeleteAsync<TEntity>(obj, _transaction);
        }

        #endregion

        #region pubulic mathod

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>> UnitOfWorkWithDapper - Thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }

        #endregion
    }
}
