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
    /// <summary>
    /// 测试服务类
    /// </summary>
    public class DemoService : IDemoService
    {
        private DapperDBContext _context;
        private IDemoRepository iDemoRepository;
        private readonly AutoMapper.IMapper modelMapper;
        public DemoService(DapperDBContext context, IDemoRepository _iDemoRepository,
            AutoMapper.IMapper _modelMapper)
        {
            _context = context;
            iDemoRepository = _iDemoRepository;
            modelMapper = _modelMapper;
        }

        /// <summary>
        /// 获取所有分页测试数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<DemoOutput>> GetModels()
        {
            PagedResult<DemoOutput> models = new PagedResult<DemoOutput>();
            PagedResult<Domain.T_Demo> SourceModels = await iDemoRepository.GetModels();

            try
            {
                models.Data = modelMapper.Map<List<DemoOutput>>(SourceModels.Data);
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

        /// <summary>
        /// 通过sql 语句查询一条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DemoOutput> GetModelBysql(Guid Id, string userName)
        {
            var model = await iDemoRepository.GetModelBysql(Id, userName);
            return modelMapper.Map<DemoOutput>(model);
        }
    }
}
