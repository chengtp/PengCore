using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DDD.Infrastructure.AutoMapper
{
    public class ServiceProfiles : Profile
    {
       
        public ServiceProfiles()
        {
            CreateMap<Domain.Sys_UserInfo, DDD.Infrastructure.Dtos.UserInfoOutput>();
            CreateMap<Domain.T_Demo, DDD.Infrastructure.Dtos.DemoOutput>();
            // CreateMap<IEnumerable<Domain.Sys_UserInfo>, IEnumerable<Application.Dtos.UserInfoOutput>>();
        }
    }
}
