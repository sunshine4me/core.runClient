using core.phoneDevice.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.Extensions
{
    public static class TaskConvent
    {
        public static CustomTask ConventToTask(this DataEntities.SmokeTestJobTask jt) {

            runClient.Task.SmokeTestTask CT = new runClient.Task.SmokeTestTask();
            CT.id = jt.Id;
            CT.ExecuteScript = jt.ExecuteScript;
            CT.PassMatch = jt.PassMatch;


            CT.ResultPath = Path.Combine(Directory.GetCurrentDirectory(), $"ResultFile{Path.DirectorySeparatorChar}{jt.Id}");


            CT.PackageName = jt.PackageName;

            CT.InstallApkFile = jt.InstallApkFile;


            return CT;
        }
    }
}
