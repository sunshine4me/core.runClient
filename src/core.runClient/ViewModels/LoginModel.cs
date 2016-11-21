using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core.runClient.ViewModels {
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "英文开头,并只能使用英文、数字或_(下划线)")]
        [StringLength(20, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20)]
        public string Password { get; set; }
        
    }

}
