using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingPriceAssessment.Data.Models.Request.Add
{
    public class AddRoleRequest
    {
        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }
    }
}
