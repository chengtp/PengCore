namespace DDD.Infrastructure.Interfaces
{
    /// <summary>
    /// 定义工作单元工厂
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork BeginTransaction();
    }
}
