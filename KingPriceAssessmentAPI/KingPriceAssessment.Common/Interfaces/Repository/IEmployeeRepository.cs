using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task AddAsync(AddEmployeeRequest employee);
        Task UpdateAsync(UpdateEmployeeRequest employee);
        Task DeleteAsync(int id);
    }
}
