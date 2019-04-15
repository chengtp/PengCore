using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApi.Controllers
{
    /// <summary>
    /// 起始控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private DapperDBContext _context;
        private IUserInfoService iUserInfoService;
        private readonly AutoMapper.IMapper modelMapper;
        private ILog Log;
        /// <summary>
        /// HomeController
        /// </summary>
        public HomeController(DapperDBContext context, IUserInfoService _iUserInfoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv)
        {
            _context = context;
            iUserInfoService = _iUserInfoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
        }

        /// <summary>
        /// 获取所有用户数据,分页
        /// </summary>
        /// <remarks>
        /// 例子
        /// Index
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            Log.Error("测试日志");
            var models = await iUserInfoService.GetModels();
            return Ok(models);
        }

    }
}