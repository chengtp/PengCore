using DDD.Infrastructure.Dtos;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Interfaces
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserInfoService
    {

        Task<UserInfoLoginOut> GetModelByLogin(UserInfoLoginInput model);
        Task<PagedResult<UserInfoOutput>> GetModels();

        Task<PagedResult<UserInfoOutput>> GetModelsBy();
        Task<UserInfoOutput> GetModelById();

        Task<bool> CreateModel();

        Task<bool> UpdateModel();

    }
}
