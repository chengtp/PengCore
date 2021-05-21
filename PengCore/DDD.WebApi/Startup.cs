using AutoMapper;
using DDD.Infrastructure;
using DDD.WebApi.Filters;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;

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
            //注册sesion
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });
            // services.AddMemoryCache(); //使用本地缓存必须添加

            //注册mvc服务
            services.AddMvc(action=> {
                //配置过滤器
                action.Filters.Add<ExceptionFilter>();          //异常处理
                action.Filters.Add<AuthorizationFilter>();      //用户授权
                action.Filters.Add<JwtCheckFilterAttribute>();  //验证token有效性
                action.Filters.Add<ResultFilter>();             //token延迟
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //添加 HttpContext
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            //添加系统配置
            services.Configure<Infrastructure.Dtos.Config.ApplicationConfiguration>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<Infrastructure.Dtos.Config.AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<Infrastructure.Dtos.Config.JWTConfig>(Configuration.GetSection("JWTConfig"));
            #region 读取配置
            Infrastructure.Dtos.Config.JWTConfig config = new Infrastructure.Dtos.Config.JWTConfig();
            Configuration.GetSection("JWTConfig").Bind(config);
            #endregion


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
            //注入
            services.AddSingleton<Token.ITokenHelper, Token.TokenHelper>();

            //添加 Mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Infrastructure.AutoMapper.ServiceProfiles());
            });
            //注册Mapper
            services.AddAutoMapper();

            //注册授权
            #region 添加验证服务

            // 添加验证服务
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    // 是否开启签名认证
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.IssuerSigningKey)),
                    // 发行人验证，这里要和token类中Claim类型的发行人保持一致
                    ValidateIssuer = true,
                    ValidIssuer = config.Issuer,//发行人
                    // 接收人验证
                    ValidateAudience = true,
                    ValidAudience = config.Audience,//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1), // TimeSpan.Zero,  //ClockSkew默认值为5分钟，它是一个缓冲期，例如Token设置有效期为30分钟，到了30分钟的时候是不会过期的，会有这么个缓冲时间，也就是35分钟才会过期
                };
            });
            #endregion


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



                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                 });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization", //jwt默认的参数名称
                    Type = SecuritySchemeType.ApiKey
                });

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
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();
            }

            // 跳转https
            app.UseHttpsRedirection();


            #region CORS
            //跨域第二种方法，使用策略，详细策略信息在ConfigureService中
            app.UseCors("LimitRequests");//将 CORS 中间件添加到 web 应用程序管线中, 以允许跨域请求。


            //跨域第一种版本，请要ConfigureService中配置服务 services.AddCors();
            //    app.UseCors(options => options.WithOrigins("http://localhost:8021").AllowAnyHeader()
            //.AllowAnyMethod()); 
            #endregion

            //添加授权中间件
            app.UseAuthentication();

            // 使用静态文件
            app.UseStaticFiles();

            // 使用cookie
            app.UseCookiePolicy();

            // 返回错误码
            app.UseStatusCodePages();//把错误码返回前台，比如是404

            app.UseMvc();

            //添加httpcontext
            // DDD.Common.HttpContext.Configure(app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());


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
