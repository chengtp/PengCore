using DDD.IdentityServerCenter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityServerCenter.Interfaces
{
    public interface ISys_UserInfoService :IBaseService<Sys_UserInfo>
    {
        Task<Sys_UserInfo> GetByStr(string username, string pwd);
    }
}
