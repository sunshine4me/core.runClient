using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.phoneDevice;

namespace core.runClient.Controllers
{
    public class HomeController : Controller
    {
        private DeviceManage dm;
        public HomeController(DeviceManage _dm) {
            dm = _dm;
        }
        public IActionResult Index()
        {
            return View(dm.pml);
        }

       

        public IActionResult Error()
        {
            return View();
        }
    }
}
