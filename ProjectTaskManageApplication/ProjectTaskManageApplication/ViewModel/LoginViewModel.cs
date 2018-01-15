using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTaskManageApplication.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
}
