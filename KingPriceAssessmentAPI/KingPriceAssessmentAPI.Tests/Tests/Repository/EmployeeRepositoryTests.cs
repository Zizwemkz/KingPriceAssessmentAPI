using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Repositories;
using KingPriceAssessmentAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPriceAssessmentAPI.Tests.Repositories
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private EmployeeDbContext _dbContext;
        private EmployeeRepository _employeeRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            _dbContext = new EmployeeDbContext(options);
            _employeeRepository = new EmployeeRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new EmployeeRepositoryTestBuilder().BuildEmployeeList(2).ToList();
            _dbContext.Employees.AddRange(employees);
            _dbContext.SaveChanges();

            // Act
            var result = await _employeeRepository.GetAllAsync();

            // Assert
            Assert.That(result, Is.EquivalentTo(employees));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenValidId_Then_ShouldReturnEmployee()
        {
            // Arrange
            var employee = new EmployeeRepositoryTestBuilder().WithId(1).BuildEmployee();
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            // Act
            var result = await _employeeRepository.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(employee.Id));
            Assert.That(result.EmployeeNumber, Is.EqualTo(employee.EmployeeNumber));
        }

        [Test]
        public async Task When_AddAsyncIsCalled_GivenValidRequest_Then_ShouldAddEmployee()
        {
            // Arrange
            var addRequest = new EmployeeRepositoryTestBuilder().BuildAddEmployeeRequest();

            // Act
            await _employeeRepository.AddAsync(addRequest);

            // Assert
            var dbEmployee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeNumber == addRequest.EmployeeNumber);
            Assert.That(dbEmployee, Is.Not.Null);
            Assert.That(dbEmployee.Name, Is.EqualTo(addRequest.Name));
        }

        [Test]
        public void When_AddAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _employeeRepository.AddAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("employeeRequest"));
        }

        [Test]
        public void When_UpdateAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _employeeRepository.UpdateAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("employeeRequest"));
        }

        [Test]
        public async Task When_DeleteAsyncIsCalled_GivenValidId_Then_ShouldRemoveEmployee()
        {
            // Arrange
            var employee = new EmployeeRepositoryTestBuilder().WithId(1).BuildEmployee();
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            // Act
            await _employeeRepository.DeleteAsync(1);

            // Assert
            var deletedEmployee = _dbContext.Employees.FirstOrDefault(e => e.Id == 1);
            Assert.That(deletedEmployee, Is.Null);
        }
    }
}