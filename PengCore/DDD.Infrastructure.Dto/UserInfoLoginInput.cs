using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Application.Dtos
{
    public class UserInfoLoginInput
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
       
    }
}
