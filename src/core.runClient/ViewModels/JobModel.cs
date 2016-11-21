using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.ViewModels
{
    public class JobListModel
    {
        public string Describe { get; set; }
        public int Id { get; set; }
        public int? JobType { get; set; }

        public int TaskCnt { get; set; }

        public DateTime CreateDate { get; set; }



    }
}
