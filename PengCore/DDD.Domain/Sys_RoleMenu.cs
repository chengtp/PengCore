using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 角色菜单关系表（Sys_RoleMenu）
    /// </summary>
    public class Sys_RoleMenu
    {
        /// <summary>
        /// 角色表唯一标识
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 菜单表唯一标识
        /// </summary>
        public Guid MenuId { get; set; }
    }
}
