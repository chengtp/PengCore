using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityServerCenter
{
    /// <summary>
    /// IdentityServer资源和客户端配置文件
    /// </summary>

    public class Config
    {

        /// <summary>
        /// 定义API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("BaseApi","MyBaseApi")
            };

        }

        /// <summary>
        /// 定义客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                    //new Client
                    //{
                    //ClientId="WebClient",
                    ////4中类型
                    //AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    ////// secret for authentication
                    //ClientSecrets={ new Secret("secret".Sha256())},
                    //// scopes that client has access to
                    //AllowedScopes={ "BaseApi"}
                    //},

                     #region SSO Demo Client
                   new Client
                   {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,//隐式方式
                    AllowAccessTokensViaBrowser = true, //允许通过浏览器传输token
                    RequireConsent=false,//如果不需要显示否同意授权 页面 这里就设置为false
                    RedirectUris = { "http://localhost:5001/signin-oidc" },//登录成功后返回的客户端地址（走配置）
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },//注销登录后返回的客户端地址（走配置）

                    //添加身份资源的 Scope
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowOfflineAccess = true,
                    //单位秒：默认1小时，Microsoft JWT验证中间件中存在时钟偏差 . 它默认设置为5分钟，不能少于，否则 - 访问令牌的建议生命周期为 as short as possible
                    AccessTokenLifetime=301
                }
                #endregion
                   ,new Client
                   {
                    ClientId = "mvc2",
                    ClientName = "MVC Client2",
                    AllowedGrantTypes = GrantTypes.Implicit,//隐式方式
                    AllowAccessTokensViaBrowser = true, //允许通过浏览器传输token
                    RequireConsent=false,//如果不需要显示否同意授权 页面 这里就设置为false
                    RedirectUris = { "http://localhost:5002/signin-oidc" },//登录成功后返回的客户端地址（走配置）
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },//注销登录后返回的客户端地址（走配置）

                    //添加身份资源的 Scope
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowOfflineAccess = true,
                    //单位秒：默认1小时，Microsoft JWT验证中间件中存在时钟偏差 . 它默认设置为5分钟，不能少于，否则 - 访问令牌的建议生命周期为 as short as possible
                    AccessTokenLifetime=301
                }
            };

        }


        public static IEnumerable<IdentityResource> GetIdentityResources() {

            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
            };
        }

    }
}
