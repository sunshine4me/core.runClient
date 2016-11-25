using System;
using System.Collections.Generic;

namespace core.runClient.DataEntities {
    public partial class SmokeTestJobTask
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }
        public string ExecuteScript { get; set; }
        public string ResultPath { get; set; }
        public string Device { get; set; }
        public DateTime RunDate { get; set; }
        public string PassMatch { get; set; }
        public string ExecuteScriptResult { get; set; }
        public int RunStatus { get; set; }

        public string PackageName { get; set; }

        public string InstallApkFile { get; set; }


        public virtual SmokeTestJob Job { get; set; }
    }
}
