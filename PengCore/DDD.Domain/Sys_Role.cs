using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 角色表（sys_Role）
    /// </summary>
    [Table("Sys_Role")]
    public class Sys_Role : BaseModel
    {
        /// <summary>
        /// 外键  部门表唯一标识
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

    }
}
