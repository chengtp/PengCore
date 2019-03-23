using DDD.Infrastructure.Interfaces;

namespace DDD.Infrastructure
{
    /// <summary>
    /// 事务工厂
    /// </summary>
    public class DapperUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DapperDBContext _context;

        public DapperUnitOfWorkFactory(DapperDBContext context)
        {
            _context = context;
        }

        public IUnitOfWork BeginTransaction()
        {
            return new UnitOfWork(_context);
        }
    }
}
