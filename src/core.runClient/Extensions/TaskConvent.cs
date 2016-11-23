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
        public static CustomTask ConventToTask(this DataEntities.JobsTask jt) {

            runClient.Task.SmokeTestTask CT = new runClient.Task.SmokeTestTask();
            CT.id = jt.Id;
            CT.ExecuteScript = jt.ExecuteScript;
            CT.Param = JsonConvert.DeserializeObject<Dictionary<string, string>>(jt.Param);
            CT.Param["result"] = Path.Combine(Directory.GetCurrentDirectory(), "ResultFile/" + jt.Id);
            CT.Param["casefile"] = jt.CaseFilePath;
            return CT;
        }
    }
}
