
using core.phoneDevice.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace core.phoneDevice {
    public class DeviceManage {
        public List<PhoneModel> pml;

        ConcurrentQueue<CustomTask> CQs;
        public DeviceManage() {
            pml = new List<PhoneModel>();
            CQs = new ConcurrentQueue<CustomTask>();
            init();


        }
        private void init() {
            var result = ExtCommand.Shell("adb", "devices");
            foreach (Match mch in Regex.Matches(result, "\\n.*\\tdevice")) {
                string x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device")).Trim();
                if (pml.FirstOrDefault(t => t.device == x) == null) {
                    PhoneModel pm = new PhoneModel(CQs);
                    pm.device = x;
                    pm.online = true;
                    string modresult = ExtCommand.Shell("adb", $"-s {pm.device} shell getprop ro.product.model");
                    pm.model = modresult.Trim();
                    pml.Add(pm);
                }
            }
        }

        public void RefreshDevice() {

            var ds = new List<string>();

            var result = ExtCommand.Shell("adb", "devices");


            foreach (Match mch in Regex.Matches(result, "\\n.*\\tdevice")) {
                string x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device")).Trim();
                ds.Add(x);
                if (pml.FirstOrDefault(t => t.device == x) == null) {
                    PhoneModel pm = new PhoneModel(CQs);
                    pm.device = x;
                    pm.online = true;
                    string modresult = ExtCommand.Shell("adb", $"-s {pm.device} shell getprop ro.product.model");
                    pm.model = modresult.Trim();
                    pml.Add(pm);
                }
            }

            pml.Where(t => !ds.Contains(t.device)).ToList().ForEach(t => t.online = false);

            pml.Where(t => ds.Contains(t.device)).ToList().ForEach(t => t.online = true);

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
                if (!pm.isRun) {
                    new Action(() => {
                        pm.runTask();
                    }).BeginInvoke(r => {
                        if (r.IsCompleted) {
                            pm.isRun = false;
                        }
                    }, null);
                }
            }
        }


    }
}
