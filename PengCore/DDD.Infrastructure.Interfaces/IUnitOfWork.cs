using System;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();


        Task<int> RegisterNew<TEntity>(TEntity entity) where TEntity : class;
        Task<bool> RegisterModified<TEntity>(TEntity obj) where TEntity : class;
        Task<bool> RegisterDeleted<TEntity>(TEntity obj) where TEntity : class;

    }
}
