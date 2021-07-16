using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 用户部门岗位关系表（Sys_UserDeptPost）
    /// </summary>
    public class Sys_UserDeptPost
    {
        /// <summary>
        /// 用户表唯一标识
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 部门表唯一标识
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 岗位表唯一标识
        /// </summary>
        public Guid PostId { get; set; }
    }
}
