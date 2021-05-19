using DDD.Util.Command;
using DDD.Util.CQuery;
using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.DataAccess
{
    /// <summary>
    /// imeplements data access for rdb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RdbDataAccess<T> : IDataAccess<T> where T : CommandEntity<T>
    {
        #region add data

        /// <summary>
        /// add data
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>icommand</returns>
        public virtual ICommand Add(T obj)
        {
            return AddAsync(obj).Result;
        }

        /// <summary>
        /// add data
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>icommand</returns>
        public virtual async Task<ICommand> AddAsync(T obj)
        {
            InitVersionFieldValue(obj);//初始化版本值
            InitRefreshDateFieldValue(obj);//初始化刷新值
            var cmd = RdbCommand.CreateNewCommand(OperateType.Insert, obj);
            SetCommand(cmd, obj.PropertyValues);
            cmd.VerifyResult = a => a > 0;
            cmd.Fields = GetEditFields();
            return await Task.FromResult(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// add data list
        /// </summary>
        /// <param name="objList">object list</param>
        /// <returns>icommand list</returns>
        public virtual List<ICommand> Add(IEnumerable<T> objList)
        {
            return AddAsync(objList).Result;
        }

        /// <summary>
        /// add data list
        /// </summary>
        /// <param name="objList">object list</param>
        /// <returns>icommand list</returns>
        public virtual async Task<List<ICommand>> AddAsync(IEnumerable<T> objList)
        {
            if (objList == null)
            {
                return new List<ICommand>(0);
            }
            List<ICommand> cmdList = new List<ICommand>();
            foreach (var obj in objList)
            {
                cmdList.Add(await AddAsync(obj).ConfigureAwait(false));
            }
            return cmdList;
        }

        #endregion

        #region edit data

        /// <summary>
        /// edit data
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        public virtual ICommand Modify(T obj, IQuery query)
        {
            return ModifyAsync(obj, query).Result;
        }

        /// <summary>
        /// edit data
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        public virtual async Task<ICommand> ModifyAsync(T obj, IQuery query)
        {
            Dictionary<string, dynamic> modifyValues = obj.GetModifyValues();
            if (modifyValues == null || modifyValues.Count <= 0)
            {
                return null;
            }

            #region control version

            string versionFieldName = QueryConfig.GetVersionField(typeof(T));
            if (!string.IsNullOrWhiteSpace(versionFieldName))
            {
                if (!modifyValues.ContainsKey(versionFieldName))
                {
                    modifyValues.Add(versionFieldName, obj.PropertyValues[versionFieldName] + 1);
                }
                query = query ?? QueryFactory.Create();
                query.And(versionFieldName, CriteriaOperator.Equal, obj.PropertyValues[versionFieldName]);
            }

            #endregion

            #region update date

            string refreshFieldName = QueryConfig.GetRefreshDateField(typeof(T));
            if (!string.IsNullOrWhiteSpace(refreshFieldName))
            {
                if (!modifyValues.ContainsKey(refreshFieldName))
                {
                    modifyValues.Add(refreshFieldName, DateTime.Now);
                }
            }

            #endregion

            return await UpdateAsync(modifyValues.Keys, modifyValues, query).ConfigureAwait(false);
        }

        /// <summary>
        /// edit data with expression
        /// </summary>
        /// <param name="modifyExpression">modify expression</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        public virtual ICommand Modify(IModify modifyExpression, IQuery query)
        {
            return ModifyAsync(modifyExpression, query).Result;
        }

        /// <summary>
        /// edit data with expression
        /// </summary>
        /// <param name="modifyExpression">modify expression</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        public virtual async Task<ICommand> ModifyAsync(IModify modifyExpression, IQuery query)
        {
            Dictionary<string, dynamic> fieldAndValues = modifyExpression.GetModifyValues();
            if (fieldAndValues == null || fieldAndValues.Count <= 0)
            {
                return null;
            }

            #region control version

            string versionFieldName = QueryConfig.GetVersionField(typeof(T));
            if (!string.IsNullOrWhiteSpace(versionFieldName))
            {
                if (!fieldAndValues.ContainsKey(versionFieldName))
                {
                    fieldAndValues.Add(versionFieldName, new CalculateModify()
                    {
                        Calculate = CalculateOperator.Add,
                        Value = 1
                    });
                }
            }

            #endregion

            #region update date

            string refreshFieldName = QueryConfig.GetRefreshDateField(typeof(T));
            if (!string.IsNullOrWhiteSpace(refreshFieldName))
            {
                if (!fieldAndValues.ContainsKey(refreshFieldName))
                {
                    fieldAndValues.Add(refreshFieldName, DateTime.Now);
                }
            }

            #endregion

            return await UpdateAsync(fieldAndValues.Keys, fieldAndValues, query).ConfigureAwait(false);
        }

        /// <summary>
        /// edit value
        /// </summary>
        /// <param name="fields">fields</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        async Task<ICommand> UpdateAsync(IEnumerable<string> fields, object parameters, IQuery query)
        {
            var cmd = RdbCommand.CreateNewCommand(OperateType.Update);
            SetCommand(cmd, parameters as Dictionary<string, dynamic>);
            cmd.Fields = fields;
            cmd.Parameters = parameters;
            if (query.VerifyResult != null)
            {
                cmd.VerifyResult = query.VerifyResult;
            }
            else
            {
                cmd.VerifyResult = r => r > 0;
            }
            cmd.Query = query;
            return await Task.FromResult(cmd);
        }

        #endregion

        #region delete data

        /// <summary>
        /// delete data
        /// </summary>
        /// <param name="query">delete query</param>
        /// <returns>ICommand object</returns>
        public virtual ICommand Delete(IQuery query)
        {
            return DeleteAsync(query).Result;
        }

        /// <summary>
        /// delete data
        /// </summary>
        /// <param name="query">delete query</param>
        /// <returns>ICommand object</returns>
        public virtual async Task<ICommand> DeleteAsync(IQuery query)
        {
            var cmd = RdbCommand.CreateNewCommand(OperateType.Delete);
            SetCommand(cmd, null);
            if (query.VerifyResult != null)
            {
                cmd.VerifyResult = query.VerifyResult;
            }
            else
            {
                cmd.VerifyResult = r => r >= 0;
            }
            cmd.Query = query;
            return await Task.FromResult(cmd).ConfigureAwait(false);
        }

        #endregion

        #region query data

        /// <summary>
        /// query data
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data</returns>
        public virtual T Get(IQuery query)
        {
            return GetAsync(query).Result;
        }

        /// <summary>
        /// query data
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data</returns>
        public virtual async Task<T> GetAsync(IQuery query)
        {
            ICommand cmd = RdbCommand.CreateNewCommand(OperateType.Query);
            SetCommand(cmd, null);
            cmd.Query = query;
            cmd.Fields = GetQueryObjectFields(query);
            T obj = await UnitOfWork.UnitOfWork.QuerySingleAsync<T>(cmd).ConfigureAwait(false);
            SingleReturnInit(obj, query);
            return obj;
        }

        /// <summary>
        /// query data list
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data list</returns>
        public List<T> GetList(IQuery query)
        {
            return GetListAsync(query).Result;
        }

        /// <summary>
        /// query data list
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data list</returns>
        public async Task<List<T>> GetListAsync(IQuery query)
        {
            List<T> objList = await QueryListAsync(query).ConfigureAwait(false);
            if (objList == null || objList.Count <= 0)
            {
                return new List<T>(0);
            }
            return objList;
        }

        /// <summary>
        /// query data list
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data list</returns>
        async Task<List<T>> QueryListAsync(IQuery query)
        {
            ICommand cmd = RdbCommand.CreateNewCommand(OperateType.Query);
            SetCommand(cmd, null);
            cmd.Query = query;
            cmd.Fields = GetQueryObjectFields(query);
            var objList = (await UnitOfWork.UnitOfWork.QueryAsync<T>(cmd).ConfigureAwait(false)).ToList();
            BatchReturnInit(objList, query);
            return objList;
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data paging</returns>
        public IPaging<T> GetPaging(IQuery query)
        {
            return GetPagingAsync(query).Result;
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data paging</returns>
        public async Task<IPaging<T>> GetPagingAsync(IQuery query)
        {
            ICommand cmd = RdbCommand.CreateNewCommand(OperateType.Query);
            SetCommand(cmd, null);
            cmd.Query = query;
            cmd.Fields = GetQueryObjectFields(query);
            var objPaging = await UnitOfWork.UnitOfWork.QueryPagingAsync<T>(cmd).ConfigureAwait(false);
            BatchReturnInit(objPaging, query);
            return objPaging;
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>whether data is exist</returns>
        public virtual bool Exist(IQuery query)
        {
            return ExistAsync(query).Result;
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>whether data is exist</returns>
        public virtual async Task<bool> ExistAsync(IQuery query)
        {
            var cmd = RdbCommand.CreateNewCommand(OperateType.Exist);
            SetCommand(cmd, null);
            cmd.VerifyResult = r => r > 0;
            cmd.Query = query;
            cmd.Fields = GetQueryFields();
            cmd.CommandResultType = ExecuteCommandResult.ExecuteScalar;
            return await UnitOfWork.UnitOfWork.QueryAsync(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// get max value
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>max value</returns>
        public virtual DT Max<DT>(IQuery query)
        {
            return MaxAsync<DT>(query).Result;
        }

        /// <summary>
        /// get max value
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>max value</returns>
        public virtual async Task<DT> MaxAsync<DT>(IQuery query)
        {
            return await AggregateFunctionAsync<DT>(OperateType.Max, query).ConfigureAwait(false);
        }

        /// <summary>
        /// get minvalue
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>minvalue</returns>
        public DT Min<DT>(IQuery query)
        {
            return MinAsync<DT>(query).Result;
        }

        /// <summary>
        /// get minvalue
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>minvalue</returns>
        public async Task<DT> MinAsync<DT>(IQuery query)
        {
            return await AggregateFunctionAsync<DT>(OperateType.Min, query).ConfigureAwait(false);
        }

        /// <summary>
        /// caculate sum
        /// </summary>
        /// <typeparam name="DT">data value</typeparam>
        /// <param name="query">query value</param>
        /// <returns>caculated value</returns>
        public DT Sum<DT>(IQuery query)
        {
            return SumAsync<DT>(query).Result;
        }

        /// <summary>
        /// caculate sum
        /// </summary>
        /// <typeparam name="DT">data value</typeparam>
        /// <param name="query">query value</param>
        /// <returns>caculated value</returns>
        public async Task<DT> SumAsync<DT>(IQuery query)
        {
            return await AggregateFunctionAsync<DT>(OperateType.Sum, query).ConfigureAwait(false);
        }

        /// <summary>
        /// caculate average
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>average value</returns>
        public DT Avg<DT>(IQuery query)
        {
            return AvgAsync<DT>(query).Result;
        }

        /// <summary>
        /// caculate average
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>average value</returns>
        public async Task<DT> AvgAsync<DT>(IQuery query)
        {
            return await AggregateFunctionAsync<DT>(OperateType.Avg, query).ConfigureAwait(false);
        }

        /// <summary>
        /// caculate count
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data count</returns>
        public long Count(IQuery query)
        {
            return CountAsync(query).Result;
        }

        /// <summary>
        /// caculate count
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data count</returns>
        public async Task<long> CountAsync(IQuery query)
        {
            return await AggregateFunctionAsync<long>(OperateType.Count, query).ConfigureAwait(false);
        }

        /// <summary>
        /// aggregate function query
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>data</returns>
        async Task<DT> AggregateFunctionAsync<DT>(OperateType funcType, IQuery query)
        {
            ICommand cmd = RdbCommand.CreateNewCommand(funcType);
            SetCommand(cmd, null);
            cmd.Query = query;
            cmd.Fields = GetQueryObjectFields(query);
            DT obj = await UnitOfWork.UnitOfWork.QuerySingleAsync<DT>(cmd).ConfigureAwait(false);
            return obj;
        }

        #endregion

        #region batch return init

        void BatchReturnInit(IEnumerable<T> values, IQuery query)
        {
            if (values == null)
            {
                return;
            }
            foreach (var value in values)
            {
                value.BatchInit(query);
            }
        }

        #endregion

        #region single return init

        void SingleReturnInit(T value, IQuery query)
        {
            if (value != default(T))
            {
                value.SingleInit(query);
            }
        }

        #endregion

        #region get edit fields

        /// <summary>
        /// get edit fields
        /// </summary>
        /// <returns></returns>
        protected abstract string[] GetEditFields();

        #endregion

        #region get query fields

        /// <summary>
        /// get query fields
        /// </summary>
        /// <returns></returns>
        protected abstract string[] GetQueryFields();

        #endregion

        #region get fields by query object

        /// <summary>
        /// get fields by query object
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns></returns>
        protected string[] GetQueryObjectFields(IQuery query)
        {
            if (query == null)
            {
                return GetQueryFields();
            }
            List<string> fields = query.QueryFields;
            if (fields == null || fields.Count <= 0)
            {
                fields = GetQueryFields().ToList();
            }
            if (query.NotQueryFields != null && query.NotQueryFields.Count > 0)
            {
                fields = fields.Except(query.NotQueryFields).ToList();
            }
            return fields.ToArray();
        }

        #endregion

        #region set command values

        /// <summary>
        /// set command values
        /// </summary>
        /// <param name="cmd">command</param>
        /// <param name="values">values</param>
        void SetCommand(ICommand cmd, Dictionary<string, dynamic> values = null)
        {
            if (cmd == null)
            {
                return;
            }
            Type type = typeof(T);

            //对象名称
            cmd.ObjectName = QueryConfig.GetObjectName(type);

            #region 主键值

            var primaryKeys = QueryConfig.GetPrimaryKeys(type);
            cmd.ObjectKeys = primaryKeys;
            if (primaryKeys != null && values != null && values.Count > 0)
            {
                SortedDictionary<string, dynamic> primaryValues = new SortedDictionary<string, dynamic>();
                foreach (var key in primaryKeys)
                {
                    if (values.ContainsKey(key))
                    {
                        primaryValues.Add(key, values[key]);
                    }
                }
                cmd.ObjectKeyValues = primaryValues;
            }

            #endregion
        }

        #endregion

        #region init datavalue

        /// <summary>
        /// init version value
        /// </summary>
        internal void InitVersionFieldValue(T obj)
        {
            if (obj == null)
            {
                return;
            }
            string versionField = QueryConfig.GetVersionField(typeof(T));
            if (string.IsNullOrWhiteSpace(versionField))
            {
                return;
            }
            long initValue = 1;
            if (obj.PropertyValues.ContainsKey(versionField))
            {
                obj.PropertyValues[versionField] = initValue;
            }
            else
            {
                obj.PropertyValues.Add(versionField, initValue);
            }
        }

        /// <summary>
        /// init refresh date
        /// </summary>
        /// <param name="obj"></param>
        internal void InitRefreshDateFieldValue(T obj)
        {
            if (obj == null)
            {
                return;
            }
            string refreshDateField = QueryConfig.GetRefreshDateField(typeof(T));
            if (string.IsNullOrWhiteSpace(refreshDateField))
            {
                return;
            }
            if (obj.PropertyValues.ContainsKey(refreshDateField))
            {
                obj.PropertyValues[refreshDateField] = DateTime.Now;
            }
            else
            {
                obj.PropertyValues.Add(refreshDateField, DateTime.Now);
            }
        }

        #endregion
    }
}
