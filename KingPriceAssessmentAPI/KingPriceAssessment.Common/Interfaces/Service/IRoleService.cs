using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Service
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<ResponseMessage> AddRoleAsync(AddRoleRequest addRoleRequest);
        Task<ResponseMessage> UpdateRoleAsync(UpdateRoleRequest updateRoleRequest);
        Task<ResponseMessage> DeleteRoleAsync(int id);

        Task<IEnumerable<DepartmentRolesDto>> GetRolesForDepartmentAsync(string departmentName);
    }
}
