using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Service
{
    public interface IEmployeeAllocationService
    {
        public Task<IEnumerable<EmployeeAllocation>> GetAllAllocationsAsync();
        Task<EmployeeAllocation> GetAllocationByIdAsync(int id);
        Task<ResponseMessage> AddAllocationAsync(AddEmployeeAllocationRequest allocation);
        Task<ResponseMessage> UpdateAllocationAsync(UpdateEmployeeAllocationRequest allocation);
        Task<ResponseMessage> DeleteAllocationAsync(int id);
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeesByDepartmentNameAsync(string departmentName);
    }
}
