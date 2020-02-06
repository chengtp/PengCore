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
using Microsoft.Extensions.Options;

namespace DDD.WebApi.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private DapperDBContext context;    //已注入默认数据库，事务操作
        private IDemoService iDemoService;  //应用层:数据操作
        private readonly AutoMapper.IMapper modelMapper; //Dto映射
        private ILog Log;  //日志文件
        private readonly IOptions<Infrastructure.Dtos.AppSettings> appSettings; //配置文件数据
        /// <summary>
        /// DemoController
        /// </summary>
        public DemoController(DapperDBContext _context
            , IDemoService _iDemoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv
            , IOptions<Infrastructure.Dtos.AppSettings> _appSettings)
        {
            context = _context;
            iDemoService = _iDemoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
            appSettings = _appSettings;
        }

        /// <summary>
        ///通过Id 查询 demo一条数据
        /// </summary>
        /// <param name="Id">7A46AEB8-EA36-4D3B-B51A-F0B4D06B006A</param>
        /// <param name="userName">chengtianpeng</param>
        /// <remarks>
        /// 例子
        /// GetModelBySql
        /// </remarks>
        /// <returns></returns>

        [HttpGet]
        [Route("GetModelBySql")]
        public async Task<ActionResult> GetModelBySql(Guid Id,string userName)
        {
            if (Id != null && Id != Guid.Empty)
            {
                // appSettings.Value..
               
                 var models = await iDemoService.GetModelBysql(Id, userName);
                return Ok(models);
            }
            else
            {
                return BadRequest("Id不能为空");
            }

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
            var models = await iDemoService.GetModels();
            return Ok(models);
        }
    }
}