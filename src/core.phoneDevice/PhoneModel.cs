
using core.phoneDevice.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.phoneDevice
{
    public class PhoneModel
    {
        public PhoneModel(ConcurrentQueue<CustomTask> queue) {
            publicQueue = queue;
            privateQueue = new ConcurrentQueue<CustomTask>();
        }
        public string device { get; set; }

        public string model { get; set; }

        public bool online { get; set; }

        public bool isRun { get; set; }


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
            Console.WriteLine($"{device}工作线程开始>>>>>>>>>>>");
            isRun = true;
            CustomTask workItem;
            bool dequeueSuccesful = false;
            while (true) {
                if (!online) break;
                dequeueSuccesful = privateQueue.TryDequeue(out workItem);

                if (!dequeueSuccesful)
                    dequeueSuccesful = publicQueue.TryDequeue(out workItem);
          
                if (dequeueSuccesful)
                    workItem.Run(this);
                else
                    break;
            }
            isRun = false;
            Console.WriteLine($"{device}工作线程关闭<<<<<<<<<<<");
        }
    }
}
