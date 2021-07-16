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
        /// 外键  岗位类型表唯一标识
        /// </summary>
        public Guid PostTypeId { get; set; }
        /// <summary>
        /// 部门表唯一标识
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 岗位中文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 岗位英文名称
        /// </summary>
        public string EName { get; set; }

    }
}
