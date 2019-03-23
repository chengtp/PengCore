using DDD.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DDD.Infrastructure
{
    /// <summary>
    /// 链接工厂，切换数据库
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        /// <returns></returns>
        private static DatabaseType GetDataBaseType(string databaseType)
        {
            DatabaseType returnValue = DatabaseType.SqlServer;
            foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
            {
                if (dbType.ToString().Equals(databaseType, StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = dbType;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            IDbConnection connection = null;

            //获取配置进行转换
            var type = AppSetting.GetConfig("ComponentDbType");
            var dbType = GetDataBaseType(type);

            //DefaultDatabase 根据这个配置项获取对应连接字符串
            var database = AppSetting.GetConfig("DefaultDatabase");
            if (string.IsNullOrEmpty(database))
            {
                database = "sqlserver";//默认配置
            }
          //  var strConn = AppConfig.Configuration.GetConnectionString(database);
            var strConn = AppSetting.GetConnectionString(database);
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(strConn);
                    break;
                case DatabaseType.MySql:
                  //  connection = new MySql.Data.MySqlClient.MySqlConnection(strConn);
                    break;
                case DatabaseType.Npgsql:
                 //   connection = new Npgsql.NpgsqlConnection(strConn);
                    break;
                case DatabaseType.Sqlite:
                   // connection = new SQLiteConnection(strConn);
                    break;
                case DatabaseType.Oracle:
                   // connection = new Oracle.ManagedDataAccess.Client.OracleConnection(strConn);
                    //connection = new System.Data.OracleClient.OracleConnection(strConn);
                    break;
                case DatabaseType.DB2:
                    //connection = new System.Data.OleDb.OleDbConnection(strConn);
                    break;
            }

            return connection;
        }


    }
}
