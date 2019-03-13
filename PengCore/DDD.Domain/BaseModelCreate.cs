using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 基础表
    /// </summary>
    public class BaseModelCreate : DomainModel.AggregateRoot
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreteDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人唯一标识
        /// </summary>
        public Guid CreateId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

    }
}
