using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Command
{
    /// <summary>
    ///  Command Execute Manager
    /// </summary>
    public static class CommandExecuteManager
    {
        #region propertys

        /// <summary>
        /// get or set command engine
        /// </summary>
        public static ICommandEngine ExectEngine { get; set; }

        #endregion

        #region methods

        #region execute

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="commands">commands</param>
        /// <returns>return the execute data numbers</returns>
        internal static int Execute(IEnumerable<ICommand> commands)
        {
            return ExecuteAsync(commands).Result;
        }

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="commands">commands</param>
        /// <returns>return the execute data numbers</returns>
        internal static async Task<int> ExecuteAsync(IEnumerable<ICommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                return 0;
            }
            return await ExectEngine.ExecuteAsync(commands.ToArray()).ConfigureAwait(false);
        }

        #endregion

        #region query

        /// <summary>
        /// execute query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static IEnumerable<T> Query<T>(ICommand cmd)
        {
            return QueryAsync<T>(cmd).Result;
        }

        /// <summary>
        /// execute query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static async Task<IEnumerable<T>> QueryAsync<T>(ICommand cmd)
        {
            return await ExectEngine.QueryAsync<T>(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static IPaging<T> QueryPaging<T>(ICommand cmd) where T : CommandEntity<T>
        {
            return QueryPagingAsync<T>(cmd).Result;
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static async Task<IPaging<T>> QueryPagingAsync<T>(ICommand cmd) where T : CommandEntity<T>
        {
            return await ExectEngine.QueryPagingAsync<T>(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>data is exist</returns>
        internal static bool Query(ICommand cmd)
        {
            return QueryAsync(cmd).Result;
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>data is exist</returns>
        internal static async Task<bool> QueryAsync(ICommand cmd)
        {
            return await ExectEngine.QueryAsync(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// query single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        internal static T QuerySingle<T>(ICommand cmd)
        {
            return QuerySingleAsync<T>(cmd).Result;
        }

        /// <summary>
        /// query single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        internal static async Task<T> QuerySingleAsync<T>(ICommand cmd)
        {
            return await ExectEngine.QuerySingleAsync<T>(cmd).ConfigureAwait(false);
        }

        #endregion 

        #endregion
    }
}
