using DDD.Infrastructure.Dtos;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Application.Interfaces
{
    /// <summary>
    /// 暴露到外面的测试服务类
    /// </summary>
    public interface IDemoService
    {
        Task<PagedResult<DemoOutput>> GetModels();
        /// <summary>
        /// 通过sql 语句查询一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DemoOutput> GetModelBysql(Guid Id,string userName);
    }
}
