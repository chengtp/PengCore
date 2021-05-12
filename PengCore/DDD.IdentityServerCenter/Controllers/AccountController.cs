using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.IdentityServerCenter.Data;
using DDD.IdentityServerCenter.Interfaces;
using DDD.IdentityServerCenter.Models;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DDD.IdentityServerCenter.Controllers
{
    public class AccountController : Controller
    {

        private ISys_UserInfoService userInfoService;
        private readonly IIdentityServerInteractionService interaction;
        public AccountController(ISys_UserInfoService _userInfoService
            , IIdentityServerInteractionService _interaction)
        {
            userInfoService = _userInfoService;
            interaction = _interaction;
        }

        [HttpGet]

        public ActionResult Login(string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string loginName, string password, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;

            ////验证用户名和密码  Id=1  
            //Sys_UserInfo user = new Sys_UserInfo()
            //{
            //    Id = Guid.NewGuid(),
            //    LoginName = "admin",
            //    UserName = "chengtp",
            //    Password = "123456",
            //    Email = "4534534@qq.com"
            //};

            Sys_UserInfo user = await userInfoService.GetByStr(loginName, password);
            if (user != null)
            {
                var props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    
                };

                // await _signInManager.SignInAsync(user,props);

                await HttpContext.SignInAsync(user.Id.ToString(), user.UserName, props);

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                //return RedirectToAction("~/");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
            //return RedirectToAction("~/");
        }


        //public  T GetRequiredService<T>(this IServiceProvider provider)
        //{
        //    if (provider == null)
        //    {
        //        throw new ArgumentNullException("provider");
        //    }


        //    Microsoft.Extensions.DependencyInjection.ISupportRequiredService supportRequiredService = null;
        //    var tt= supportRequiredService.GetRequiredService(typeof(IAuthenticationService));

        //    return (T)tt;
        //}


        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string logoutId)
        {

            //await Context.SignOutAsync(IdentityConstants.ApplicationScheme);
            //await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            //await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            //    Microsoft.Extensions.DependencyInjection.a

            //   string ApplicationScheme = ".Application";

            //   GetRequiredService<IAuthenticationService>().SignOutAsync(HttpContext, ApplicationScheme, null);

            // await Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<IAuthenticationService>(null).SignOutAsync(HttpContext, ApplicationScheme,null);

            //  	return context.RequestServices.GetRequiredService<IAuthenticationService>().SignOutAsync(context, scheme, properties);


            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            //await HttpContext.SignOutAsync();

            //return RedirectToAction("Index", "Home");






            if (HttpContext.Request.Cookies.Count > 0)
            {
                var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
                foreach (var cookie in siteCookies)
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }

            await HttpContext.SignOutAsync();
            //  HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");









            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var prop = new AuthenticationProperties()
            //{
            //    RedirectUri = Request.Headers["Referer"].ToString()
            //};
            //// after signout this will redirect to your provided target
            //await HttpContext.SignOutAsync();

            //return Redirect(Request.Headers["Referer"].ToString());



            //  var logout = await interaction.GetLogoutContextAsync(logoutId);

            //  // SignOut("Cookies", "oidc");


            //  var user = HttpContext.User;
            //  if (user?.Identity.IsAuthenticated == true)
            //  {
            //      // delete local authentication cookie
            //      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //  }

            ////  await HttpContext.SignOutAsync();
            ////  await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            //  if (!string.IsNullOrWhiteSpace(logout?.PostLogoutRedirectUri))
            //  {
            //      return Redirect(logout?.PostLogoutRedirectUri);
            //  }
            //  var refererUrl = Request.Headers["Referer"].ToString();

            //  return RedirectToAction("Index", "Home");
            // return Redirect(refererUrl);
        }





        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
