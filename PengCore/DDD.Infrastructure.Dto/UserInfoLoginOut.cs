using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos
{
    public class UserInfoLoginOut
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Id { get; set; }
        
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
    }
}
