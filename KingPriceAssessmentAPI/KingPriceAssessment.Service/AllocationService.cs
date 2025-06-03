using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Service
{
    public class AllocationService : IEmployeeAllocationService
    {
        private readonly IEmployeeAllocationRepository _allocationRepository;

        public AllocationService(IEmployeeAllocationRepository allocationRepository)
        {
            _allocationRepository = allocationRepository;
        }

        public async Task<IEnumerable<EmployeeAllocationDto>> GetAllAllocationsAsync()
        {
            var allocations = await _allocationRepository.GetAllAsync();
            return allocations.Select(a => new EmployeeAllocationDto
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.Name,
                RoleId = a.RoleId,
                RoleName = a.Role.RoleName,
                DepartmentId = a.DepartmentId,
                DepartmentName = a.Department.DepartmentName,
            }).ToList();
        }
        public async Task<EmployeeAllocation> GetAllocationByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Allocation ID must be greater than 0", nameof(id));
            }

            var allocation = await _allocationRepository.GetByIdAsync(id);
            if (allocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {id}");
            }
            return allocation;
        }

        public async Task<ResponseMessage> AddAllocationAsync(AddEmployeeAllocationRequest AddEmployeeAllocationRequest)
        {
            if (AddEmployeeAllocationRequest == null)
            {
                throw new ArgumentNullException(nameof(AddEmployeeAllocationRequest));
            }
           
            var allAllocations = await _allocationRepository.GetAllAsync();
            if (allAllocations.Any(a =>
                a.EmployeeId == AddEmployeeAllocationRequest.EmployeeId &&
                a.RoleId == AddEmployeeAllocationRequest.RoleId &&
                a.DepartmentId == AddEmployeeAllocationRequest.DepartmentId))
            {
                throw new ArgumentException("This AddEmployeeAllocationRequest already exists for the specified employee, role, and department.");
            }

            await _allocationRepository.AddAsync(AddEmployeeAllocationRequest);
            return new ResponseMessage
            {
                Success = true,
                Message = "EmployeeAllocation Added successfully.",
            };
        }

        public async Task<ResponseMessage> UpdateAllocationAsync(UpdateEmployeeAllocationRequest employeeAllocationRequest)
        {
            if (employeeAllocationRequest == null)
            {
                throw new ArgumentNullException(nameof(employeeAllocationRequest));
            }

            var existingAllocation = await _allocationRepository.GetByIdAsync(employeeAllocationRequest.Id);
            if (existingAllocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {employeeAllocationRequest.Id}");
            }

            var allAllocations = await _allocationRepository.GetAllAsync();
            if (allAllocations.Any(a =>
                a.EmployeeId == employeeAllocationRequest.EmployeeId &&
                a.RoleId == employeeAllocationRequest.RoleId &&
                a.DepartmentId == employeeAllocationRequest.DepartmentId &&
                a.Id != employeeAllocationRequest.Id))
            {
                throw new ArgumentException("This AddEmployeeAllocationRequest already exists for the specified employee, role, and department.");
            }

            await _allocationRepository.UpdateAsync(employeeAllocationRequest);
            return new ResponseMessage
            {
                Success = true,
                Message = "EmployeeAllocation Updated successfully.",
            };
        }

        public async Task<ResponseMessage> DeleteAllocationAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Allocation ID must be greater than 0", nameof(id));
            }

            var existingAllocation = await _allocationRepository.GetByIdAsync(id);
            if (existingAllocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {id}");
            }

            await _allocationRepository.DeleteAsync(id);
            return new ResponseMessage
            {
                Success = true,
                Message = "EmployeeAllocation Deleted successfully.",
            };
        }

        public async Task<IEnumerable<EmployeeDetailsDto>> GetEmployeesByDepartmentNameAsync(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Department name cannot be empty.", nameof(departmentName));

            return await _allocationRepository.GetEmployeesByDepartmentNameAsync(departmentName);
        }
    }
}