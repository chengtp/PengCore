using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Infrastructure;
using DDD.Infrastructure.Dtos.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace DDD.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //添加系统配置
            services.Configure<ApplicationConfiguration>(Configuration.GetSection("ConnectionStrings"));

            //连接字符串
            var connection = Configuration.GetConnectionString("sqlserver");
            //连接字符串添加到配置中
            services.AddDapperDBContext<SqlserverDBContext>(options =>
            {
                options.Configuration = connection.ToString();
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //DI 
            //services.AddTransient<IProductRepository, UowProductRepository>();
            //services.AddTransient<ICategoryRepository, UowCategoryRepository>();
            Infrastructure.IoC.DefaultConfigHelper.RegisterBasicType(services);


            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Infrastructure.AutoMapper.ServiceProfiles());
            });

           // var config = new MapperConfiguration(cfg => cfg.AddProfile<Infrastructure.AutoMapper.ServiceProfiles>());

            services.AddAutoMapper();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //DefaultFilesOptions options = new DefaultFilesOptions();
            //options.DefaultFileNames.Add("index.html");    //将index.html改为需要默认起始页的文件名.
            //app.UseDefaultFiles(options);

            app.UseStaticFiles();
            app.UseCookiePolicy();

            //  app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");  //（手动高亮）
            });
        }
    }
}
