using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Dtos.Config
{
    public class JWTConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string IssuerSigningKey { get; set; }
        public int AccessTokenExpiresMinutes { get; set; }

        public string RefreshTokenAudience { get; set; }
        public int RefreshTokenExpiresMinutes { get; set; }
    }
}
