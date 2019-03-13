using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace DDD.Domain.DomainModel
{
    /// <summary>
    /// mongodb,xml,redis实体基类
    /// 主键类型为string
    /// </summary>
    [Serializable]
    public abstract class NoSqlEntity : IEntity
    {
       
    }
}
