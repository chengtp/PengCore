using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 岗位菜单关系表（Sys_PostMenu）
    /// </summary>
    public class Sys_PostMenu
    {
        /// <summary>
        /// 岗位表唯一标识
        /// </summary>
        public Guid PostId { get; set; }
        /// <summary>
        /// 菜单表唯一标识
        /// </summary>
        public Guid MenuId { get; set; }
    }
}
