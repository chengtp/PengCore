﻿using DDD.Application.Dtos;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using DDD.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Services
{
    public class UserInfoService : IUserInfoService
    {
        private DapperDBContext _context;
        private IUserInfoRepository iUserInfoRepository;
        private readonly AutoMapper.IMapper modelMapper;
        public UserInfoService(DapperDBContext context, IUserInfoRepository _iUserInfoRepository,
            AutoMapper.IMapper _modelMapper)
        {
            _context = context;
            iUserInfoRepository = _iUserInfoRepository;
            AutoMapper.IMapper modelMapper = _modelMapper;
        }


        public async Task<IEnumerable<UserInfoOutput>> GetModels()
        {
            IEnumerable<UserInfoOutput> models = null;
            IEnumerable<Domain.Sys_UserInfo> SourceModels = await iUserInfoRepository.GetModels();
           //  models = modelMapper.Map<IEnumerable<UserInfoOutput>>(SourceModels);
            models = modelMapper.Map<IEnumerable<Domain.Sys_UserInfo>, IEnumerable<UserInfoOutput>>(SourceModels);
            return models;
        }
    }
}