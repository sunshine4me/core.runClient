using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.ViewModels
{
    public class SmokeTestEditModel
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [StringLength(200)]
        public string FilePath { get; set; }
    }

    public class SmokeTestAddModel {

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [StringLength(200)]
        public string FilePath { get; set; }
    }


    
}
