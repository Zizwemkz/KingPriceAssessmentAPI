using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Response;

namespace KingPriceAssessment.Common.Interfaces.Repository
{
    public interface IEmployeeDepartmentRepository
    {
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeesByDepartmentNameAsync(string departmentName);
        //Task<IEnumerable<DepartmentRolesDto>> GetRolesForDepartmentAsync(string departmentName);
    }
}
