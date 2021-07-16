using DDD.Application.Dtos;
using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Repository.Interfaces
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserInfoRepository:BaseInterfaces.IRepository<Sys_UserInfo>
    {
        Task<PagedResult<Sys_UserInfo>> GetModels();

        Task<UserInfoLoginOut> GetModelByLogin(UserInfoLoginInput model);
    }
}
