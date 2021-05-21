using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.WebApi.Filters
{
    /// <summary>
    /// 授权过滤器
    /// 于确定是否已针对当前请求为当前用户授权。 如果请求未获授权，它们可以让管道短路
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           
        }
    }
}
