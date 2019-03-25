using Dapper;
using DDD.Common;
using DDD.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Repository.Interfaces.BaseInterfaces
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot
    {
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<int> InsertAsync(TEntity entityToInsert, int? commandTimeout = null, ISqlAdapter sqlAdapter = null);
        Task<bool> UpdateAsync(TEntity entityToUpdate, int? commandTimeout = null);
        Task<bool> DeleteAsync(TEntity entityToDelete, int? commandTimeout = null);
        Task<bool> DeleteAllAsync(TEntity entityToDelete, int? commandTimeout = null);

        Task<IEnumerable<TEntity>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<PagedResult<TEntity>> QueryPageAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<IEnumerable<TEntity>> GetAllAsync(int? commandTimeout = null);
        Task<PagedResult<TEntity>> GetAllPageAsync(int? commandTimeout = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<TEntity> QueryFirstOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<TEntity> Get(Guid id, int? commandTimeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", int? commandTimeout = null, CommandType commandType = CommandType.Text);


    }
}
