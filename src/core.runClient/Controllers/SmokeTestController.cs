using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core.runClient.DataEntities;
using core.phoneDevice;
using System.IO;
using core.runClient.Task;
using core.phoneDevice.Interface;
using core.runClient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using core.runClient.Extensions;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace core.runClient.Controllers {
    
    public class SmokeTestController : Controller {
        runClientDbContext db;
        public SmokeTestController(runClientDbContext _db) {
            db = _db;
        }


   
        public IActionResult Index() {
            return View(db.SmokeTest.ToList());
        }
 
        [HttpPost]
        public IActionResult RunSmoke(RunSmokeModel rsm, [FromServices] DeviceManage dm) {

            if (!ModelState.IsValid) {
                Response.StatusCode = 400;
                return Content("ModelState is not Valid!");
            }

            var sm = db.SmokeTest.FirstOrDefault(t => t.Id == rsm.id);
            if (sm == null) {
                Response.StatusCode = 400;
                return Content("Not found your id!"); ;
            }


            var jb = new SmokeTestJob();
            jb.CreateDate = DateTime.Now;
            jb.SmokeId = sm.Id;
            db.SmokeTestJob.Add(jb);


            DirectoryInfo dir = new DirectoryInfo(sm.FilePath);


            List<CustomTask> STTL = new List<CustomTask>();

            List<SmokeTestJobTask> jbts = new List<SmokeTestJobTask>();
            FileInfo[] allFile = dir.GetFiles();

            foreach (FileInfo fi in allFile) {
                var jbt = new SmokeTestJobTask();
                jbt.JobId = jb.Id;
                jbt.Name = System.IO.Path.GetFileNameWithoutExtension(fi.Name);

                jbt.ExecuteScript = sm.ExecuteScript.Replace("{casefile}", fi.FullName);

                jbt.PassMatch = sm.PassMatch;

                jbt.PackageName = rsm.packagename;
                jbt.InstallApkFile = rsm.appfile;
                db.SmokeTestJobTask.Add(jbt);
                jbts.Add(jbt);
            }
            db.SaveChanges();



            foreach (var jbt in jbts) {
                var CT = jbt.ConventToTask();
                dm.addPublicTask(CT);
            }

            dm.run();

            return Content("/SmokeTest/JobReport/"+ jb.Id);
        }




        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id) {
            var st = db.SmokeTest.First(t => t.Id == id);
            SmokeTestEditModel md = new SmokeTestEditModel();
            md.Id = st.Id;
            md.Name = st.Name;
            md.FilePath = st.FilePath;
            md.ExecuteScript = st.ExecuteScript;
            md.PassMatch = st.PassMatch;

            return PartialView("_editSmoke", md);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(SmokeTestEditModel md) {

            if (ModelState.IsValid) {
                var st = db.SmokeTest.First(t => t.Id == md.Id);

                st.ExecuteScript = md.ExecuteScript;
                st.Name = md.Name;
                st.FilePath = md.FilePath;
                st.PassMatch = md.PassMatch;
                db.SaveChanges();


            } else {
                Response.StatusCode = 400;
            }
            return Json(md);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(SmokeTestAddModel md) {

            if (ModelState.IsValid) {
                var st = new SmokeTest();

                st.ExecuteScript = md.ExecuteScript;
                st.Name = md.Name;
                st.FilePath = md.FilePath;
                st.PassMatch = md.PassMatch;
                db.SmokeTest.Add(st);
                db.SaveChanges();


            } else {

                Response.StatusCode = 400;
                return Content("非法提交!!");
            }
            return RedirectToAction("Index");

        }


        [Authorize]
        [HttpPost]
        public void Delete(int id) {

            var st = db.SmokeTest.First(t => t.Id == id);
            db.SmokeTest.Remove(st);
            db.SaveChanges();

        }


        [HttpGet]
        public IActionResult Job(int page = 1, int row = 20) {

            var jbs = from JB in db.SmokeTestJob
                      orderby JB.Id descending
                      select new SmokeJobListModel {
                          Describe = "SmokeTest-" + JB.Smoke.Id + "-" + JB.Smoke.Name ,
                          TaskCnt = JB.SmokeTestJobTask.Count,
                          Id = JB.Id,
                          CreateDate = JB.CreateDate
                      };


            ViewBag.total = db.SmokeTestJob.Count();
            ViewBag.page = page;
            ViewBag.row = row;
            return View(jbs.Skip((page - 1) * row).Take(row).ToList());

        }
        

        [HttpGet]
        public IActionResult JobReport(int id) {

            var ts = from t in db.SmokeTestJobTask
                     where t.JobId == id
                     orderby t.Id descending
                     select new SmokeTaskListModel {
                         Id = t.Id,
                         Name = t.Name,
                         RunStatus = t.RunStatus,
                         RunDate = t.RunDate,
                         JobId = t.JobId,
                         Device = t.Device,
                         Result = t.ExecuteScriptResult,
                         ResultPath = t.ResultPath
                     };



            return View(ts.ToList());

        }

    }


}
