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
    public class TaskController : Controller
    {
        runClientDbContext db;
        public TaskController(runClientDbContext _db) {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int row = 50) {

            var ts = from t in db.JobsTask
                     where t.RunStatus == 0
                     orderby t.Id descending
                     select new TaskListModel {
                         Name = t.Name,
                         RunStatus = t.RunStatus,
                         RunDate = t.RunDate,
                         JobId = t.JobId,
                         //CaseFilePath = t.CaseFilePath,
                         //ResultPath= t.ResultPath,
                         JobType = t.Job.TestType
                     };


            ViewBag.total = db.JobsTask.Count(t => t.RunStatus == 0);
            ViewBag.page = page;
            ViewBag.row = row;

            return View(ts.Skip((page - 1) * row).Take(row).ToList());
        }


        public IActionResult ByJob(int id) {

            var ts = from t in db.JobsTask
                     where t.JobId == id
                     orderby t.Id descending
                     select new TaskListModel {
                         Id = t.Id,
                         Name = t.Name,
                         RunStatus = t.RunStatus,
                         RunDate = t.RunDate,
                         JobId = t.JobId,
                         Device = t.Device,
                         //CaseFilePath = t.CaseFilePath,
                         //ResultPath= t.ResultPath,
                         JobType = t.Job.TestType
                     };



            return View(ts.ToList());
        }
    }
}
