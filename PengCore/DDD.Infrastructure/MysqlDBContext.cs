using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
namespace DDD.Infrastructure
{
    /// <summary>
    ///   Mysql 数据上下文
    /// </summary>
    public class MysqlDBContext : DapperDBContext
    {
        public MysqlDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor) { }
        protected override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
    }
}
