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

namespace DDD.WebApi.Controllers
{
    /// <summary>
    /// 起始控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private DapperDBContext _context;
        private IUserInfoService iUserInfoService;
        private readonly AutoMapper.IMapper modelMapper;
        private IConfiguration _configuration;
        private ITokenHelper tokenHelper;
        private ILog Log;
        /// <summary>
        /// HomeController
        /// </summary>
        public HomeController(DapperDBContext context, IUserInfoService _iUserInfoService,
            AutoMapper.IMapper _modelMapper, IHostingEnvironment hostingEnv, IConfiguration configuration
            , ITokenHelper _tokenHelper)
        {
            _context = context;
            iUserInfoService = _iUserInfoService;
            modelMapper = _modelMapper;
            _configuration = configuration;
            tokenHelper = _tokenHelper;
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
        [Microsoft.AspNetCore.Mvc.HttpGet, Authorize]
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            Log.Error("测试日志");
            var models = await iUserInfoService.GetModels();
            return Ok(models);
        }





        /// <summary>
        /// 校验登录
        /// </summary>
        /// <returns></returns>
        [HttpPost,AllowAnonymous]
        [Route("login")]
        public  IActionResult login([FromBody] DDD.Application.Dtos.UserInfoLoginOut input)
        {

            //验证用户名和密码  

            var user = new DDD.Application.Dtos.UserInfoInput()
            {
                LoginName = "admin",
                Password = "123456",
                UserName = "admin"
            };



            if (null != user && user.Password.Equals(user.Password))
            {
                ComplexToken ct = tokenHelper.CreateToken(user);

                return Ok(ct);
            }
            return BadRequest();

           
        }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
       
        [HttpPost]
        [Route("refresh")]
        public IActionResult refresh()
        {
            Token.Token token = tokenHelper.RefreshToken(Request.HttpContext.User);
            return Ok(token);
        }


        /// <summary>
        /// test1
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("test1")]
        public IActionResult test1()
        {
            
            return Ok("test1");
        }

        /// <summary>
        /// test2
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("test2")]
        public IActionResult test2()
        {
            return Ok("test2");
        }

        ///// <summary>
        ///// 获取系统信息接口
        ///// </summary>
        ///// <returns></returns>
        //[TypeFilter(typeof(JwtCheckFilterAttribute))]
        //[Route("GetMsg")]
        //public IActionResult GetMsg()
        //{
        //    string msg = "windows10";
        //    return Ok(new { status = "ok", msg = msg });
        //}



    }
}