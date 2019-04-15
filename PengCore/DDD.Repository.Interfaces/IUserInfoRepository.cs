using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Repository.Interfaces
{
    public interface IUserInfoRepository:BaseInterfaces.IRepository<Sys_UserInfo>
    {
        Task<PagedResult<Sys_UserInfo>> GetModels();
    }
}
