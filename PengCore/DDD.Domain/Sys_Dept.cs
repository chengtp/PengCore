using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 部门表（sys_Dept）
    /// </summary>
    [Table("sys_Post")]
    public class Sys_Dept : BaseModel
    {
        /// <summary>
        /// 父的唯一标识
        /// </summary>
        public Guid? Pid { get; set; }
        /// <summary>
        /// 层级,默认0
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string EName { get; set; }
    }
}
