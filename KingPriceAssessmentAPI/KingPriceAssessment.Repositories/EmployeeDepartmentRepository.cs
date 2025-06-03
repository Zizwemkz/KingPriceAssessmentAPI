using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeDepartmentRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeDepartmentRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<EmployeeDetailsDto>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
            var query = from allocation in _employeeDbContext.EmployeeAllocation
            join emp in _employeeDbContext.Employees on allocation.EmployeeId equals emp.Id
                        join role in _employeeDbContext.Roles on allocation.RoleId equals role.Id
                        join dept in _employeeDbContext.Departments on allocation.DepartmentId equals dept.Id
                        where dept.DepartmentName == departmentName
                        select new EmployeeDetailsDto
                        {
                            EmployeeId = emp.Id,
                            EmployeeNumber = emp.EmployeeNumber,
                            Name = emp.Name,
                            Lastname = emp.Lastname,
                            Position = emp.Position,
                            RoleName = role.RoleName,
                            DepartmentName = dept.DepartmentName
                        };

            return await query.ToListAsync();
        }
    }
}
