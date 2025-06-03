using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync()
            => _roleRepository.GetAllAsync();

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Role ID cannot be 0 or negative");
            }

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {id}");
            }
            return role;

        }
        public async Task<ResponseMessage> AddRoleAsync(AddRoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                throw new ArgumentNullException(nameof(roleRequest));
            }

            if (string.IsNullOrWhiteSpace(roleRequest.RoleName))
            {
                throw new ArgumentException("RoleName cannot be empty or whitespace.", nameof(roleRequest.RoleName));
            }

            var roleRecord = await _roleRepository.GetAllAsync();
            if (roleRecord.Any(x => x.RoleName.Equals(roleRequest.RoleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"A updateRoleRequest with the name '{roleRequest.RoleName}' already exists.");
            }

             await _roleRepository.AddAsync(roleRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Role added successfully.",
            };
        }
             

      public async Task<ResponseMessage> UpdateRoleAsync(UpdateRoleRequest updateRoleRequest)
        {
            if (updateRoleRequest.Id <= 0 || updateRoleRequest == null)
            {
                throw new ArgumentException("Role ID must be 0 or Null", nameof(updateRoleRequest.Id));
            }
            if (string.IsNullOrWhiteSpace(updateRoleRequest.RoleName))
            {
                throw new ArgumentException("RoleName cannot be empty or whitespace.", nameof(updateRoleRequest.RoleName));
            }

            var existingRole = await _roleRepository.GetByIdAsync(updateRoleRequest.Id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {updateRoleRequest.Id}");
            }

            var roleRecord = await _roleRepository.GetAllAsync();
            if (roleRecord.Any(x => x.RoleName.Equals(updateRoleRequest.RoleName, StringComparison.OrdinalIgnoreCase) && x.Id != updateRoleRequest.Id))
            {
                throw new ArgumentException($"A updateRoleRequest with the name '{updateRoleRequest.RoleName}' already exists.");
            }

            await _roleRepository.UpdateAsync(updateRoleRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Role uodated successfully.",
            };
        }

        public async Task<ResponseMessage> DeleteRoleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Role ID must be greater than 0", nameof(id));
            }

            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {id}");
            }

            await _roleRepository.DeleteAsync(id);

            return new ResponseMessage
            {
                Success = true,
                Message = "Role Deleted successfully.",
            };
        }

        public async Task<IEnumerable<DepartmentRolesDto>> GetRolesForDepartmentAsync(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Department name cannot be empty.", nameof(departmentName));

            return await _roleRepository.GetRolesForDepartmentAsync(departmentName);
        }
    }
}
