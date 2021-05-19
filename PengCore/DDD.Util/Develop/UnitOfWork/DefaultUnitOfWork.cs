using DDD.Util.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.UnitOfWork
{
    /// <summary>
    /// default implements for IUnitOfWork
    /// </summary>
    public class DefaultUnitOfWork : IUnitOfWork
    {
        List<ICommand> commandList = new List<ICommand>();//command list

        /// <summary>
        /// commit success callback event
        /// </summary>
        public event Action CommitSuccessCallbackEvent;

        /// <summary>
        /// instance a defaultunitofwork object
        /// </summary>
        internal DefaultUnitOfWork()
        {
            UnitOfWork.Current?.Dispose();
            UnitOfWork.InvokeCreateWorkEvent();
            UnitOfWork.Current = this;
        }

        /// <summary>
        /// command count
        /// </summary>
        public int CommandCount
        {
            get
            {
                return commandList?.Count ?? 0;
            }
        }

        #region Instance Methods

        /// <summary>
        /// Add Commands To UnitOfWork
        /// </summary>
        /// <param name="cmds">Commands</param>
        public void AddCommand(params ICommand[] cmds)
        {
            if (cmds == null)
            {
                return;
            }
            foreach (var cmd in cmds)
            {
                if (cmd == null)
                {
                    continue;
                }
                commandList.Add(cmd);
            }
        }

        /// <summary>
        /// Commit Work
        /// </summary>
        /// <returns></returns>
        public CommitResult Commit()
        {
            return CommitAsync().Result;
        }

        /// <summary>
        /// Commit Work
        /// </summary>
        /// <returns></returns>
        public async Task<CommitResult> CommitAsync()
        {
            try
            {
                if (commandList.Count <= 0)
                {
                    return new CommitResult()
                    {
                        CommitCommandCount = 0,
                        ExecutedDataCount = 0
                    };
                }
                var exectCommandList = commandList.Where(c => !c.IsObsolete).ToList();
                bool beforeExecuteResult = await ExecuteCommandBeforeExecuteAsync(exectCommandList).ConfigureAwait(false);
                if (!beforeExecuteResult)
                {
                    throw new Exception("Any Command BeforeExecute Event Return Fail");
                }
                var result = await CommandExecuteManager.ExecuteAsync(exectCommandList).ConfigureAwait(false);
                await ExecuteCommandCallbackAsync(exectCommandList, result > 0).ConfigureAwait(false);
                var commitResult = new CommitResult()
                {
                    CommitCommandCount = exectCommandList.Count,
                    ExecutedDataCount = result,
                    AllowNoneResultCommandCount = exectCommandList.Count(c => c.VerifyResult?.Invoke(0) ?? false)
                };
                if (commitResult.NoneCommandOrSuccess)
                {
                    CommitSuccessCallbackEvent?.Invoke();
                    UnitOfWork.InvokeCommitSuccessEvent();
                }
                return commitResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Execute Command Before Execute
        /// </summary>
        /// <param name="cmds">command</param>
        static async Task<bool> ExecuteCommandBeforeExecuteAsync(IEnumerable<ICommand> cmds)
        {
            if (cmds == null)
            {
                return false;
            }
            bool result = true;
            foreach (var cmd in cmds)
            {
                result = result && await cmd.ExecuteBeforeAsync().ConfigureAwait(false);
            }
            return result;
        }

        /// <summary>
        /// Execute Command Callback
        /// </summary>
        /// <param name="cmds">commands</param>
        static async Task ExecuteCommandCallbackAsync(IEnumerable<ICommand> cmds, bool success)
        {
            foreach (var cmd in cmds)
            {
                await cmd.ExecuteCompleteAsync(success).ConfigureAwait(false);
            }
        }

        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            commandList.Clear();
        }
    }
}
