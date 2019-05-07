﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.IoC
{
    public class DefaultConfigHelper
    {
        public static void RegisterBasicType(IServiceCollection services)
        {
            //services.AddTransient<IUserInfoRepository, UserInfoRepository>();
            services.AddTransient(typeof(DDD.Repository.Interfaces.BaseInterfaces.IRepository<>), typeof(Repository.BaseRepository<>));
            services.AddTransient<Interfaces.IUnitOfWorkFactory, DDD.Infrastructure.DapperUnitOfWorkFactory>();

            services.AddTransient<DDD.Repository.Interfaces.IUserInfoRepository, Repository.UserInfoRepository>();
            services.AddTransient<DDD.Repository.Interfaces.IDemoRepository, Repository.DemoRepository>();

            #region  Applicaton

            services.AddTransient<Application.Interfaces.IUserInfoService, Application.Services.UserInfoService>();
            services.AddTransient<Application.Interfaces.IDemoService, Application.Services.DemoService>();

            #endregion
        }
    }
}
