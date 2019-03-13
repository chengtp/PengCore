using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Repository.Interfaces.UnitOfWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
