using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<ResponseMessage> AddEmployeeAsync(AddEmployeeRequest employee);
        Task<ResponseMessage> UpdateEmployeeAsync(UpdateEmployeeRequest employee);
        Task<ResponseMessage> DeleteEmployeeAsync(int id);
    }
}
