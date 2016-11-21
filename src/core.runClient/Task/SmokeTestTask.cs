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

        public string CaseFilePath { get; set; }
        public string ResultPath { get; set; }


        public void Run(PhoneModel pm) {
            var date = DateTime.Now;
            Console.WriteLine($">>>>>>startRun:{date},device:{pm.device},CaseFilePath:{CaseFilePath},ResultPath:{ResultPath}");

            Thread.Sleep(20000);
            var db = Provider.GetService<runClientDbContext>();
            var jbt = db.JobsTask.FirstOrDefault(t => t.Id == id);
            jbt.RunStatus = 1;
            jbt.Result = "OKOK";
            db.SaveChanges();

            Console.WriteLine($"<<<<<<endRun:{date}-{DateTime.Now},device:{pm.device}");
        }
    }
}
