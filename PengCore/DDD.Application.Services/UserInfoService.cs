using DDD.Application.Dtos;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using DDD.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DDD.Infrastructure.Dtos.PageList;

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
            modelMapper = _modelMapper;
        }


        public async Task<PagedResult<UserInfoOutput>> GetModels()
        {
            PagedResult<UserInfoOutput> models = new PagedResult<UserInfoOutput>();
            PagedResult<Domain.Sys_UserInfo> SourceModels = await iUserInfoRepository.GetModels();

            try
            {
                models.Data = modelMapper.Map<List<UserInfoOutput>>(SourceModels.Data);
                models.PageIndex = SourceModels.PageIndex;
                models.PageSize = SourceModels.PageSize;
                models.TotalPages = SourceModels.TotalPages;
                models.TotalRecords = SourceModels.TotalRecords;
                //models = modelMapper.Map<IEnumerable<Domain.Sys_UserInfo>, IEnumerable<UserInfoOutput>>(SourceModels);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return models;
        }
    }
}
