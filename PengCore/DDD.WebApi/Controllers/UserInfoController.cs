using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Common;
using DDD.Infrastructure;
using DDD.Infrastructure.Dtos.Config;
using DDD.WebApi.Filters;
using DDD.WebApi.Token;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DDD.WebApi.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        #region 构造方法

        private DapperDBContext context;    //已注入默认数据库，事务操作
        private IUserInfoService iUserInfoService;  //应用层:数据操作
        private readonly AutoMapper.IMapper modelMapper; //Dto映射
        private ILog Log;  //日志文件
        private readonly IOptions<Infrastructure.Dtos.Config.AppSettings> appSettings; //配置文件数据
        /// <summary>
        /// DemoController
        /// </summary>
        public UserInfoController(DapperDBContext _context
            , IUserInfoService _iUserInfoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv
            , IOptions<Infrastructure.Dtos.Config.AppSettings> _appSettings)
        {
            context = _context;
            iUserInfoService = _iUserInfoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
            appSettings = _appSettings;
        }

        #endregion

        #region 查询所有用户集合信息

        /// <summary>
        /// 查询所有用户集合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModels")]
        public IActionResult GetModels()
        {

            return Ok();
        }

        #endregion

        #region 查询条件所有部分用户集合信息

        /// <summary>
        /// 查询条件所有部分用户集合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModelsBy")]
        public IActionResult GetModelsBy()
        {

            return Ok();
        }

        #endregion

        #region 根据主键查询一条用户记录

        /// <summary>
        /// 根据主键查询一条用户记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModelById")]
        public IActionResult GetModelById()
        {

            return Ok();
        }

        #endregion

        #region 创建用户

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateModel")]
        public IActionResult CreateModel()
        {

            return Ok();
        }

        #endregion

        #region 更新用户

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateModel")]
        public IActionResult UpdateModel()
        {

            return Ok();
        }

        #endregion
    }
}
