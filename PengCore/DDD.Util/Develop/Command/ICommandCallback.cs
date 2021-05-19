using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Command
{
    /// <summary>
    /// Command Callback
    /// </summary>
    /// <param name="request">request</param>
    /// <returns>response</returns>
    public delegate CommandCallbackResponse ExecuteCommandCallback(CommandCallbackRequest request);

    /// <summary>
    /// Command Before Execute
    /// </summary>
    /// <param name="request">request</param>
    public delegate bool BeforeExecute(BeforeExecuteRequest request);
}
