using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain.DomainModel
{
    /// <summary>
    /// 集合根接口，用于泛型约束，约束领域实体为聚合根，表示实现了该接口的为集合根实例，
    /// 由于聚合根也是领域实体的一种，所以也要实现IEntity接口
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
    }
}
