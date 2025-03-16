using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SystemAccountDTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        public string AccountPassword { get; set; }
    }
}

