using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDD.IdentityClient.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]//身份认证
        public IActionResult Index()
        {
            return View();
        }

        public async Task Logout() {

            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

    }
}
