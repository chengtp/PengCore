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
    /// 结果过滤器
    /// </summary>
    public class ResultFilter : IResultFilter
    {

        private IOptions<JWTConfig> jWTConfig;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_jWTConfig">jwt配置属性</param>
        public ResultFilter(IOptions<JWTConfig> _jWTConfig)
        {
            jWTConfig = _jWTConfig;

        }

        /// <summary>
        /// OnResultExecuted方法是在viewresult执行之后（即页面内容生成之后）执行
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        /// <summary>
        /// 其中OnResultExecuting是在controller中的action代码执行完毕后，但viewresult执行之前（即页面内容生成之前）执行
        /// 在结果过滤器中刷新jwt的token；token延迟
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
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
                return;
            }
            //string[] stringList = authorization.Split(' ');
            //if (stringList.Length > 1)
            //{
            //    string tokenOld = stringList[1];
            //    var tokens = new JwtSecurityTokenHandler().ReadJwtToken(tokenOld);
            //    var temp = tokens.Claims;

            //    List<Claim> claims = new List<Claim>();
            //    //添加的别的claims数据
            //    claims.AddRange(temp.Where(t => t.Type != JwtRegisteredClaimNames.Iat));

            //    //查询真实的claims数据
            //    string time = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            //    claims.Add(new Claim(JwtRegisteredClaimNames.Iat, time, ClaimValueTypes.Integer64));

            //    var now = DateTime.UtcNow;
            //    var jwtSecurityToken = new JwtSecurityToken(
            //        issuer: jWTConfig.Value.Issuer,
            //        audience: jWTConfig.Value.Audience,
            //        claims: claims,
            //        notBefore: now,
            //        expires: now.Add(TimeSpan.FromMilliseconds(jWTConfig.Value.AccessTokenExpiresMinutes)),
            //        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfig.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256)
            //    );

            //    string tokenNew = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            //    //在响应头中返回新的Token
            //    context.HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenNew);

            //}
        }


    }
}
