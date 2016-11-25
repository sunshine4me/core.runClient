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
using System.Text.RegularExpressions;

namespace core.runClient.Task {
    public class SmokeTestTask : CustomTask {

        public static IServiceProvider Provider;

        public int id { get; set; }



        public string ExecuteScript { get; set; }

        public Dictionary<string,string> Param { get; set; }

        public string PassMatch { get; set; }

        public string InstallApkFile { get; set; }
     
        public string PackageName { get; set; }


        public string ResultPath { get; set; }




        public void Run(PhoneModel pm) {

            string ExtResult = "";

            if (!string.IsNullOrEmpty(InstallApkFile)) {
                if(pm.LastInstallApk == null || pm.LastInstallApk.ApkFile != InstallApkFile) {
                    try {
                        if (pm.installApk(InstallApkFile, PackageName))
                            ExtResult += $"安装apk成功{System.Environment.NewLine}";
                        else
                            ExtResult += $"安装apk失败:请查看日志{System.Environment.NewLine}";
                    } catch (Exception e) {
                        ExtResult += $"安装apk失败:{e.StackTrace}{System.Environment.NewLine}";
                    }

                } else {
                    ExtResult += $"本机已安装该apk,跳过安装步骤{System.Environment.NewLine}";
                }
            }

            ExecuteScript = ExecuteScript.Replace("{result}", ResultPath);
            ExecuteScript = ExecuteScript.Replace("{device}", pm.Device);


            int rs = 2;
            try {
                ExtResult +=  ExtCommand.Shell(ExecuteScript).Result;
                if (string.IsNullOrEmpty(PassMatch) || Regex.IsMatch(ExtResult, PassMatch))
                    rs = 1;
            } catch (Exception e) {
                ExtResult += e.StackTrace;
            }
            


            var db = Provider.GetService<runClientDbContext>();
            var jbt = db.SmokeTestJobTask.FirstOrDefault(t => t.Id == id);
            jbt.RunStatus = rs;
            jbt.ExecuteScriptResult = ExtResult;
            jbt.Device = pm.Device;
            jbt.ResultPath = ResultPath;
            jbt.RunDate = DateTime.Now;
            db.SaveChanges();
            
        }
    }
}
