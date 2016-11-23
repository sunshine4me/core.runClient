using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.ViewModels
{
    public class TaskListModel
    {
        public string Name { get; set; }
        public DateTime? RunDate { get; set; }
        public int RunStatus { get; set; }
        public string CaseFilePath { get; set; }
        public string ResultPath { get; set; }


        public string Device { get; set; }

        public int JobId { get; set; }

        public int Id { get; set; }

        public int? JobType { get; set; }
    }
}
