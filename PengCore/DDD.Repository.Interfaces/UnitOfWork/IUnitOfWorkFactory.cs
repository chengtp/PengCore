using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Repository.Interfaces.UnitOfWork
{
    /// <summary>
    /// 定义工作单元工厂
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
