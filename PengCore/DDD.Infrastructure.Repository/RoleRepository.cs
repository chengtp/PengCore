using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 角色仓储数据
    /// </summary>
    public class RoleRepository : BaseRepository<Domain.Sys_Role>, DDD.Repository.Interfaces.IRoleRepository
    {
        private DapperDBContext _context;
        public RoleRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
