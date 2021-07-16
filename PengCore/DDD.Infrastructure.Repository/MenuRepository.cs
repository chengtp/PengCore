using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 菜单仓储数据
    /// </summary>
    public class MenuRepository : BaseRepository<Domain.Sys_Menu>, DDD.Repository.Interfaces.IMenuRepository
    {
        private DapperDBContext _context;
        public MenuRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
