﻿using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.IdentityServer4
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        //clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client{
                    ClientId="DDDMVC",
                     ClientName="MVC Client",
                      AllowedGrantTypes=GrantTypes.Implicit, //隐式方式
                      RequireConsent=false, //如果不需要显示是否同意授权也没，这里就设置为false
                      RedirectUris={ "http://localhost:5002/signin-oidc"}, //登录成功后返回的客户端地址
                      PostLogoutRedirectUris={ "http://localhost:5002/signout-callback-oidc" },//注销登录后返回的客户端地址
                      
                      AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }

    }
}
