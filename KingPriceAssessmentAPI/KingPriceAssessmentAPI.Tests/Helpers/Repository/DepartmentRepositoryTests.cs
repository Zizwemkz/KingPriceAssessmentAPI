using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

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
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var dept1 = new Department { Id = 1, DepartmentName = "HR" };
            var dept2 = new Department { Id = 2, DepartmentName = "IT" };
            _dbContext.Departments.AddRange(dept1, dept2);
            _dbContext.SaveChanges();

            // Act
            var result = await _departmentRepository.GetAllAsync();

            // Assert
            Assert.That(result, Is.EquivalentTo(new[] { dept1, dept2 }));
        }

        [Test]
        public async Task GetByIdAsync_GivenValidId_ShouldReturnDepartment()
        {
            // Arrange
            var dept = new Department { Id = 1, DepartmentName = "HR" };
            _dbContext.Departments.Add(dept);
            _dbContext.SaveChanges();

            // Act
            var result = await _departmentRepository.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.DepartmentName, Is.EqualTo("HR"));
        }

        [Test]
        public async Task AddAsync_GivenValidRequest_ShouldAddDepartment()
        {
            // Arrange
            var addRequest = new AddDepartmentRequest { DepartmentName = "Finance" };

            // Act
            await _departmentRepository.AddAsync(addRequest);

            // Assert
            var dbDept = _dbContext.Departments.FirstOrDefault(d => d.DepartmentName == "Finance");
            Assert.That(dbDept, Is.Not.Null);
            Assert.That(dbDept.DepartmentName, Is.EqualTo("Finance"));
        }

        [Test]
        public void AddAsync_GivenNullRequest_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _departmentRepository.AddAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentRequest"));
        }

        [Test]
        public async Task UpdateAsync_GivenValidRequest_ShouldUpdateDepartment()
        {
            // Arrange
            var dept = new Department { Id = 1, DepartmentName = "OldDept" };
            _dbContext.Departments.Add(dept);
            _dbContext.SaveChanges();

            var updateRequest = new UpdateDepartmentRequest { Id = 1, DepartmentName = "NewDept" };

            // Act
            await _departmentRepository.UpdateAsync(updateRequest);

            // Assert
            var updatedDept = _dbContext.Departments.FirstOrDefault(d => d.Id == 1);
            Assert.That(updatedDept, Is.Not.Null);
            Assert.That(updatedDept.DepartmentName, Is.EqualTo("NewDept"));
        }

        [Test]
        public void UpdateAsync_GivenNullRequest_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _departmentRepository.UpdateAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentRequest"));
        }

        [Test]
        public async Task DeleteAsync_GivenValidId_ShouldRemoveDepartment()
        {
            // Arrange
            var dept = new Department { Id = 1, DepartmentName = "ToRemove" };
            _dbContext.Departments.Add(dept);
            _dbContext.SaveChanges();

            // Act
            await _departmentRepository.DeleteAsync(1);

            // Assert
            var deletedDept = _dbContext.Departments.FirstOrDefault(d => d.Id == 1);
            Assert.That(deletedDept, Is.Null);
        }
    }
}