using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using core.phoneDevice.Interface;
using core.phoneDevice;
using core.runClient.DataEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace core.runClient.Task {
    public class SmokeTestTask : CustomTask {

        public static IServiceProvider Provider;

        public int id { get; set; }



        public string ExecuteScript { get; set; }

        public Dictionary<string,string> Param { get; set; }




        public void Run(PhoneModel pm) {


            ExecuteScript = ExecuteScript.Replace("{app}", Param["app"]);
            ExecuteScript = ExecuteScript.Replace("{casefile}", Param["casefile"]);
            ExecuteScript = ExecuteScript.Replace("{result}", Param["result"]);
            ExecuteScript = ExecuteScript.Replace("{device}", pm.device);

            var ExtResult = ExtCommand.Shell(ExecuteScript);

    
            var db = Provider.GetService<runClientDbContext>();
            var jbt = db.JobsTask.FirstOrDefault(t => t.Id == id);
            jbt.RunStatus = 1;
            jbt.Result = ExtResult;
            jbt.Device = pm.device;
            jbt.RunDate = DateTime.Now;
            db.SaveChanges();
            
        }
    }
}
