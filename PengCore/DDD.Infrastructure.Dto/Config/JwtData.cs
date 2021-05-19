using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.Config
{
    public class JwtData
    {
        public DateTime expire { get; set; }  //代表过期时间

        public string userId { get; set; }

        public string userAccount { get; set; }
    }
}
