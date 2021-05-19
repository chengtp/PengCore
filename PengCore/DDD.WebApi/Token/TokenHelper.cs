using DDD.Infrastructure.Dtos.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DDD.WebApi.Token
{

    /// <summary>
    /// Token 类型
    /// </summary>
    public enum TokenType
    {

        /// <summary>
        /// 授权令牌
        /// </summary>
        AccessToken = 1,
        /// <summary>
        /// 刷新令牌
        /// </summary>
        RefreshToken = 2
    }

    /// <summary>
    /// 令牌帮助类
    /// </summary>
    public class TokenHelper : ITokenHelper
    {
        private IOptions<JWTConfig> _options;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="options"></param>
        public TokenHelper(IOptions<JWTConfig> options)
        {
            _options = options;
        }

        /// <summary>
        /// 创建授权令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token CreateAccessToken(DDD.Application.Dtos.UserInfoInput user)
        {
            Claim[] claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.LoginName), new Claim(ClaimTypes.Name, user.UserName) };

            return CreateToken(claims, TokenType.AccessToken);
        }

        /// <summary>
        /// 创建授权令牌和刷新令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ComplexToken CreateToken(DDD.Application.Dtos.UserInfoInput user)
        {
            Claim[] claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.LoginName), new Claim(ClaimTypes.Name, user.UserName) };

            return CreateToken(claims);
        }

        /// <summary>
        /// 创建授权令牌和刷新令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public ComplexToken CreateToken(Claim[] claims)
        {
            return new ComplexToken { AccessToken = CreateToken(claims, TokenType.AccessToken), RefreshToken = CreateToken(claims, TokenType.RefreshToken) };
        }

        /// <summary>
        /// 用于创建AccessToken和RefreshToken。
        /// 这里AccessToken和RefreshToken只是过期时间不同，【实际项目】中二者的claims内容可能会不同。
        /// 因为RefreshToken只是用于刷新AccessToken，其内容可以简单一些。
        /// 而AccessToken可能会附加一些其他的Claim。
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        private Token CreateToken(Claim[] claims, TokenType tokenType)
        {
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(tokenType.Equals(TokenType.AccessToken) ? _options.Value.AccessTokenExpiresMinutes : _options.Value.RefreshTokenExpiresMinutes));
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: tokenType.Equals(TokenType.AccessToken) ? _options.Value.Audience : _options.Value.RefreshTokenAudience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256));
            return new Token { TokenContent = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires };
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public Token RefreshToken(ClaimsPrincipal claimsPrincipal)
        {
            var code = claimsPrincipal.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier));
            if (null != code)
            {

                //数据库中获取 判断  loginname= code
                DDD.Application.Dtos.UserInfoInput user = new Application.Dtos.UserInfoInput() { 
                 UserName="admin",
                 Password="123456",
                 LoginName="admin"
                  
                };

                return CreateAccessToken(user);
            }
            else
            {
                return null;
            }
        }
    }
}
