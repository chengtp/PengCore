using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Interfaces
{
    /// <summary>
    /// 定义事务接口
    /// </summary>
    public interface IContext : IDisposable
    {
        bool IsTransactionStarted { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();


        Task<int> RegisterNew<TEntity>(TEntity entity) where TEntity : class;
        Task<bool> RegisterModified<TEntity>(TEntity obj) where TEntity : class;
        Task<bool> RegisterDeleted<TEntity>(TEntity obj) where TEntity : class;
    }
}
