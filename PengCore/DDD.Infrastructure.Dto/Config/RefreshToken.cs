using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.Config
{
    public class RefreshToken
    {
        //主键
        public string id { get; set; }
        //用户编号
        public string userId { get; set; }
        //refreshToken
        public string Token { get; set; }
        //过期时间
        public DateTime expire { get; set; }
    }

}
