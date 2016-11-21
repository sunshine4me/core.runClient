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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace core.runClient.Controllers
{
    [Authorize]
    public class SmokeTestController : Controller
    {
        runClientDbContext db;
        public SmokeTestController(runClientDbContext _db) {
            db = _db;
        }


        [AllowAnonymous]
        public IActionResult Index() {
            return View(db.SmokeTest.ToList());
        }
        [AllowAnonymous]
        [HttpPost]
        public void RunSmoke(int id, [FromServices] DeviceManage dm) {

            var sm = db.SmokeTest.FirstOrDefault(t => t.Id == id);
            if (sm == null) return;
            
            var jb = new Jobs();
            jb.CreateDate = DateTime.Now;
            jb.TestId = sm.Id;
            jb.TestType = 1;//1-smoke

            db.Jobs.Add(jb);


            DirectoryInfo dir = new DirectoryInfo(sm.FilePath);


            List<CustomTask> STTL = new List<CustomTask>();

            List<JobsTask> jbts = new List<JobsTask>();
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile) {

                var jbt = new JobsTask();
                jbt.JobId = jb.Id;
                jbt.Name = System.IO.Path.GetFileNameWithoutExtension(fi.Name);
                jbt.CaseFilePath = fi.FullName;
                jbt.ResultPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Result_Data/"+ jbt.Id);
                db.JobsTask.Add(jbt);
                jbts.Add(jbt);


            }
            db.SaveChanges();
            foreach (var jbt in jbts) {
                SmokeTestTask STT = new SmokeTestTask();
                STT.CaseFilePath = jbt.CaseFilePath;
                STT.ResultPath = jbt.ResultPath;
                STT.id = jbt.Id;
                STTL.Add(STT);
            }
            dm.addPublicTask(STTL);
            dm.run();
        }


        [HttpGet]
        public IActionResult Edit(int id) {
            var st = db.SmokeTest.First(t => t.Id == id);
            SmokeTestEditModel md = new SmokeTestEditModel();
            md.Id = st.Id;
            md.Name = st.Name;
            md.FilePath = st.FilePath;

            return PartialView("_editSmoke", md);
        }

        [HttpPost]
        public IActionResult Add(SmokeTestAddModel md) {

            if (ModelState.IsValid) {
                var st = new SmokeTest();
             

                st.Name = md.Name;
                st.FilePath = md.FilePath;
                db.SmokeTest.Add(st);
                db.SaveChanges();


            } else {
                
                Response.StatusCode = 400;
                return Content("非法提交!!");
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult Edit(SmokeTestEditModel md) {

            if (ModelState.IsValid) {
                var st = db.SmokeTest.First(t => t.Id == md.Id);

                st.Name = md.Name;
                st.FilePath = md.FilePath;
                db.SaveChanges();
                

            } else {
                Response.StatusCode = 400;
            }
            return Json(md);

        }

        [HttpPost]
        public void Delete(int id) {

            var st = db.SmokeTest.First(t => t.Id == id);
            db.SmokeTest.Remove(st);
            db.SaveChanges();

        }
    }
}
