using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public RoleRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving roles.", ex);
            }
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            try
            {
                var role = await _employeeDbContext.Roles.FindAsync(id);
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the roleRequest.", ex);
            }
        }
        public async Task AddAsync(AddRoleRequest roleRequest)
        {
            if (roleRequest == null)
                throw new ArgumentNullException(nameof(roleRequest));

            try
            {
                var role = new Role
                {
                    RoleName = roleRequest.RoleName
                };
                await _employeeDbContext.Roles.AddAsync(role);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the roleRequest.", ex);
            }
        }
        public async Task UpdateAsync(UpdateRoleRequest roleRequest)
        {
            if (roleRequest == null)
                throw new ArgumentNullException(nameof(roleRequest));

            try
            {
                var existingRole = await _employeeDbContext.Roles.FindAsync(roleRequest.Id);
                if (existingRole == null)
                    throw new Exception($"Role with Id {roleRequest.Id} not found.");

                existingRole.RoleName = roleRequest.RoleName;
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the roleRequest.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var rol = await _employeeDbContext.Roles.FindAsync(id);
                _employeeDbContext.Roles.Remove(rol);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the roleRequest.", ex);
            }
        }

        public async Task<IEnumerable<DepartmentRolesDto>> GetRolesForDepartmentAsync(string departmentName)
        {
            var roles = await (from allocation in _employeeDbContext.EmployeeAllocation
                               join dept in _employeeDbContext.Departments on allocation.DepartmentId equals dept.Id
                               join role in _employeeDbContext.Roles on allocation.RoleId equals role.Id
                               where dept.DepartmentName == departmentName
                               select role.RoleName).Distinct().ToListAsync();

            return new List<DepartmentRolesDto>
    {
        new DepartmentRolesDto
        {
            DepartmentName = departmentName,
            Roles = roles
        }
    };
        }
    }
}
