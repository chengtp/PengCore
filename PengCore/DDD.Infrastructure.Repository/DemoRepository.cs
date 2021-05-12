using Dapper;
using Dapper.Contrib.Extensions;
using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using DDD.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 测试仓储
    /// </summary>
    public class DemoRepository : BaseRepository<T_Demo>, IDemoRepository
    {
        private DapperDBContext context;
        private readonly IOptions<Infrastructure.Dtos.Config.AppSettings> appSettings; //配置文件数据
        public DemoRepository(DapperDBContext _context, IOptions<Infrastructure.Dtos.Config.AppSettings> _appSettings)
            : base(_context)
        {
            context = _context;
            appSettings = _appSettings;
        }

        /// <summary>
        /// 通过sql 语句查询一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T_Demo> GetModelBysql(Guid Id, string userName)
        {

            DDD.Common.SqlModel sqlModel = DDD.Common.SerializationHelper.XmlFileToStringSql("Demo.xml", "GetModel");


            //DynamicParameters param = new DynamicParameters();
            //param.Add("@Id", Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);

            #region 参数

            Dapper.DynamicParameters param = new Dapper.DynamicParameters();

            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            dict.Add(nameof(Id), Id);
            dict.Add(nameof(userName), userName);
            param = DDD.Common.StringHelper.GetParams(sqlModel, dict);



            ////得到sql中的参数集合
            //List<string> sqlParamList = DDD.Common.StringHelper.GetStringList(sqlModel.commandType,sqlModel.sqlStatement);
            ////遍历xml中的参数集合
            //List<DDD.Common.SqlParameterModels> listparam = sqlModel.listParameter;
            //if (listparam != null && listparam.Any())
            //{
            //    foreach (DDD.Common.SqlParameterModels item in listparam)
            //    {
            //        if (item.property == nameof(Id))
            //        {
            //            //处理sql中的参数问题
            //            sqlModel.sqlStatement = DDD.Common.StringHelper.GetStringSql(sqlParamList, sqlModel.sqlStatement, item.property);
            //            param.Add(item.column, Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
            //        }
            //    }
            //}
            #endregion


            return await QueryFirstOrDefaultAsync(sqlModel.sqlStatement.Trim(), param);
        }

        /// <summary>
        /// 查询分页查询所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<T_Demo>> GetModels()
        {
            PagedRequest request = new PagedRequest()
            {

            };

            //var test=  context_PMS._connection.Query<string>("SELECT Password FROM dbo.ss_User WHERE LoginName ='admin' ");
            var test = appSettings.Value;
            IDbConnection connectionPMS = new SqlConnection("");
            // connectionPMS.Open();
          

            return await QueryPageAsync(request);

            //  return await context._connection.GetAllAsync<T_Demo>();
        }



    }
}
