using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DDD.WebApi.Token
{


    /// <summary>
    /// 令牌接口
    /// </summary>
    public interface ITokenHelper
    {
        /// <summary>
        /// 创建令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ComplexToken CreateToken(DDD.Infrastructure.Dtos.UserInfoInput user);
        /// <summary>
        /// 创建令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        ComplexToken CreateToken(Claim[] claims);
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        Token RefreshToken(ClaimsPrincipal claimsPrincipal);
    }
}
