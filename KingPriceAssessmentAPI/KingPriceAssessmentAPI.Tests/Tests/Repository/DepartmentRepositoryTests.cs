using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingPriceAssessmentAPI.Tests.Repositories
{
    [TestFixture]
    public class DepartmentRepositoryTests 
    {
        private EmployeeDbContext _dbContext;
        private DepartmentRepository _departmentRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new EmployeeDbContext(options);
            _departmentRepository = new DepartmentRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnAllDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, DepartmentName = "HR" },
                new Department { Id = 2, DepartmentName = "IT" }
            };
            _dbContext.Departments.AddRange(departments);
            _dbContext.SaveChanges();

            // Act
            var result = (await _departmentRepository.GetAllAsync()).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(departments.Count));
            Assert.That(result.Any(d => d.DepartmentName == "HR"));
            Assert.That(result.Any(d => d.DepartmentName == "IT"));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenValidId_Then_ShouldReturnDepartment()
        {
            // Arrange
            var department = new Department { Id = 1, DepartmentName = "Finance" };
            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();

            // Act
            var result = await _departmentRepository.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(department.Id));
            Assert.That(result.DepartmentName, Is.EqualTo(department.DepartmentName));
        }

        [Test]
        public async Task When_AddAsyncIsCalled_GivenValidRequest_Then_ShouldAddDepartment()
        {
            // Arrange
            var addRequest = new AddDepartmentRequest { DepartmentName = "Legal" };

            // Act
            await _departmentRepository.AddAsync(addRequest);

            // Assert
            var dbDepartment = _dbContext.Departments.FirstOrDefault(d => d.DepartmentName == "Legal");
            Assert.That(dbDepartment, Is.Not.Null);
            Assert.That(dbDepartment.DepartmentName, Is.EqualTo("Legal"));
        }

        [Test]
        public void When_AddAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _departmentRepository.AddAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentRequest"));
        }

        [Test]
        public void When_UpdateAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _departmentRepository.UpdateAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentRequest"));
        }

        [Test]
        public async Task When_UpdateAsyncIsCalled_GivenValidRequestAndDepartmentExists_Then_ShouldUpdateDepartment()
        {
            // Arrange
            var department = new Department { Id = 1, DepartmentName = "OldName" };
            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();
            var updateRequest = new UpdateDepartmentRequest { Id = 1, DepartmentName = "NewName" };

            // Act
            await _departmentRepository.UpdateAsync(updateRequest);

            // Assert
            var updated = _dbContext.Departments.FirstOrDefault(d => d.Id == 1);
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.DepartmentName, Is.EqualTo("NewName"));
        }

        [Test]
        public void When_UpdateAsyncIsCalled_GivenDepartmentDoesNotExist_Then_ShouldThrowException()
        {
            // Arrange
            var updateRequest = new UpdateDepartmentRequest { Id = 99, DepartmentName = "NonExistent" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _departmentRepository.UpdateAsync(updateRequest));
            //Assert.That(ex.Message, Does.Contain("not found"));
            Assert.That(ex.Message, Does.Contain("An error occurred while updating the department."));
        }

        [Test]
        public async Task When_DeleteAsyncIsCalled_GivenValidId_Then_ShouldRemoveDepartment()
        {
            // Arrange
            var department = new Department { Id = 1, DepartmentName = "Removable" };
            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();

            // Act
            await _departmentRepository.DeleteAsync(1);

            // Assert
            var deleted = _dbContext.Departments.FirstOrDefault(d => d.Id == 1);
            Assert.That(deleted, Is.Null);
        }
    }
}