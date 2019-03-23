using AutoMapper;
using System;

namespace DDD.Infrastructure.AutoMapper
{
    public class ServiceProfiles : Profile
    {
        public ServiceProfiles()
        {
            CreateMap<Domain.Sys_UserInfo, Application.Dtos.UserInfoOutput>();
        }
    }
}
