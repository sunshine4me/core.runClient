using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.phoneDevice;
using core.runClient.DataEntities;
using Microsoft.AspNetCore.Authorization;
using core.runClient.Extensions;
using Microsoft.Extensions.Logging;

namespace core.runClient.Controllers
{
    public class HomeController : Controller
    {

       


        public IActionResult Index([FromServices] DeviceManage dm)
        {
            ViewBag.task = dm.getQueueCount();
            return View(dm.pml);
        }

        private static string RefreshToken = "true";
        [Authorize]
        public IActionResult Refresh([FromServices] DeviceManage dm) {
            lock (RefreshToken) {
                dm.RefreshDevice();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult StartRun([FromServices] DeviceManage dm, [FromServices]runClientDbContext db) {


            dm.run();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
