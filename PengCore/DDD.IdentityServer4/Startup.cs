using AutoMapper;
using DDD.Infrastructure;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;


namespace DDD.IdentityServer4
{
    /// <summary>
    /// 启动Strartup  : http://localhost:5000/.well-known/openid-configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("NETCoreRepository");
            // 指定配置文件
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// 配置日志
        /// </summary>
        public static ILoggerRepository Repository { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //注册mvc服务
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //添加 HttpContext
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            //添加系统配置
            services.Configure<Infrastructure.Dtos.Config.ApplicationConfiguration>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<Infrastructure.Dtos.Config.AppSettings>(Configuration.GetSection("AppSettings"));

            //默认数据库类型
            var componentDbType = Configuration.GetConnectionString("ComponentDbType");

            //默认连接字符串
            var connection = Configuration[string.Format("ConnectionStrings:ConnectionString:{0}", componentDbType)];
            //连接字符串添加到配置中
            if (componentDbType == "sqlserver")
            {
                services.AddDapperDBContext<SqlserverDBContext>(options => { options.Configuration = connection.ToString(); });
            }
            else if (componentDbType == "mysql")
            {
                services.AddDapperDBContext<MysqlDBContext>(options => { options.Configuration = connection.ToString(); });
            }

            //添加Ioc
            Infrastructure.IoC.DefaultConfigHelper.RegisterBasicType(services);

            //添加 Mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Infrastructure.AutoMapper.ServiceProfiles());
            });
            //注册Mapper
            services.AddAutoMapper();

            //注入Ids4服务,将配置文件的Client配置资源放在内存中
            services.AddIdentityServer().AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
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
