using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Util.CQuery;
using DDD.Util.Paging;
using DDD.Util.Domain.Aggregation;
using DDD.Util.Extension;
using DDD.Util.Command;
using DDD.Util.IoC;
using DDD.Util;
using DDD.Util.DataAccess;

namespace DDD.Util.Domain.Repository
{
    /// <summary>
    /// Default Repository
    /// </summary>
    public abstract class DefaultRepository<DT, ET, DAI> : BaseRepository<DT> where DT : IAggregationRoot<DT> where ET : CommandEntity<ET> where DAI : IDataAccess<ET>
    {
        protected IDataAccess<ET> dataAccess = ContainerManager.Resolve<DAI>();

        public DefaultRepository()
        {
            BindEvent();
        }

        #region Propertys

        public event DataChange<DT> RemoveEvent;//Remove Event
        public event DataChange<DT> SaveEvent; //Save Event
        public event QueryData<DT> QueryEvent;//Query Event

        #endregion

        #region Impl Methods

        /// <summary>
        /// Save Objects
        /// </summary>
        /// <param name="objDatas">Objects</param>
        public sealed override void Save(params DT[] objDatas)
        {
            SaveAsync(objDatas).Wait();
        }

        /// <summary>
        /// Save Objects
        /// </summary>
        /// <param name="objDatas">Objects</param>
        public sealed override async Task SaveAsync(params DT[] objDatas)
        {
            #region Verify Parameters

            if (objDatas == null || objDatas.Length <= 0)
            {
                throw new Exception("objDatas is null or empty");
            }
            foreach (var obj in objDatas)
            {
                if (obj == null)
                {
                    throw new Exception("save object data is null");
                }
                if (!obj.CanBeSave)
                {
                    throw new Exception("object data cann't to be save");
                }
            }

            #endregion

            await ExecuteSaveAsync(objDatas).ConfigureAwait(false);//Execute Save

            SaveEvent?.Invoke(objDatas);//Execute Save Event

            #region After Save Operations

            foreach (var data in objDatas)
            {
                data.MarkStored();//Mark Object Has Stored
            }

            #endregion
        }

        /// <summary>
        /// Remove Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        public sealed override void Remove(params DT[] objDatas)
        {
            RemoveAsync(objDatas).Wait();
        }

        /// <summary>
        /// Remove Objects
        /// </summary>
        /// <param name="objDatas">objects</param>
        public sealed override async Task RemoveAsync(params DT[] objDatas)
        {
            #region Verify Parameters

            if (objDatas == null || objDatas.Length <= 0)
            {
                throw new Exception("objDatas is null or empty");
            }
            foreach (var obj in objDatas)
            {
                if (obj == null)
                {
                    throw new Exception("remove object data is null");
                }
                if (!obj.CanBeRemove)
                {
                    throw new Exception("object data cann't to be remove");
                }
            }

            #endregion

            await ExecuteRemoveAsync(objDatas).ConfigureAwait(false);//execute remove
            RemoveEvent?.Invoke(objDatas);//execute remove event

            #region After Remove Operations

            foreach (var data in objDatas)
            {
                data.MarkRemove();//Mark Object Has Removed
            }

            #endregion
        }

        /// <summary>
        /// Remove Object
        /// </summary>
        /// <param name="query">query model</param>
        public sealed override void Remove(IQuery query)
        {
            RemoveAsync(query).Wait();
        }

        /// <summary>
        /// Remove Object
        /// </summary>
        /// <param name="query">query model</param>
        public sealed override async Task RemoveAsync(IQuery query)
        {
            await ExecuteRemoveAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Modify Object
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        public sealed override void Modify(IModify expression, IQuery query)
        {
            ModifyAsync(expression, query).Wait();
        }

        /// <summary>
        /// Modify Object
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        public sealed override async Task ModifyAsync(IModify expression, IQuery query)
        {
            await ExecuteModifyAsync(expression, query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Object
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>Object</returns>
        public sealed override DT Get(IQuery query)
        {
            return GetAsync(query).Result;
        }

        /// <summary>
        /// Get Object
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>Object</returns>
        public sealed override async Task<DT> GetAsync(IQuery query)
        {
            var data = await GetDataAsync(query).ConfigureAwait(false);
            if (QueryEvent == null)
            {
                return data;
            }
            IEnumerable<DT> dataList = new List<DT>() { data };
            QueryEvent?.Invoke(ref dataList);
            return dataList.FirstOrDefault();
        }

        /// <summary>
        /// Get Object List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        public sealed override List<DT> GetList(IQuery query)
        {
            return GetListAsync(query).Result;
        }

        /// <summary>
        /// Get Object List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object list</returns>
        public sealed override async Task<List<DT>> GetListAsync(IQuery query)
        {
            IEnumerable<DT> datas = await GetDataListAsync(query).ConfigureAwait(false);
            if (QueryEvent == null)
            {
                return datas.ToList();
            }
            QueryEvent?.Invoke(ref datas);
            return datas.ToList();
        }

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object paging</returns>
        //public sealed override IPaging<DT> GetPaging(IQuery query)
        //{
        //    return GetPagingAsync(query).Result;
        //}

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns>object paging</returns>
        //public sealed override async Task<IPaging<DT>> GetPagingAsync(IQuery query)
        //{
        //    var paging = await GetDataPagingAsync(query).ConfigureAwait(false);
        //    if (QueryEvent == null)
        //    {
        //        return paging;
        //    }
        //    IEnumerable<DT> datas = paging;
        //    QueryEvent?.Invoke(ref datas);
        //    return new Paging<DT>(paging.Page, paging.PageSize, paging.TotalCount, datas);
        //}

        /// <summary>
        /// Wheather Has Any Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        public sealed override bool Exist(IQuery query)
        {
            return ExistAsync(query).Result;
        }

        /// <summary>
        /// Wheather Has Any Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        public sealed override async Task<bool> ExistAsync(IQuery query)
        {
            return await IsExistAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        public sealed override long Count(IQuery query)
        {
            return CountAsync(query).Result;
        }

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        public sealed override async Task<long> CountAsync(IQuery query)
        {
            return await CountValueAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>max value</returns>
        public sealed override VT Max<VT>(IQuery query)
        {
            return MaxAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>max value</returns>
        public sealed override async Task<VT> MaxAsync<VT>(IQuery query)
        {
            return await MaxValueAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Min Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        public sealed override VT Min<VT>(IQuery query)
        {
            return MinAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Min Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        public sealed override async Task<VT> MinAsync<VT>(IQuery query)
        {
            return await MinValueAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>sum value</returns>
        public sealed override VT Sum<VT>(IQuery query)
        {
            return SumAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>sum value</returns>
        public sealed override async Task<VT> SumAsync<VT>(IQuery query)
        {
            return await SumValueAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>average value</returns>
        public sealed override VT Avg<VT>(IQuery query)
        {
            return AvgAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="DT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>average value</returns>
        public sealed override async Task<VT> AvgAsync<VT>(IQuery query)
        {
            return await AvgValueAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        protected virtual void BindEvent()
        {

        }

        #endregion

        #region Functions

        /// <summary>
        /// Execute Save
        /// </summary>
        /// <param name="objDatas">objects</param>
        protected virtual void ExecuteSave(params DT[] objDatas)
        {
            ExecuteSaveAsync(objDatas).Wait();
        }

        /// <summary>
        /// Execute Save
        /// </summary>
        /// <param name="objDatas">objects</param>
        protected virtual async Task ExecuteSaveAsync(params DT[] objDatas)
        {
            if (objDatas == null || objDatas.Length <= 0)
            {
                return;
            }
            foreach (var data in objDatas)
            {
                await SaveAsync(data).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Execute Remove
        /// </summary>
        /// <param name="objDatas">objects</param>
        protected virtual void ExecuteRemove(params DT[] objDatas)
        {
            ExecuteRemoveAsync(objDatas).Wait();
        }

        /// <summary>
        /// Execute Remove
        /// </summary>
        /// <param name="objDatas">objects</param>
        protected virtual async Task ExecuteRemoveAsync(params DT[] objDatas)
        {
            IEnumerable<ET> entityList = objDatas.Select(c => c.MapTo<ET>());
            await RemoveAsync(entityList).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute Remove
        /// </summary>
        /// <param name="query">query model</param>
        protected virtual void ExecuteRemove(IQuery query)
        {
            ExecuteRemoveAsync(query).Wait();
        }

        /// <summary>
        /// Execute Remove
        /// </summary>
        /// <param name="query">query model</param>
        protected virtual async Task ExecuteRemoveAsync(IQuery query)
        {
            if (query == null)
            {
                query = QueryFactory.Create();
            }
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} isn't set primary keys", entityType.FullName));
            }
            var dataList = GetList(query);
            if (dataList == null || dataList.Count <= 0)
            {
                return;
            }
            await RemoveAsync(dataList.ToArray()).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual DT GetData(IQuery query)
        {
            return GetDataAsync(query).Result;
        }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<DT> GetDataAsync(IQuery query)
        {
            var entityData = await dataAccess.GetAsync(query).ConfigureAwait(false);
            DT data = default(DT);
            if (entityData != null)
            {
                data = entityData.MapTo<DT>();
            }
            return data;
        }

        /// <summary>
        /// Get Data List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual List<DT> GetDataList(IQuery query)
        {
            return GetDataListAsync(query).Result;
        }

        /// <summary>
        /// Get Data List
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<List<DT>> GetDataListAsync(IQuery query)
        {
            var entityDataList = await dataAccess.GetListAsync(query).ConfigureAwait(false);
            if (entityDataList.IsNullOrEmpty())
            {
                return new List<DT>(0);
            }
            return entityDataList.Select(c => c.MapTo<DT>()).ToList();
        }

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual IPaging<DT> GetDataPaging(IQuery query)
        {
            return GetDataPagingAsync(query).Result;
        }

        /// <summary>
        /// Get Object Paging
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<IPaging<DT>> GetDataPagingAsync(IQuery query)
        {
            var entityPaging = await dataAccess.GetPagingAsync(query).ConfigureAwait(false);
            return entityPaging.ConvertTo<DT>();
        }

        /// <summary>
        /// Check Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual bool IsExist(IQuery query)
        {
            return IsExistAsync(query).Result;
        }

        /// <summary>
        /// Check Data
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExistAsync(IQuery query)
        {
            return await dataAccess.ExistAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual long CountValue(IQuery query)
        {
            return CountValueAsync(query).Result;
        }

        /// <summary>
        /// Get Data Count
        /// </summary>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<long> CountValueAsync(IQuery query)
        {
            return await dataAccess.CountAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="VT">Data Type</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual VT MaxValue<VT>(IQuery query)
        {
            return MaxValueAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Max Value
        /// </summary>
        /// <typeparam name="VT">Data Type</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<VT> MaxValueAsync<VT>(IQuery query)
        {
            return await dataAccess.MaxAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// get Min Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        protected virtual VT MinValue<VT>(IQuery query)
        {
            return MinValueAsync<VT>(query).Result;
        }

        /// <summary>
        /// get Min Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns>min value</returns>
        protected virtual async Task<VT> MinValueAsync<VT>(IQuery query)
        {
            return await dataAccess.MinAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual VT SumValue<VT>(IQuery query)
        {
            return SumValueAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Sum Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<VT> SumValueAsync<VT>(IQuery query)
        {
            return await dataAccess.SumAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual VT AvgValue<VT>(IQuery query)
        {
            return AvgValueAsync<VT>(query).Result;
        }

        /// <summary>
        /// Get Average Value
        /// </summary>
        /// <typeparam name="VT">DataType</typeparam>
        /// <param name="query">query model</param>
        /// <returns></returns>
        protected virtual async Task<VT> AvgValueAsync<VT>(IQuery query)
        {
            return await dataAccess.AvgAsync<VT>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute Modify
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        protected virtual void ExecuteModify(IModify expression, IQuery query)
        {
            ExecuteModifyAsync(expression, query).Wait();
        }

        /// <summary>
        /// Execute Modify
        /// </summary>
        /// <param name="expression">modify expression</param>
        /// <param name="query">query model</param>
        protected virtual async Task ExecuteModifyAsync(IModify expression, IQuery query)
        {
            UnitOfWork.UnitOfWork.RegisterCommand(await dataAccess.ModifyAsync(expression, query).ConfigureAwait(false));
        }

        #endregion

        #region Util

        /// <summary>
        /// Save Object
        /// </summary>
        /// <param name="data">data</param>
        async Task SaveAsync(DT data)
        {
            if (data.SaveByAdd)
            {
                await AddAsync(data).ConfigureAwait(false);
            }
            else
            {
                await ModifyAsync(data).ConfigureAwait(false);
            }
        }

        #region Add Object

        /// <summary>
        /// Add Object
        /// </summary>
        /// <param name="data">data</param>
        protected async Task AddAsync(DT data)
        {
            if (data.LifeStatus == Aggregation.LifeStatus.New || data.LifeStatus == Aggregation.LifeStatus.Remove)
            {
                ET entity = data.MapTo<ET>();
                await AddAsync(entity).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Add Objects
        /// </summary>
        /// <param name="datas">datas</param>
        protected async Task AddAsync(params ET[] datas)
        {
            UnitOfWork.UnitOfWork.RegisterCommand((await dataAccess.AddAsync(datas).ConfigureAwait(false)).ToArray());
        }

        #endregion

        #region Modify Objects

        /// <summary>
        ///Modify
        /// </summary>
        /// <param name="data">数据</param>
        protected async Task ModifyAsync(DT data)
        {
            if (data.LifeStatus == Aggregation.LifeStatus.Stored || data.LifeStatus == Aggregation.LifeStatus.Modify)
            {
                ET entity = data.MapTo<ET>();
                await ModifyAsync(entity).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Modify Objects
        /// </summary>
        /// <param name="datas">datas</param>
        protected async Task ModifyAsync(params ET[] datas)
        {
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} is not set primary keys", entityType.FullName));
            }
            foreach (var data in datas)
            {
                IQuery query = QueryFactory.Create();
                foreach (var key in keys)
                {
                    query.Equal(key, data.GetPropertyValue(key));
                }
                UnitOfWork.UnitOfWork.RegisterCommand(await dataAccess.ModifyAsync(data, query).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Remove Objects
        /// </summary>
        /// <param name="entityList">datas</param>
        protected async Task RemoveAsync(IEnumerable<ET> entityList)
        {
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} isn't set primary keys", entityType.FullName));
            }
            IQuery query = QueryFactory.Create();
            List<dynamic> keyValueList = new List<dynamic>();
            foreach (ET entity in entityList)
            {
                if (keys.Count == 1)
                {
                    keyValueList.Add(entity.GetPropertyValue(keys.ElementAt(0)));
                }
                else
                {
                    IQuery entityQuery = QueryFactory.Create();
                    foreach (var key in keys)
                    {
                        entityQuery.And(key, CriteriaOperator.Equal, entity.GetPropertyValue(key));
                    }
                    query.Or(entityQuery);
                }
            }
            if (keys.Count == 1)
            {
                query.In(keys.ElementAt(0), keyValueList);
            }
            UnitOfWork.UnitOfWork.RegisterCommand(await dataAccess.DeleteAsync(query).ConfigureAwait(false));
        }

        #endregion

        #endregion
    }
}
