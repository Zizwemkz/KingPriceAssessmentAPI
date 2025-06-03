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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task<IEnumerable<Department>> GetAllDepartmentsAsync()
            => _departmentRepository.GetAllAsync();

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(id));
            }

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {id}");
            }
            return department;
        }

        public async Task<ResponseMessage> AddDepartmentAsync(AddDepartmentRequest departmentRequest)
        {
            if (departmentRequest == null)
            {
                throw new ArgumentNullException(nameof(departmentRequest));
            }

            if (string.IsNullOrWhiteSpace(departmentRequest.DepartmentName))
            {
                throw new ArgumentException("DepartmentName cannot be empty or whitespace.", nameof(departmentRequest.DepartmentName));
            }

            var allDepartments = await _departmentRepository.GetAllAsync();
            if (allDepartments.Any(d => d.DepartmentName.Equals(departmentRequest.DepartmentName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"A departmentRequest with the name '{departmentRequest.DepartmentName}' already exists.");
            }
           
            await _departmentRepository.AddAsync(departmentRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Department Added successfully.",
            };
        }

        public async Task<ResponseMessage> UpdateDepartmentAsync(UpdateDepartmentRequest departmentRequest)
        {
            if (departmentRequest.Id <= 0 || departmentRequest ==  null)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(departmentRequest.Id));
            }
            if (string.IsNullOrWhiteSpace(departmentRequest.DepartmentName))
            {
                throw new ArgumentException("DepartmentName cannot be empty or whitespace.", nameof(departmentRequest.DepartmentName));
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(departmentRequest.Id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {departmentRequest.Id}");
            }

            var allDepartments = await _departmentRepository.GetAllAsync();
            if (allDepartments.Any(d => d.DepartmentName.Equals(departmentRequest.DepartmentName, StringComparison.OrdinalIgnoreCase) && d.Id != departmentRequest.Id))
            {
                throw new ArgumentException($"A departmentRequest with the name '{departmentRequest.DepartmentName}' already exists.");
            }

            await _departmentRepository.UpdateAsync(departmentRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Department Updated successfully.",
            };
        }

        public async Task<ResponseMessage> DeleteDepartmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(id));
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {id}");
            }

            await _departmentRepository.DeleteAsync(id);

            return new ResponseMessage
            {
                Success = true,
                Message = "Department Deleted successfully.",
            };
        }
    }
}