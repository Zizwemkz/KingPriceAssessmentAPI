using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Repository
{
    public interface IEmployeeAllocationRepository
    {
        public Task<IEnumerable<EmployeeAllocation>> GetAllAsync();
        public Task<EmployeeAllocation> GetByIdAsync(int id);
        public Task AddAsync(AddEmployeeAllocationRequest allocation);
        public Task UpdateAsync(UpdateEmployeeAllocationRequest allocation);
        public Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeesByDepartmentNameAsync(string departmentName);
    }
}
