using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 岗位表（sys_Post）
    /// </summary>
    [Table("sys_Post")]
    public class Sys_Post : BaseModel
    {
        /// <summary>
        /// 外键  部门表唯一标识
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string Name { get; set; }

    }
}
