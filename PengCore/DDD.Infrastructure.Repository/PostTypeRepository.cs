using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 岗位类型仓储数据
    /// </summary>
    public class PostTypeRepository : BaseRepository<Domain.Sys_PostType>, DDD.Repository.Interfaces.IPostTypeRepository
    {
        private DapperDBContext _context;
        public PostTypeRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
