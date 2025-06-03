using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                var employee = await _employeeDbContext.Employees.FindAsync(id);
                return employee;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task AddAsync(AddEmployeeRequest employeeRequest)
        {
            if (employeeRequest == null) 
            {
                throw new ArgumentNullException(nameof(employeeRequest));
            }
 
            try
            {
                var employee = new Employee
                {
                    EmployeeNumber = employeeRequest.EmployeeNumber,
                    Name = employeeRequest.Name,
                    Lastname = employeeRequest.Lastname,
                    Age = employeeRequest.Age,
                    Position = employeeRequest.Position
                };

                await _employeeDbContext.Employees.AddAsync(employee);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        public async Task UpdateAsync(UpdateEmployeeRequest employeeRequest)
        {
            if (employeeRequest == null)
                throw new ArgumentNullException(nameof(employeeRequest));

            try
            {
                var employee = await _employeeDbContext.Employees.FindAsync(employeeRequest.Id);
                if (employee == null)
                    throw new Exception($"Employee with Id {employeeRequest.Id} not found.");

                employee.EmployeeNumber = employeeRequest.EmployeeNumber;
                employee.Name = employeeRequest.Name;
                employee.Lastname = employeeRequest.Lastname;
                employee.Age = employeeRequest.Age;
                employee.Position = employeeRequest.Position;

                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Employee.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var emp = await _employeeDbContext.Employees.FindAsync(id);
                _employeeDbContext.Employees.Remove(emp);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Employee.", ex);
            }
        }
    }
}
