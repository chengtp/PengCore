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
        /// 角色中文名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色英文名称
        /// </summary>
        public string EName { get; set; }
    }
}
