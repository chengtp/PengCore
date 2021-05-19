﻿using DDD.Util.CQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Command
{
    /// <summary>
    /// command for RDB
    /// </summary>
    public class RdbCommand : ICommand
    {
        private RdbCommand()
        { }

        #region propertys

        /// <summary>
        /// command text
        /// </summary>
        public string CommandText { get; set; } = string.Empty;

        /// <summary>
        /// parameters
        /// </summary>
        public object Parameters { get; set; } = null;

        /// <summary>
        /// command type
        /// </summary>
        public RdbCommandTextType CommandType { get; set; } = RdbCommandTextType.Text;

        /// <summary>
        /// transaction command
        /// </summary>
        public bool TransactionCommand { get; set; } = false;

        /// <summary>
        /// result type
        /// </summary>
        public ExecuteCommandResult CommandResultType { get; set; } = ExecuteCommandResult.ExecuteRows;

        /// <summary>
        /// verify result method
        /// </summary>
        public Func<int, bool> VerifyResult
        {
            get; set;
        }

        /// <summary>
        /// object name
        /// </summary>
        public string ObjectName { get; set; } = string.Empty;

        /// <summary>
        /// object keys
        /// </summary>
        public SortedSet<string> ObjectKeys { get; set; } = null;

        /// <summary>
        /// object key values
        /// </summary>
        public SortedDictionary<string, dynamic> ObjectKeyValues { get; set; } = null;

        /// <summary>
        /// server keys
        /// </summary>
        public SortedSet<string> ServerKeys { get; set; } = null;

        /// <summary>
        /// server key values
        /// </summary>
        public SortedDictionary<string, dynamic> ServerKeyValues { get; set; } = null;

        /// <summary>
        /// execute mode
        /// </summary>
        public CommandExecuteMode ExecuteMode { get; set; } = CommandExecuteMode.Transform;

        /// <summary>
        /// query object
        /// </summary>
        public IQuery Query { get; set; } = null;

        /// <summary>
        /// operate
        /// </summary>
        public OperateType Operate { get; set; } = OperateType.Query;

        /// <summary>
        /// fields
        /// </summary>
        public IEnumerable<string> Fields { get; set; } = null;

        /// <summary>
        /// success callback
        /// </summary>
        public event ExecuteCommandCallback SuccessCallback;

        /// <summary>
        /// failed callback
        /// </summary>
        public event ExecuteCommandCallback FailedCallback;

        /// <summary>
        /// callback request
        /// </summary>
        public CommandCallbackRequest CallbackRequest { get; set; }

        /// <summary>
        /// before execute
        /// </summary>
        public event BeforeExecute BeforeExecute;

        /// <summary>
        /// before request
        /// </summary>
        public BeforeExecuteRequest BeforeRequest { get; set; }

        /// <summary>
        /// direct return if command is obsolete
        /// </summary>
        public bool IsObsolete
        {
            get
            {
                return Query?.IsObsolete ?? false;
            }
        }

        #endregion

        #region static methods

        /// <summary>
        /// get a new rdbcommand object
        /// </summary>
        /// <param name="operate">operate</param>
        /// <param name="parameters">parameters</param>
        /// <param name="objectName">objectName</param>
        /// <param name="objectKey">objectKey</param>
        /// <returns></returns>
        public static RdbCommand CreateNewCommand(OperateType operate, object parameters = null, string objectName = "", SortedSet<string> objectKeys = null, SortedDictionary<string, dynamic> objectKeyValues = null, SortedSet<string> serverKeys = null, SortedDictionary<string, dynamic> serverKeyValues = null)
        {
            return new RdbCommand()
            {
                Operate = operate,
                Parameters = parameters,
                ObjectName = objectName,
                ObjectKeyValues = objectKeyValues,
                ServerKeyValues = serverKeyValues,
                ObjectKeys = objectKeys,
                ServerKeys = serverKeys
            };
        }

        #endregion

        #region methods

        /// <summary>
        /// execute commplete
        /// </summary>
        /// <param name="success">success</param>
        public void ExecuteComplete(bool success)
        {
            ExecuteCompleteAsync(success).Wait();
        }

        /// <summary>
        /// execute commplete
        /// </summary>
        /// <param name="success">success</param>
        public async Task ExecuteCompleteAsync(bool success)
        {
            await Task.Run(() =>
            {
                if (success)
                {
                    SuccessCallback?.Invoke(CallbackRequest);
                }
                else
                {
                    FailedCallback?.Invoke(CallbackRequest);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// execute before
        /// </summary>
        /// <returns></returns>
        public bool ExecuteBefore()
        {
            return ExecuteBeforeAsync().Result;
        }

        /// <summary>
        /// execute before
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecuteBeforeAsync()
        {
            return await Task.Run(() =>
            {
                return BeforeExecute?.Invoke(BeforeRequest) ?? true;
            }).ConfigureAwait(false);
        }

        #endregion
    }
}
