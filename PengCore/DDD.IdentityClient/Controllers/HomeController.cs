using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDD.IdentityClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace DDD.IdentityClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        ///// <summary>
        ///// 登出
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync("Cookies");
        //    await HttpContext.SignOutAsync("oidc");
        //    return View("Index");
        //}


        [Microsoft.AspNetCore.Authorization.AllowAnonymous]

        public IActionResult Test() {

            return Content("12345");
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
