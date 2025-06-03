using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Data.Models.Request.Add
{
    public class AddEmployeeRequest
    {
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

        public void validate()
        {

            if (string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                throw new ArgumentException("EmployeeNumber cannot be empty or whitespace.", nameof(EmployeeNumber));
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(Name));
            }
            if (string.IsNullOrWhiteSpace(Lastname))
            {
                throw new ArgumentException("Lastname cannot be empty or whitespace.", nameof(Lastname));
            }
            if (string.IsNullOrWhiteSpace(Position))
            {
                throw new ArgumentException("Position cannot be empty or whitespace.", nameof(Position));
            }
            if (Age < 18 || Age > 65)
            {
                throw new ArgumentException("Age must be between 18 and 65.", nameof(Age));
            }
        }
    }

    
}
