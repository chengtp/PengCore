using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityClient.Data
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {

        }

        #region 实体集

        public DbSet<Sys_UserInfo> Sys_UserInfo { get; set; }//注意 这里这个Blog不能写成Blogs否则会报错找不到Blogs 因为我们现在数据库和表是现成的 这里就相当于实体对应的数据库是Blog

        #endregion
    }
}
