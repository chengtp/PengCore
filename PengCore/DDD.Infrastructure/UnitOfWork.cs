using DDD.Infrastructure.Interfaces;
using System;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContext _context;

        public UnitOfWork(IContext context)
        {
            _context = context;
            _context.BeginTransaction();
        }

        public void Commit()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("Transaction have already been commited or disposed.");

            _context.Commit();
        }

        public void Dispose()
        {
            if (_context.IsTransactionStarted)
                _context.Rollback();
        }

        public void Rollback()
        {
            _context.Rollback();
        }


        public async Task<int> RegisterNew<TEntity>(TEntity obj) where TEntity : class
        {
            return await _context.RegisterNew<TEntity>(obj);
        }

        public async Task<bool> RegisterModified<TEntity>(TEntity obj) where TEntity : class
        {
            return await _context.RegisterModified<TEntity>(obj);
        }

        public async Task<bool> RegisterDeleted<TEntity>(TEntity obj) where TEntity : class
        {
            return await _context.RegisterModified<TEntity>(obj);
        }
    }
}
