using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain
{
    /// <summary>
    /// 岗位类型表（Sys_PostType）
    /// </summary>
    [Table("Sys_UserInfo")]
    public class Sys_PostType: BaseModel
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
        public int Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public int EName { get; set; }
    }
}
