using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.ViewModels
{
    public class SmokeTestEditModel: SmokeTestAddModel {
        [Required]
        public int Id { get; set; }

    }

    public class SmokeTestAddModel {

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }


        [Required]
        [StringLength(500)]
        public string ExecuteScript { get; set; }

        [StringLength(50)]
        public string PassMatch { get; set; }
    }



    public class RunSmokeModel {

        [Required]
        public int id { get; set; }


        [Required]
        [StringLength(200)]
        public string appfile { get; set; }


        [Required]
        [StringLength(200)]
        public string packagename { get; set; }

    }



    public class SmokeTaskListModel {
        public string Name { get; set; }
        public DateTime? RunDate { get; set; }
        public int RunStatus { get; set; }
        public string CaseFilePath { get; set; }
        public string ResultPath { get; set; }

        public string Result { get; set; }


        public string Device { get; set; }

        public int JobId { get; set; }

        public int Id { get; set; }


    }


    public class SmokeJobListModel {
        public string Describe { get; set; }
        public int Id { get; set; }

        public int TaskCnt { get; set; }

        public DateTime CreateDate { get; set; }


    }


}
