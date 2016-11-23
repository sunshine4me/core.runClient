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
    }



    public class RunSmokeModel {

        [Required]
        public int id { get; set; }


        [Required]
        [StringLength(200)]
        public string app { get; set; }

        
    }
    



}
