using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
            => _employeeRepository.GetAllAsync();

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {id}");
            }
            return employee;
        }

        public async Task<ResponseMessage> AddEmployeeAsync(AddEmployeeRequest employeeRequest)
        {

            var allEmployees = await _employeeRepository.GetAllAsync();
            if (allEmployees.Any(e => e.EmployeeNumber.Equals(employeeRequest.EmployeeNumber, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"An employeeRequest with the number '{employeeRequest.EmployeeNumber}' already exists.");
            }

            await _employeeRepository.AddAsync(employeeRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Employee added successfully.",
            };
        }

        public async Task<ResponseMessage> UpdateEmployeeAsync(UpdateEmployeeRequest employeeRequest)
        {
            if (employeeRequest.Id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(employeeRequest.Id));
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeRequest.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {employeeRequest.Id}");
            }

            var allEmployees = await _employeeRepository.GetAllAsync();
            if (allEmployees.Any(e => e.EmployeeNumber.Equals(employeeRequest.EmployeeNumber, StringComparison.OrdinalIgnoreCase) && e.Id != employeeRequest.Id))
            {
                throw new ArgumentException($"An employeeRequest with the number '{employeeRequest.EmployeeNumber}' already exists.");
            }

            await _employeeRepository.UpdateAsync(employeeRequest);

            return new ResponseMessage
            {
                Success = true,
                Message = "Employee Updated successfully.",
            };
        }

        public async Task<ResponseMessage> DeleteEmployeeAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {id}");
            }

            await _employeeRepository.DeleteAsync(id);

            return new ResponseMessage
            {
                Success = true,
                Message = "Employee added successfully.",
            };
        }
    }
}
