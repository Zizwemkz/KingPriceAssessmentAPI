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
    public interface IRoleRepository
    {
        public Task<IEnumerable<Role>> GetAllAsync();
        public Task<Role> GetByIdAsync(int id);
        public Task AddAsync(AddRoleRequest addRoleRequest);
        public Task UpdateAsync(UpdateRoleRequest addRoleRequest);
        public Task DeleteAsync(int id);
        Task<IEnumerable<DepartmentRolesDto>> GetRolesForDepartmentAsync(string departmentName);
    }
}
