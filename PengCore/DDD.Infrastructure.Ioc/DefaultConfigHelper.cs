using DDD.Infrastructure.Repository;
using DDD.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient(typeof(DDD.Repository.Interfaces.BaseInterfaces.IRepository<>), typeof(BaseRepository<>));
            //  services.AddTransient<IUserInfoRepository, UserInfoRepository>();
          
           
        }
    }
}
