using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 用户角色关系表（Sys_UserRole）
    /// </summary>
    public class Sys_UserRole
    {
        /// <summary>
        /// 用户表唯一标识
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色表唯一标识
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
