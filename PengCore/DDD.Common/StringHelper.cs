using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDD.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// 通过sql 字符串 ，得到参数集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<string> GetStringList(SqlModel sqlModel)
        {
            List<string> sqlParamList = new List<string>();
            if (sqlModel != null)
            {
                if (sqlModel.oprationType != Common.OperationType.procedure)
                {
                    string Regexstr = @"#.*?#";
                    MatchCollection mc = Regex.Matches(sqlModel.sqlStatement, Regexstr, RegexOptions.IgnoreCase);

                    foreach (Match item in mc)
                    {
                        if (!sqlParamList.Exists(m => m == item.Value))
                        {
                            sqlParamList.Add(item.Value);
                        }
                    }
                }
            }
            return sqlParamList;
        }

        /// <summary>
        /// sql 语句处理
        /// </summary>
        /// <param name="sqlParamList">参数集合</param>
        /// <param name="sql">sql语句处理</param>
        /// <param name="property">变量</param>
        /// <returns></returns>
        public static string GetStringSql(List<string> sqlParamList, string sql, string property)
        {
            if (sqlParamList != null && sqlParamList.Any())
            {
                string paramSingle = sqlParamList.FirstOrDefault(m => m == "#" + property + "#");
                string paramName = "@" + paramSingle.Substring(1, paramSingle.Length - 2);
                sql = sql.Replace(paramSingle, paramName);
            }
            return sql;
        }
    }
}
