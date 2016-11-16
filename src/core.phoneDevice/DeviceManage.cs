
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
        public void init() {
            var result = ExtCommand.Shell("adb", "devices");

            foreach (Match mch in Regex.Matches(result, "\\r\\n.*\\tdevice")) {
                string x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device"));
                PhoneModel pd = new PhoneModel(CQs);
                pd.device = x.Trim();
                pml.Add(pd);

            }

            foreach (var pm in pml) {
                string modresult = ExtCommand.Shell("adb", $"-s {pm.device} shell getprop ro.product.model");
                pm.model = modresult.Trim();
            }


        }


       


        public void run() {
            foreach (var pm in pml) {
                if (!pm.isRun)
                    Task.Run(() => pm.runTask());
            }

        }


    }
}
