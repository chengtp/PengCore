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
        public static List<string> GetStringList(Common.OperationType oprationType, string sqlStatement)
        {
            List<string> sqlParamList = new List<string>();

            if (oprationType != Common.OperationType.procedure && !string.IsNullOrEmpty(sqlStatement))
            {
                string Regexstr = @"#.*?#";
                MatchCollection mc = Regex.Matches(sqlStatement, Regexstr, RegexOptions.IgnoreCase);

                foreach (Match item in mc)
                {
                    if (!sqlParamList.Exists(m => m == item.Value))
                    {
                        sqlParamList.Add(item.Value);
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
                if (!string.IsNullOrEmpty(paramSingle))
                {
                    string paramName = "@" + paramSingle.Substring(1, paramSingle.Length - 2);
                    sql = sql.Replace(paramSingle, paramName);
                }
            }
            return sql;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlModel">对象</param>
        /// <param name="dict">key 为名称，value为值</param>
        /// <returns></returns>
        public static Dapper.DynamicParameters GetParams(DDD.Common.SqlModel sqlModel, Dictionary<string, dynamic> dict)
        {
            Dapper.DynamicParameters param = new Dapper.DynamicParameters();


            //得到sql中的参数集合
            List<string> sqlParamList = DDD.Common.StringHelper.GetStringList(sqlModel.commandType, sqlModel.sqlStatement);
            //遍历xml中的参数集合
            List<DDD.Common.SqlParameterModels> listparam = sqlModel.listParameter;
            if (listparam != null && listparam.Any())
            {
                foreach (DDD.Common.SqlParameterModels item in listparam)
                {
                    var dictModel = dict.FirstOrDefault(m => m.Key == item.property);
                    if (!string.IsNullOrEmpty(dictModel.Key))
                    {
                        //处理sql中的参数问题
                        sqlModel.sqlStatement = DDD.Common.StringHelper.GetStringSql(sqlParamList, sqlModel.sqlStatement, item.property);
                        param.Add(item.column, dictModel.Value, (System.Data.DbType)Enum.Parse(typeof(System.Data.DbType), item.propertyType, true), System.Data.ParameterDirection.Input);
                    }
                }
            }


            return param;

        }

    }
}
