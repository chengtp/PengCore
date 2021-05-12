using Dapper;
using Dapper.Contrib.Extensions;
using DDD.Common;
using DDD.Domain.DomainModel;
using DDD.Infrastructure.Dtos.PageList;
using DDD.Repository.Interfaces.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : AggregateRoot
    {
        private readonly DapperDBContext _context;
        public BaseRepository(DapperDBContext context)
        {
            _context = context;
        }

        #region Dapper Execute & Query
        /// <summary>
        /// sql 增删改  操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            return await _context._connection.ExecuteAsync(sql, param, _context._transaction, commandTimeout, commandType);
        }
        /// <summary>
        /// 实体新增记录
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <param name="_commandTimeout"></param>
        /// <param name="sqlAdapter"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertAsync(TEntity entityToInsert, int? commandTimeout = null, ISqlAdapter sqlAdapter = null)
        {
            return await _context._connection.InsertAsync<TEntity>(entityToInsert, _context._transaction, commandTimeout, sqlAdapter);
        }
        /// <summary>
        ///  实体修改记录
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <param name="_commandTimeout"></param>
        /// <param name="sqlAdapter"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entityToUpdate, int? commandTimeout = null)
        {
            return await _context._connection.UpdateAsync<TEntity>(entityToUpdate, _context._transaction, commandTimeout);
        }
        /// <summary>
        ///  实体删除记录
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <param name="_commandTimeout"></param>
        /// <param name="sqlAdapter"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TEntity entityToDelete, int? commandTimeout = null)
        {
            return await _context._connection.DeleteAsync<TEntity>(entityToDelete, _context._transaction, commandTimeout);
        }
        /// <summary>
        ///  实体删除所有记录
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <param name="_commandTimeout"></param>
        /// <param name="sqlAdapter"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAllAsync(TEntity entityToDelete, int? commandTimeout = null)
        {
            return await _context._connection.DeleteAllAsync<TEntity>(_context._transaction, commandTimeout);
        }


        /// <summary>
        /// sql查询集合对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            return await _context._connection.QueryAsync<TEntity>(sql, param, _context._transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// sql查询 分页集合对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<TEntity>> QueryPageAsync(PagedRequest request, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var type = typeof(TEntity);

            var para = new Dapper.DynamicParameters();
            para.Add("TableName", Reflector.GetTableName(type), DbType.String, ParameterDirection.Input);
            para.Add("FieldsStr", "*", DbType.String, ParameterDirection.Input);
            para.Add("OrderString", request.Order, DbType.String, ParameterDirection.Input);
            para.Add("PageIndex", request.PageIndex, DbType.Int16, ParameterDirection.Input);
            para.Add("PageSize", request.PageSize, DbType.Int16, ParameterDirection.Input);
            para.Add("TotalRecord", request.PageSize, DbType.Int64, ParameterDirection.Output);
            para.AddDynamicParams(param);

            var DataList = await _context._connection.QueryAsync<TEntity>("proc_PageList", para, _context._transaction, commandTimeout, CommandType.StoredProcedure);

            var TotalRecord = para.Get<long>("TotalRecord");

            PagedResult<TEntity> entities = new PagedResult<TEntity>
            {
                Data = DataList.ToList(),
                TotalRecords = TotalRecord,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalPages = ((int)TotalRecord + request.PageSize - 1) / request.PageSize
            };
            return entities;

        }

        /// <summary>
        /// 实体查询所有记录
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int? commandTimeout = null)
        {
            return await _context._connection.GetAllAsync<TEntity>(_context._transaction, commandTimeout);
        }
        
        /// <summary>
        /// 通过ssql查询多个实体集合数据
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            return await _context._connection.QueryMultipleAsync(sql, param, _context._transaction, commandTimeout, commandType);
        }



        /// <summary>
        /// 查询单条实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> QueryFirstOrDefaultAsync(string sql, object param = null, int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            return await _context._connection.QueryFirstOrDefaultAsync<TEntity>(sql, param, _context._transaction, commandTimeout, commandType);
        }
        /// <summary>
        /// 通过Id，查询单条实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Get(Guid id, int? commandTimeout = null)
        {
            return await _context._connection.GetAsync<TEntity>(id, _context._transaction, commandTimeout);
        }



        /// <summary>
        /// 通过sql 查询集合
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            return await _context._connection.QueryAsync(sql, map, param, _context._transaction, true, splitOn, commandTimeout, commandType);
        }





        #endregion
    }
}
