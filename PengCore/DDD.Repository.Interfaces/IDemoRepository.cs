using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Repository.Interfaces
{
    public interface IDemoRepository : BaseInterfaces.IRepository<T_Demo>
    {
        Task<PagedResult<T_Demo>> GetModels();
        /// <summary>
        /// 通过sql 语句查询一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<T_Demo> GetModelBysql(Guid Id,string userName);
    }
}
