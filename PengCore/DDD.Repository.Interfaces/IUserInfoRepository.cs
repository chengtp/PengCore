using DDD.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Repository.Interfaces
{
    public interface IUserInfoRepository:BaseInterfaces.IRepository<Sys_UserInfo>
    {
        Task<IEnumerable<Sys_UserInfo>> GetModels();
    }
}
