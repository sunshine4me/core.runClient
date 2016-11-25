
using core.phoneDevice.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace core.phoneDevice
{
    public class PhoneModel
    {
        private readonly ILogger<DeviceManage> _logger;
        public PhoneModel(ConcurrentQueue<CustomTask> queue, ILogger<DeviceManage> logger) {
            _logger = logger;
            publicQueue = queue;
            privateQueue = new ConcurrentQueue<CustomTask>();
        }
        public string Device { get; set; }

        public string Model { get; set; }

        public bool Online { get; set; }

        public bool IsRun { get; set; }

        /// <summary>
        /// 最后安装的apk
        /// </summary>
        public ApkInfo LastInstallApk {  get; private set; }


        public ConcurrentQueue<CustomTask> publicQueue;

        private ConcurrentQueue<CustomTask> privateQueue;

        

        public void addPrivateQueue(CustomTask CT) {
            privateQueue.Enqueue(CT);
        }

        public void addPrivateQueue(List<CustomTask> CTS) {
            foreach(var CT in CTS) {
                addPrivateQueue(CT);
            }
        }


        public void runTask() {
           
          
            CustomTask workItem;
            bool dequeueSuccesful = false;
            while (true) {
                if (!Online) break;
                dequeueSuccesful = privateQueue.TryDequeue(out workItem);

                if (!dequeueSuccesful)
                    dequeueSuccesful = publicQueue.TryDequeue(out workItem);

                if (!dequeueSuccesful) break;

                try {
                    _logger.LogInformation("run workItem>>>>>>>>>>>>>>>");
                    workItem.Run(this);
                } catch (Exception e) {
                    _logger.LogError(e.StackTrace);
                    break;
                }
                    
               
            }
          
        }


        public bool installApk(string apkFile, string packageName) {


            if(!string.IsNullOrEmpty(packageName)) {
                var unstall = ExtCommand.Shell("adb", $"-s {Device} uninstall {packageName}").Result;
                _logger.LogInformation($"unstall:{unstall}");
                if (LastInstallApk!= null && LastInstallApk.PackageName == packageName) {
                    LastInstallApk = null;
                }
            }

            //var push = ExtCommand.Shell("adb", $"-s {device} push {apk} /sdcard/").Result;


            var uiautomator = ExtCommand.Shell("adb", $"-s {Device} shell uiautomator runtest DialogDetect.jar -c com.zhongan.test.DetectMain");

            var install = ExtCommand.Shell("adb", $"-s {Device} install -r {apkFile}");

            var installResult = install.Result;
            var uiautomatorResult = uiautomator.Result;
            if(installResult.ToLower().Contains("sucess") || installResult.Contains("[-99]")) {
                this.LastInstallApk = new ApkInfo(apkFile, packageName);

                _logger.LogInformation($"{this.Device}:install {apkFile} is sucess!");
                return true;
            } else {
                _logger.LogError($"{this.Device}:install {apkFile} is fail!");
                _logger.LogError($"{installResult}");
                return false;
            }
        }

     
    }

    public class ApkInfo {

        public ApkInfo(string _apkFile, string _packageName) {
            ApkFile = _apkFile;
            PackageName = _packageName;
        }


        public string ApkFile { get; private set; }
        public string PackageName { get; private set; }
    }
}
