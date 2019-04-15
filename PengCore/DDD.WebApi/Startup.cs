using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Infrastructure;
using DDD.Infrastructure.Dtos.Config;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DDD.WebApi
{
    /// <summary>
    /// 启动Strartup
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
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 配置日志
        /// </summary>
        public static ILoggerRepository Repository { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //添加系统配置
            services.Configure<ApplicationConfiguration>(Configuration.GetSection("ConnectionStrings"));

            //连接字符串
            var connection = Configuration.GetConnectionString("sqlserver");
            //连接字符串添加到配置中
            services.AddDapperDBContext<SqlserverDBContext>(options =>
            {
                options.Configuration = connection.ToString();
            });

            //添加Ioc
            Infrastructure.IoC.DefaultConfigHelper.RegisterBasicType(services);

            //添加 Mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Infrastructure.AutoMapper.ServiceProfiles());
            });

            services.AddAutoMapper();


            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "chengtp WebApi",
                    Version = "v1",
                    Description = " ASP.NET Core Web API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "程天鹏",
                        Email = "chengtianpeng@live.com",
                        Url = new System.Uri("https://github.com/chengtp")
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = System.IO.Path.Combine(basePath, "SwaggerData.xml");
                c.IncludeXmlComments(xmlPath);

            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();

            //Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "MyBigDataDocument";
                // c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("DDD.WebApi.Swagger.index.html");

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");

            });
        }
    }
}
