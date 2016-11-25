using System;
using System.Collections.Generic;

namespace core.runClient.DataEntities {
    public partial class SmokeTest
    {
        public SmokeTest()
        {
            SmokeTestJob = new HashSet<SmokeTestJob>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string ExecuteScript { get; set; }
        public string PassMatch { get; set; }

        public virtual ICollection<SmokeTestJob> SmokeTestJob { get; set; }
    }
}
