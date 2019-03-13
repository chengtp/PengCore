using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 菜单表（sys_Menu）
    /// </summary>
    [Table("Sys_Menu")]
    public class Sys_Menu : BaseModel
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
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Link { get; set; }

    }
}
