using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingPriceAssessment.Data.Models.Request.Update
{
    public class UpdateEmployeeRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Range(18, 70, ErrorMessage = "Age must be between 18 and 70.")]
        public int Age { get; set; }

        [Required]
        [StringLength(30)]
        public string Position { get; set; }
    }
}
