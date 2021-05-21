using log4net;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.WebApi.Filters
{
    /// <summary>
    /// 异常信息过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILog Log;  //日志文件

        /// <summary>
        /// 构造方法
        /// </summary>
        public ExceptionFilter()
        {
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(ExceptionFilter));
        }

        /// <summary>
        /// 异常方法
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {

            if (!context.ExceptionHandled)//如果异常没有处理
            {
                Log.Error(context.Exception.Message);

                context.Result = new JsonResult(new
                {
                    Result = false,
                    Code = 500,
                    Message = context.Exception.Message
                });

                context.ExceptionHandled = true;//异常已处理
            }
        }
    }
}
