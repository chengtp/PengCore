using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace DDD.Infrastructure
{
    /// <summary>
    ///  Sqlserver 数据上下文
    /// </summary>
    public class SqlserverDBContext : DapperDBContext
    {
        public SqlserverDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor) { }
        protected override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
