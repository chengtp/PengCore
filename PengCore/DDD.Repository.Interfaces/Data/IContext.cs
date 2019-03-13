using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Repository.Interfaces.Data
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
    }
}
