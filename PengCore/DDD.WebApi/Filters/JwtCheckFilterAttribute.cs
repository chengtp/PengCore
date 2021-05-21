using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AutoMapper.Configuration;
using DDD.Infrastructure.Dtos.Config;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace DDD.WebApi.Filters
{
    /// <summary>
    /// Bearer认证,返回ajax中的error
    /// 校验访问令牌的合法性
    /// </summary>
    public class JwtCheckFilterAttribute : ActionFilterAttribute
    {
        private IOptions<JWTConfig> jWTConfig;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_jWTConfig">jwt配置属性</param>
        public JwtCheckFilterAttribute(IOptions<JWTConfig> _jWTConfig)
        {
            jWTConfig = _jWTConfig;

        }

        /// <summary>
        /// action执行前执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            //如果没有token   或者  是声明了 allAnonymous   
            if (!context.HttpContext.User.Identity.IsAuthenticated || context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            //获取当前请求的Token
            var authorization = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorization == null)
            {
                context.Result = new ContentResult { StatusCode = 401, Content = "token不能为空" };
                return;
            }
            string pattern = "^Bearer (.*?)$";
            if (!Regex.IsMatch(authorization, pattern))
            {
                context.Result = new ContentResult { StatusCode = 401, Content = "token格式不对!格式为:Bearer {token}" };
                return;
            }

            string[] stringList = authorization.Split(' ');
            if (stringList.Length > 1)
            {
                string tokenOld = stringList[1];
                var tokens = new JwtSecurityTokenHandler().ReadJwtToken(tokenOld);
                var temp = tokens.Claims;

                //查询真实的claims数据
                var clarm = temp.Where(t => t.Type == JwtRegisteredClaimNames.Iat).FirstOrDefault();
                string expiredDate = clarm.Value;


                ////Unix时间戳转换为C# DateTimes
                //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                //DateTime dt = startTime.AddSeconds(Convert.ToInt64(expiredDate));

                //if (Convert.ToInt64(expiredDate) > DateTimeOffset.Now.ToUnixTimeSeconds())
                //{
                //    context.Result = new ContentResult { StatusCode = 401, Content = "expired" };
                //    return;
                //}

            }

        }


    }
}
