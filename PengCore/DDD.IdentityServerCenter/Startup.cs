using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDD.IdentityServerCenter.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DDD.IdentityServerCenter.Interfaces;
using DDD.IdentityServerCenter.Services;

namespace DDD.IdentityServerCenter
{
    public class Startup
    {
        /// <summary>
        /// 启动Strartup :http://localhost:5000/.well-known/openid-configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<EFContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));//注入DbContext

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EFContext>()
                .AddDefaultTokenProviders();

            // cookie policy to deal with temporary browser incompatibilities
            //添加cookie的服务
            services.Configure<CookiePolicyOptions>(option => {
                option.CheckConsentNeeded = ContextBoundObject => true;
                option.MinimumSameSitePolicy = SameSiteMode.None;
            }); 


            //配置IdentityServer：所有必须的服务配置并依赖注入系统中
            //services.AddIdentityServer()
            //     .AddDeveloperSigningCredential() //添加证书
            //     .AddInMemoryApiResources(Config.GetApiResources()) //API资源加载到内存中
            //     .AddInMemoryClients(Config.GetClients())  //客户端定义加载到内存中
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())  //身份认证资源加载到内存
            // //  .AddAspNetIdentity<Models.ApplicationUser>();
            //    .AddResourceOwnerValidator<Validator.ResourceOwnerPasswordValidator>(); //验证用户密码有效性


            //#region SSO 单点登陆方式
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()//添加证书
                .AddInMemoryIdentityResources(Config.GetIdentityResources())//身份认证资源加载到内存
                .AddInMemoryClients(Config.GetClients());//客户端定义加载到内存中

            //#endregion

            services.AddTransient<ISys_UserInfoService, Sys_UserInfoService>();//service注入

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //添加cookie
            app.UseCookiePolicy();
            //身份验证
            app.UseAuthentication();


            //认证服务中心  中间件添加到http管道中
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
