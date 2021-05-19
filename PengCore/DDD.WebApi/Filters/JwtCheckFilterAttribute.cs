using Microsoft.AspNetCore.Mvc.Filters;

namespace DDD.WebApi.Filters
{
    /// <summary>
    /// Bearer认证,返回ajax中的error
    /// 校验访问令牌的合法性
    /// </summary>
    public class JwtCheckFilterAttribute : ActionFilterAttribute
    {
        //private IConfiguration _configuration;
        ///// <summary>
        ///// 构造方法
        ///// </summary>
        ///// <param name="configuration"></param>
        //public JwtCheckFilterAttribute(IConfiguration configuration)
        //{
        //    _configuration = configuration;
          
        //}

        ///// <summary>
        ///// action执行前执行
        ///// </summary>
        ///// <param name="context">上下文</param>
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    //1.判断是否需要校验
        //    //var isSkip = context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(SkipAttribute));
        //    //if (isSkip == false)
        //    //{
        //        //2. 判断是什么请求(ajax or 非ajax)
        //        var actionContext = context.HttpContext;
        //        if (IsAjaxRequest(actionContext.Request))
        //        {
        //            //表示是ajax
        //            var token = context.HttpContext.Request.Headers["Authorization"].ToString();    //ajax请求传过来
        //            string pattern = "^Bearer (.*?)$";
        //            if (!Regex.IsMatch(token, pattern))
        //            {
        //                context.Result = new ContentResult { StatusCode = 401, Content = "token格式不对!格式为:Bearer {token}" };
        //                return;
        //            }
        //            token = Regex.Match(token, pattern).Groups[1]?.ToString();
        //            if (token == "null" || string.IsNullOrEmpty(token))
        //            {
        //                context.Result = new ContentResult { StatusCode = 401, Content = "token不能为空" };
        //                return;
        //            }
        //            //校验auth的正确性
        //            var result = JWTHelp.JWTJieM(token, _configuration.GetSection("JWTConfig:IssuerSigningKey").Value);
        //            if (result == "expired")
        //            {
        //                context.Result = new ContentResult { StatusCode = 401, Content = "expired" };
        //                return;
        //            }
        //            else if (result == "invalid")
        //            {
        //                context.Result = new ContentResult { StatusCode = 401, Content = "invalid" };
        //                return;
        //            }
        //            else if (result == "error")
        //            {
        //                context.Result = new ContentResult { StatusCode = 401, Content = "error" };
        //                return;
        //            }
        //            else
        //            {
        //                //表示校验通过,用于向控制器中传值
        //                context.RouteData.Values.Add("auth", result);
        //            }

        //        }
        //        else
        //        {
        //            //表示是非ajax请求，则auth拼接在参数中传过来
        //            context.Result = new RedirectResult("/Home/NoPerIndex?reason=null");
        //            return;
        //        }
        //  //  }

        //}


        ///// <summary>
        ///// 判断该请求是否是ajax请求
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //private bool IsAjaxRequest(HttpRequest request)
        //{
        //    string header = request.Headers["X-Requested-With"];
        //    return "XMLHttpRequest".Equals(header);
        //}
    }
}
