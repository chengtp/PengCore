using DDD.Infrastructure.Dtos;
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
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserInfoService : IUserInfoService
    {
        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
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

        #endregion

        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserInfoLoginOut> GetModelByLogin(UserInfoLoginInput model)
        {
            var userInfo = await iUserInfoRepository.GetModelByLogin(model);
            return userInfo;
        }

        #endregion

        #region 查询所有用户集合信息

        /// <summary>
        /// 查询所有用户集合信息
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region 查询条件所有部分用户集合信息

        /// <summary>
        /// 查询条件所有部分用户集合信息
        /// </summary>
        /// <returns></returns>
        public Task<PagedResult<UserInfoOutput>> GetModelsBy()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 根据主键查询一条用户记录

        /// <summary>
        /// 根据主键查询一条用户记录
        /// </summary>
        /// <returns></returns>
        public Task<UserInfoOutput> GetModelById()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 创建用户

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public Task<bool> CreateModel()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 更新用户

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public Task<bool> UpdateModel()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
