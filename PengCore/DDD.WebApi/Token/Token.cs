using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.WebApi.Token
{
    /// <summary>
    /// 令牌实体
    /// </summary>
    public class Token
    {

        /// <summary>
        /// 令牌内容
        /// </summary>
        public string TokenContent { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; set; }
    }


    /// <summary>
    /// 复合令牌
    /// </summary>
    public class ComplexToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public Token AccessToken { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public Token RefreshToken { get; set; }
    }
}
