using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeAllocationRepository : IEmployeeAllocationRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeAllocationRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<EmployeeAllocation>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.EmployeeAllocation
                    .Include(a => a.Employee)
                    .Include(a => a.Role)
                    .Include(a => a.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving employee allocations.", ex);
            }
        }

        public async Task<EmployeeAllocation> GetByIdAsync(int id)
        {
            try
            {
                var allocation = await _employeeDbContext.EmployeeAllocation
                    .Include(a => a.Employee)
                    .Include(a => a.Role)
                    .Include(a => a.Department)
                    .FirstOrDefaultAsync(a => a.Id == id);

                return allocation;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee allocation.", ex);
            }
        }
       

        public async Task AddAsync(AddEmployeeAllocationRequest allocationRequest)
        {
           
           if (await _employeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocationRequest.EmployeeId &&
                        a.RoleId == allocationRequest.RoleId &&
                        a.DepartmentId == allocationRequest.DepartmentId))
           {
              throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
           }

            try
            {
                var allocation = new EmployeeAllocation
                {
                    EmployeeId = allocationRequest.EmployeeId,
                    RoleId = allocationRequest.RoleId,
                    DepartmentId = allocationRequest.DepartmentId
                };

                await _employeeDbContext.EmployeeAllocation.AddAsync(allocation);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the employee allocation.", ex);
            }
        }

        public async Task UpdateAsync(UpdateEmployeeAllocationRequest allocationRequest)
        {
            
            if (await _employeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocationRequest.EmployeeId &&
                        a.RoleId == allocationRequest.RoleId &&
                        a.DepartmentId == allocationRequest.DepartmentId &&
                        a.Id != allocationRequest.Id))
            {
                throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
            }

            try
            {
                var allocation = new EmployeeAllocation
                {
                    EmployeeId = allocationRequest.EmployeeId,
                    RoleId = allocationRequest.RoleId,
                    DepartmentId = allocationRequest.DepartmentId
                };
                _employeeDbContext.EmployeeAllocation.Update(allocation);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the employee allocation.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var allocation = await _employeeDbContext.EmployeeAllocation.FindAsync(id);
                _employeeDbContext.EmployeeAllocation.Remove(allocation);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the employee allocation.", ex);
            }
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
