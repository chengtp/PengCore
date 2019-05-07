using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DDD.Common
{
    /// <summary>
    /// 数据库类型定义
    /// </summary>
    public enum DatabaseType
    {
        SqlServer,  //SQLServer数据库
        MySql,      //Mysql数据库
        Npgsql,     //PostgreSQL数据库
        Oracle,     //Oracle数据库
        Sqlite,     //SQLite数据库
        DB2         //IBM DB2数据库
    }

    //数据操作类型 
    public enum OperationType
    {
        [Description("select")]
        select = 0,
        [Description("insert")]
        insert = 1,
        [Description("update")]
        update = 2,
        [Description("delete")]
        delete = 3,
        [Description("procedure")]
        procedure = 4

    }

}
