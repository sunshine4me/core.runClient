using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.runClient.DataEntities;
using core.runClient.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace core.runClient.Controllers
{
    public class JobController : Controller
    {
        runClientDbContext db;
        public JobController(runClientDbContext _db) {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult Index(int page=1,int row =20)
        {

            var jbs = from JB in db.Jobs
                      join ST in db.SmokeTest
                      on JB.TestId equals ST.Id into JoinedEmpDept
                      from JBST in JoinedEmpDept.DefaultIfEmpty()
                      select new JobListModel {
                          Describe = JB.TestType == 1 ? "SmokeTest-" + JBST.Id + "-" + JBST.Name : "Unknown Type",
                          TaskCnt = JB.JobsTask.Count,
                          Id = JB.Id,
                          JobType = JB.TestType,
                          CreateDate = JB.CreateDate
                      };


            ViewBag.total = db.Jobs.Count();
            ViewBag.page = page;
            ViewBag.row = row;
            return View(jbs.Skip((page - 1) * row).Take(row).ToList());
        }
    }
}
