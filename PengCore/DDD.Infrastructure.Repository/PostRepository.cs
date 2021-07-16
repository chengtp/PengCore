using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Repository
{
    /// <summary>
    /// 岗位仓储数据
    /// </summary>
    public class PostRepository : BaseRepository<Domain.Sys_Post>, DDD.Repository.Interfaces.IPostRepository
    {
        private DapperDBContext _context;
        public PostRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
