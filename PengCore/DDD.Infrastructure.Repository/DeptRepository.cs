using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 部门仓储数据
    /// </summary>
    public class DeptRepository : BaseRepository<Domain.Sys_Dept>, DDD.Repository.Interfaces.IDeptRepository
    {
        private DapperDBContext _context;
        public DeptRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
