using Dapper;
using Dapper.Contrib.Extensions;
using DDD.Domain;
using DDD.Infrastructure.Dtos.PageList;
using DDD.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repository
{
    public class UserInfoRepository : BaseRepository<Sys_UserInfo>, IUserInfoRepository
    {
        private DapperDBContext _context;
        public UserInfoRepository(DapperDBContext context)
            : base(context)
        {
            _context = context;
        }

        public void Etesttt()
        {
            DapperUnitOfWorkFactory tttt = new DapperUnitOfWorkFactory(_context);
            IUnitOfWork unitOfWork = tttt.BeginTransaction();

            unitOfWork.Commit();
            unitOfWork.Rollback();




            _context.BeginTransaction();

            _context.Commit();
            _context.Rollback();


            _context._connection.DeleteAllAsync<string>(null, null);

        }

        public async Task<IEnumerable<Sys_UserInfo>> GetModels()
        {
            PagedRequest request = new PagedRequest()
            {

            };
            var list = await QueryPageAsync(request);

            return await _context._connection.GetAllAsync<Sys_UserInfo>();
        }
    }
}
