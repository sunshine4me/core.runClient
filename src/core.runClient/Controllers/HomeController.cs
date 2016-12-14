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

        [Authorize]
        public IActionResult UnUsed(string device, [FromServices] DeviceManage dm) {
            var pm = dm.pml.First(t => t.Device == device);
            pm.phoneStatus = PhoneStatus.UnUsed;

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Used(string device, [FromServices] DeviceManage dm) {
            var pm = dm.pml.First(t => t.Device == device);
            if (pm.phoneStatus == PhoneStatus.UnUsed)
                pm.phoneStatus = PhoneStatus.OffLine;

            return RedirectToAction("Index");
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
