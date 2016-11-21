using System;
using System.Collections.Generic;

namespace core.runClient.DataEntities {
    public partial class JobsTask
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }

        public string CaseFilePath { get; set; }

        public string ResultPath { get; set; }
        
        public string Result { get; set; }
        public int RunStatus { get; set; }

        

        public DateTime? RunDate { get; set; }

        

        public virtual Jobs Job { get; set; }
    }
}
