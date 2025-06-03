using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;

namespace KingPriceAssessment.Data.Models.Request.Update
{
    public class UpdateRoleRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }
    }
}
