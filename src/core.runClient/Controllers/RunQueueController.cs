using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.phoneDevice;
using core.runClient.DataEntities;
using core.runClient.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace core.runClient.Controllers
{
    public class RunQueueController : Controller
    {

        runClientDbContext db;
        public RunQueueController(runClientDbContext _db) {
            db = _db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var md = from t in db.SmokeTestJobTask
                     orderby t.Id descending
                     where t.RunStatus==0
                     select new QueueListModel {
                         Name = t.Name,
                         Type = "Smoke Test",
                         CreateDate = t.Job.CreateDate
                     };

            ViewBag.total = db.SmokeTestJobTask.Count();
            return View(md.ToList());
        }
    }
}
