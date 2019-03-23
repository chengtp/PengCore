using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.Config
{
    /// <summary>
    /// 配置文件中读取配置数据
    /// </summary>
    public class ApplicationConfiguration
    {
        public string DefalutDatabase { get; set; }
        public string ComponentDbType { get; set; }
        public string Sqlserver { get; set; }
    }
}
