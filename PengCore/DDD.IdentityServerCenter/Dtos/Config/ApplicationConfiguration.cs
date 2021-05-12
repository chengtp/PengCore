using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityServerCenter.Dtos.Config
{
    /// <summary>
    /// 配置文件中读取配置数据
    /// </summary>
    public class ApplicationConfiguration
    {
        public string DefalutDatabase { get; set; }
        public string ComponentDbType { get; set; }
        public ConnectionString ConnectionStr { get; set; }
    }

    public class ConnectionString
    {
        public string sqlserver { get; set; }
        public string mysql { get; set; }
        public string oracle { get; set; }
    }
}
