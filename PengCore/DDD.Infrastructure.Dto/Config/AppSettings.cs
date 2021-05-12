using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.Config
{
    /// <summary>
    /// 配置字段数据
    /// </summary>
    public class AppSettings
    {
        public string sqlserver_PMS { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
    }
}
