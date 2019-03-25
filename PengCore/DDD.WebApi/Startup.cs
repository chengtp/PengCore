using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Infrastructure;
using DDD.Infrastructure.Dtos.Config;
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
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test API", Version = "v1" });
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseMvc();

            //Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                 c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("DDD.WebApi.Swagger.index.html");

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }
    }
}
