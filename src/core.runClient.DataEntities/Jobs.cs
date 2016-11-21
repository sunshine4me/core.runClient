using System;
using System.Collections.Generic;

namespace core.runClient.DataEntities {
    public partial class Jobs
    {
        public Jobs()
        {
            JobsTask = new HashSet<JobsTask>();
        }

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int TestId { get; set; }
        public int? TestType { get; set; }

        public virtual ICollection<JobsTask> JobsTask { get; set; }
    }
}
