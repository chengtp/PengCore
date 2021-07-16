using DDD.Util.CQuery;
using DDD.Util.Paging;
using DDD.Util.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Domain.Repository
{
    /// <summary>
    /// IRepository
    /// </summary>
    /// <typeparam name="T">IAggregationRoot</typeparam>
    public interface IRepository<T> where T : IAggregationRoot<T>
    {
        #region Propertys

        /// <summary>
        /// Remove Event
        /// </summary>
        event DataChange<T> RemoveEvent;

        /// <summary>
        /// Save Event
        /// </summary>
        event DataChange<T> SaveEvent;

        /// <summary>
        /// Query Event
        /// </summary>
        event QueryData<T> QueryEvent;

        #endregion

        #region Methods

        /// <summary>
        /// Save Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        void Save(params T[] objDatas);

        /// <summary>
        /// Save Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        Task SaveAsync(params T[] objDatas);

        /// <summary>
        /// Remove Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        void Remove(params T[] objDatas);

        /// <summary>
        /// Remove Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        Task RemoveAsync(params T[] objDatas);

        /// <summary>
        /// Remove Object
        /// </summary>
        /// <param name="query">query model</param>
        void Remove(IQuery query);

        /// <summary>
        /// Remove Object
        /// </summary>
        /// <param name="query">query model</param>
        Task RemoveAsync(IQuery query);

        /// <summary>
        /// Modify Object
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        void Modify(IModify expression, IQuery query);

        /// <summary>
        /// Modify Object
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        Task ModifyAsync(IModify expression, IQuery query);

        /// <summary>
        /// Get Object
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object</returns>
        T Get(IQuery query);

        /// <summary>
        /// Get Object
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object</returns>
        Task<T> GetAsync(IQuery query);

        /// <summary>
        /// Get Object List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        List<T> GetList(IQuery query);

        /// <summary>
        /// Get Object List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        Task<List<T>> GetListAsync(IQuery query);

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        IPaging<T> GetPaging(IQuery query);

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        Task<IPaging<T>> GetPagingAsync(IQuery query);

        /// <summary>
        /// exist data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        bool Exist(IQuery query);

        /// <summary>
        /// exist data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        Task<bool> ExistAsync(IQuery query);

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        long Count(IQuery query);

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        Task<long> CountAsync(IQuery query);

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>max value</returns>
        DT Max<DT>(IQuery query);

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>max value</returns>
        Task<DT> MaxAsync<DT>(IQuery query);

        /// <summary>
        /// Get Min Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        DT Min<DT>(IQuery query);

        /// <summary>
        /// Get Min Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        Task<DT> MinAsync<DT>(IQuery query);

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>sum value</returns>
        DT Sum<DT>(IQuery query);

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>sum value</returns>
        Task<DT> SumAsync<DT>(IQuery query);

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>average value</returns>
        DT Avg<DT>(IQuery query);

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>average value</returns>
        Task<DT> AvgAsync<DT>(IQuery query);

        #endregion
    }
}
