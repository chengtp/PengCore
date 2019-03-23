using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure
{
    /// <summary>
    /// 声明连接字符串
    /// </summary>
    public class DapperDBContextOptions : IOptions<DapperDBContextOptions>
    {
        public string Configuration { get; set; }

        DapperDBContextOptions IOptions<DapperDBContextOptions>.Value
        {
            get { return this; }
        }
    }
}
