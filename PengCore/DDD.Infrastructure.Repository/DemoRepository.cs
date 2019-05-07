using Dapper;
using Dapper.Contrib.Extensions;
using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using DDD.Repository.Interfaces;
using System;
using System.Collections.Generic;
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
        private DapperDBContext _context;
        public DemoRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 通过sql 语句查询一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T_Demo> GetModelBysql(Guid Id)
        {

            DDD.Common.SqlModel sqlModel = DDD.Common.SerializationHelper.XmlFileToStringSql("Demo.xml", "GetModel");


            //DynamicParameters param = new DynamicParameters();
            //param.Add("@Id", Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);

            #region 参数

            Dapper.DynamicParameters param = new Dapper.DynamicParameters();
            //得到sql中的参数集合
            List<string> sqlParamList = DDD.Common.StringHelper.GetStringList(sqlModel);
            //遍历xml中的参数集合
            List<DDD.Common.SqlParameterModels> listparam = sqlModel.listParameter;
            if (listparam != null && listparam.Any())
            {
                Guid ddd = Id;
                Type type = sqlModel.GetType();
                var ss = type.GetProperties().FirstOrDefault(m=>m.Name=="id");
                var sss = ss.PropertyType.Name; //== System.Data.DbType.Guid   需要修改为 通用的方法

                foreach (DDD.Common.SqlParameterModels item in listparam)
                {
                    if (item.property == nameof(Id))
                    {
                        //处理sql中的参数问题
                        sqlModel.sqlStatement = DDD.Common.StringHelper.GetStringSql(sqlParamList, sqlModel.sqlStatement, item.property);
                        param.Add(item.column, Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
                    }
                }
            }
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
            return await QueryPageAsync(request);

            //  return await _context._connection.GetAllAsync<T_Demo>();
        }



    }
}
