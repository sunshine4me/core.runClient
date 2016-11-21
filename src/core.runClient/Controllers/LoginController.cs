using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.runClient.ViewModels;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace core.runClient.Controllers
{
    public class LoginController : Controller
    {

        [Route("Login")]
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel md,[FromServices] IOptions<AppSetting> settings) {

            if (ModelState.IsValid) {

                if (md.UserName == settings.Value.adminUser && md.Password == settings.Value.adminPassword) {


                    ClaimsIdentity _identity = new ClaimsIdentity("core.runClient");
                    _identity.AddClaim(new Claim(ClaimTypes.Name, md.UserName));

                    var ExpiresUtc = DateTime.UtcNow.AddDays(7);

                    //await HttpContext.Authentication.SignOutAsync("mcookie");
                    await HttpContext.Authentication.SignInAsync("core.runClient", new ClaimsPrincipal(_identity), new AuthenticationProperties {
                        ExpiresUtc = ExpiresUtc,
                        IsPersistent = true,//cookie持久化
                        AllowRefresh = false //自动刷新cookie
                    });

                    return Redirect("/");
                }
            }
            return View(md);
            
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff() {
            await HttpContext.Authentication.SignOutAsync("core.runClient");
            return Redirect("/");

        }

        public IActionResult Forbidden() {
            return View("Error");
        }

    }
}
