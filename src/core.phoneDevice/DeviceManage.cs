
using core.phoneDevice.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace core.phoneDevice {
    public class DeviceManage {

        public List<PhoneModel> pml;

        public ConcurrentQueue<CustomTask> CQs;

        private readonly ILogger<DeviceManage> _logger;

        public DeviceManage(ILogger<DeviceManage> logger) {
            _logger = logger;
            pml = new List<PhoneModel>();
            CQs = new ConcurrentQueue<CustomTask>();
            initPhone();

        }

        public int getQueueCount() {
            return CQs.Count;
        }



        private void initPhone() {
            var result = ExtCommand.Shell("adb", "devices").Result;

            var result2 = ExtCommand.Shell("adb devices").Result;
            foreach (Match mch in Regex.Matches(result, "\\n.*\\tdevice")) {
                string x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device")).Trim();
                if (pml.FirstOrDefault(t => t.Device == x) == null) {
                    PhoneModel pm = new PhoneModel(CQs, _logger);
                    pm.Device = x;
                    pm.Online = true;
                    string modresult = ExtCommand.Shell("adb", $"-s {pm.Device} shell getprop ro.product.model").Result;
                    pm.Model = modresult.Trim();


                    //push 安装用的jar 包
                    var ss = ExtCommand.Shell("adb", $"-s {pm.Device} push {Path.Combine(Directory.GetCurrentDirectory(), @"Tools/DialogDetect.jar")} /data/local/tmp/").Result;
                    _logger.LogInformation(ss);

                    pml.Add(pm);
                }
            }
            
        }

        public void RefreshDevice() {

            var ds = new List<string>();

            var result = ExtCommand.Shell("adb", "devices");


            foreach (Match mch in Regex.Matches(result.Result, "\\n.*\\tdevice")) {
                string x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device")).Trim();
                ds.Add(x);
                if (pml.FirstOrDefault(t => t.Device == x) == null) {
                    PhoneModel pm = new PhoneModel(CQs, _logger);
                    pm.Device = x;
                    pm.Online = true;
                    string modresult = ExtCommand.Shell("adb", $"-s {pm.Device} shell getprop ro.product.model").Result;
                    pm.Model = modresult.Trim();

                    //push 安装用的jar 包
                    var ss = ExtCommand.Shell("adb", $"-s {pm.Device} push {Path.Combine(Directory.GetCurrentDirectory(), @"Tools\DialogDetect.jar")} /data/local/tmp/").Result;
                    _logger.LogInformation(ss);

                    pml.Add(pm);
                }
            }

            pml.Where(t => !ds.Contains(t.Device)).ToList().ForEach(t => t.Online = false);

            pml.Where(t => ds.Contains(t.Device)).ToList().ForEach(t => t.Online = true);

        }



        public void addPublicTask(CustomTask CT) {

            CQs.Enqueue(CT);

        }

        public void addPublicTask(List<CustomTask> CTS) {
            foreach (var CT in CTS) {
                CQs.Enqueue(CT);
            }
        }


        public void run() {
            foreach (var pm in pml) {
                if (!pm.IsRun) {
                    pm.IsRun = true;
                    Task.Run(() => {
                        try {
                            _logger.LogInformation($"{pm.Device}:run start!");
                            pm.runTask();
                            _logger.LogInformation($"{pm.Device}:run over!");
                        } catch (Exception e) {
                            _logger.LogError(e.StackTrace);
                        }
                        pm.IsRun = false;
                    });
                }
            }
        }


    }
}
