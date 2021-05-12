using DDD.Application.Dtos;
using DDD.Application.Interfaces;
using DDD.Infrastructure;
using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DDD.IdentityServer4.Controllers
{
    public class HomeController : Controller
    {

        private DapperDBContext context;    //已注入默认数据库，事务操作
        private IUserInfoService iUserInfoService;  //应用层:数据操作
        private readonly AutoMapper.IMapper modelMapper; //Dto映射
        private ILog Log;  //日志文件
        private readonly IOptions<Infrastructure.Dtos.Config.AppSettings> appSettings; //配置文件数据
        /// <summary>
        /// DemoController
        /// </summary>
        public HomeController(DapperDBContext _context
            , IUserInfoService _iUserInfoService
            , AutoMapper.IMapper _modelMapper
            , IOptions<Infrastructure.Dtos.Config.AppSettings> _appSettings)
        {
            context = _context;
            iUserInfoService = _iUserInfoService;
            modelMapper = _modelMapper;
            this.Log = LogManager.GetLogger(Startup.Repository.Name, typeof(HomeController));
            appSettings = _appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> Loign(string userName, string password, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;

            UserInfoLoginInput model = new UserInfoLoginInput { LoginName = userName, Password = password };
            var user = await iUserInfoService.GetModelByLogin(model);
            if (user != null)
            {
                AuthenticationProperties props = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                };
                await HttpContext.SignInAsync(user.Id.ToString(), user.UserName, props);
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}