using DDD.IdentityServerCenter.Data;
using DDD.IdentityServerCenter.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityServerCenter.Services
{
    public class Sys_UserInfoService : BaseService<Sys_UserInfo>, ISys_UserInfoService
    {
        public Sys_UserInfoService(EFContext _efContext)
        {
            db = _efContext;
        }

        public async Task<Sys_UserInfo> GetByStr(string username, string pwd)
        {
            Sys_UserInfo m = await db.Sys_UserInfo.Where(a => a.LoginName == username && a.Password == pwd).SingleOrDefaultAsync();
            if (m != null)
            {
                return m;
            }
            else
            {
                return null;
            }
        }
    }
}
