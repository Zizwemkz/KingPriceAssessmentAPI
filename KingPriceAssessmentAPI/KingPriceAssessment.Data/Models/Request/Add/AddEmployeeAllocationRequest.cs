using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingPriceAssessment.Data.Models.Request.Add
{
    public class AddEmployeeAllocationRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public void validation()
        {
            if (EmployeeId <= 0)
            {
                throw new ArgumentException("EmployeeId must be greater than 0.", nameof(EmployeeId));
            }
            if (RoleId <= 0)
            {
                throw new ArgumentException("RoleId must be greater than 0.", nameof(RoleId));
            }
            if (DepartmentId <= 0)
            {
                throw new ArgumentException("DepartmentId must be greater than 0.", nameof(DepartmentId));
            }
        }
    }
}
