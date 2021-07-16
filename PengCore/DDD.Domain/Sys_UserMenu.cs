using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 用户菜单关系表（Sys_UserMenu）
    /// </summary>
    public class Sys_UserMenu
    {
        /// <summary>
        /// 用户表唯一标识
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 菜单表唯一标识
        /// </summary>
        public Guid MenuId { get; set; }
    }
}
