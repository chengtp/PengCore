using DDD.Util.Command;
using DDD.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.UnitOfWork
{
    /// <summary>
    /// IUnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// commit success callback event
        /// </summary>
        event Action CommitSuccessCallbackEvent;

        /// <summary>
        /// Commit Command
        /// </summary>
        /// <returns></returns>
        CommitResult Commit();

        /// <summary>
        /// Commit Command
        /// </summary>
        /// <returns></returns>
        Task<CommitResult> CommitAsync();

        /// <summary>
        /// add command
        /// </summary>
        /// <param name="cmds">commands</param>
        void AddCommand(params ICommand[] cmds);

        /// <summary>
        /// command count
        /// </summary>
        int CommandCount { get; }
    }
}
