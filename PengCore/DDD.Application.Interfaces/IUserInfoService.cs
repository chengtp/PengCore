using DDD.Application.Dtos;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Interfaces
{
    public interface IUserInfoService
    {
        Task<PagedResult<UserInfoOutput>> GetModels();

        Task<UserInfoLoginOut> GetModelByLogin(UserInfoLoginInput model);
    }
}
