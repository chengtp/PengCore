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
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private DapperDBContext _context;
        private IDemoService iDemoService;
        private readonly AutoMapper.IMapper modelMapper;
        private ILog Log;
        /// <summary>
        /// DemoController
        /// </summary>
        public DemoController(DapperDBContext context, IDemoService _iDemoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv)
        {
            _context = context;
            iDemoService = _iDemoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
        }

        /// <summary>
        ///通过Id 查询 demo一条数据
        /// </summary>
        /// <remarks>
        /// 例子
        /// GetModelBySql
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModelBySql")]
        public async Task<ActionResult> GetModelBySql(Guid Id)
        {
            if (Id != null && Id != Guid.Empty)
            {
                var models = await iDemoService.GetModelBysql(Id);
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