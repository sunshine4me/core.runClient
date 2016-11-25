using System;
using System.Collections.Generic;

namespace core.runClient.DataEntities {
    public partial class SmokeTestJob
    {
        public SmokeTestJob()
        {
            SmokeTestJobTask = new HashSet<SmokeTestJobTask>();
        }

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int SmokeId { get; set; }

        public virtual ICollection<SmokeTestJobTask> SmokeTestJobTask { get; set; }
        public virtual SmokeTest Smoke { get; set; }
    }
}
