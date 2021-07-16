using Dapper.Contrib.Extensions;
using System;

namespace DDD.Domain
{
    /// <summary>
    /// 用户表(sys_UserInfo)
    /// </summary>
    [Table("Sys_UserInfo")]
    public class Sys_UserInfo : BaseModel
    {
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
        /// 用户英文名称
        /// </summary>
        public string UserEName { get; set; }
        /// <summary>
        /// 公司Idcard
        /// </summary>
        public string CompanyIDCard { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 离开日期
        /// </summary>
        public DateTime? LeftDate { get; set; }
        /// <summary>
        /// 国家；0中国，1乍得
        /// </summary>
        public int? Country { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 其它邮箱
        /// </summary>
        public string OtherEmail { get; set; }
        /// <summary>
        /// 国内手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 其他手机号码
        /// </summary>
        public string OtherPhone { get; set; }
        /// <summary>
        /// 分机号码
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 性别；0男，1女
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 禁用状态;0启用，1禁用，默认0启用
        /// </summary>
        public int Disabled { get; set; }

    }
}
