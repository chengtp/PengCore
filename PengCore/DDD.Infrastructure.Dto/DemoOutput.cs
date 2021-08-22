using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos
{
    public class DemoOutput
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreteDate { get; set; }
        /// <summary>
        /// 创建人唯一标识
        /// </summary>
        public Guid CreateId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 是否删除；0: 正常;1:删除,默认0正常
        /// </summary>
        public int Deleted { get; set; }
        /// <summary>
        /// 排序码，默认0
        /// </summary>
        public int SortNum { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditDate { get; set; }
        /// <summary>
        /// 编辑人唯一标识
        /// </summary>
        public Guid EditerId { get; set; }
        /// <summary>
        /// 编辑人名称
        /// </summary>
        public string EditerName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 性别；0男，1女,默认 0男
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 禁用状态;0启用，1禁用，默认1启用
        /// </summary>
        public int Disabled { get; set; }
    }
}
