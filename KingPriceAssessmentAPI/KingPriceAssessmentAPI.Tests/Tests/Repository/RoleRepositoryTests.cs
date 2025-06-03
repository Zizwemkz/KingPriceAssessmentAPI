using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Repositories;
using KingPriceAssessmentAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace KingPriceAssessmentAPI.Tests.Repositories
{
    [TestFixture]
    public class RoleRepositoryTests
    {
        private EmployeeDbContext _dbContext;
        private RoleRepository _roleRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()) // <-- should now work
               .Options;

            _dbContext = new EmployeeDbContext(options);
            _roleRepository = new RoleRepository(_dbContext);
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnAllRoles()
        {
            // Arrange
            var builder = new RoleRepositoryTestBuilder();
            var role1 = builder.WithId(1).WithRoleName("Manager").BuildRole();
            var role2 = builder.WithId(2).WithRoleName("Developer").BuildRole();
            _dbContext.Roles.AddRange(role1, role2);
            _dbContext.SaveChanges();

            // Act
            var result = await _roleRepository.GetAllAsync();

            // Assert
            Assert.That(result, Is.EquivalentTo(new[] { role1, role2 }));
        }

        [Test]
        public async Task When_AddAsyncIsCalled_GivenValidRequest_Then_ShouldAddRole()
        {
            // Arrange
            var builder = new RoleRepositoryTestBuilder();
            var addRequest = builder.WithRoleName("Tester").BuildAddRoleRequest();

            // Act
            await _roleRepository.AddAsync(addRequest);

            // Assert
            var dbRole = _dbContext.Roles.FirstOrDefault(r => r.RoleName == "Tester");
            Assert.That(dbRole, Is.Not.Null);
            Assert.That(dbRole.RoleName, Is.EqualTo("Tester"));
        }

        [Test]
        public void When_AddAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleRepository.AddAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("roleRequest"));
        }

        [Test]
        public async Task When_UpdateAsyncIsCalled_GivenValidRequest_Then_ShouldUpdateRole()
        {
            // Arrange
            var builder = new RoleRepositoryTestBuilder();
            var role = builder.WithId(1).WithRoleName("OldName").BuildRole();
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();

            var updateRequest = builder.WithId(1).WithRoleName("UpdatedName").BuildUpdateRoleRequest();

            // Act
            await _roleRepository.UpdateAsync(updateRequest);

            // Assert
            var updatedRole = _dbContext.Roles.FirstOrDefault(r => r.Id == 1);
            Assert.That(updatedRole, Is.Not.Null);
            Assert.That(updatedRole.RoleName, Is.EqualTo("UpdatedName"));
        }

        [Test]
        public void When_UpdateAsyncIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleRepository.UpdateAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("roleRequest"));
        }

        [Test]
        public async Task When_DeleteAsyncIsCalled_GivenValidId_Then_ShouldRemoveRole()
        {
            // Arrange
            var builder = new RoleRepositoryTestBuilder();
            var role = builder.WithId(1).WithRoleName("ToDelete").BuildRole();
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();

            // Act
            await _roleRepository.DeleteAsync(1);

            // Assert
            var deletedRole = _dbContext.Roles.FirstOrDefault(r => r.Id == 1);
            Assert.That(deletedRole, Is.Null);
        }

        [Test]
        public async Task When_GetRolesForDepartmentIsCalled_GivenValidDepartmentName_Then_ShouldReturnDepartmentRolesDto()
        {
            // Arrange
            var departmentName = "Finance";
            var department = new Department { Id = 1, DepartmentName = departmentName };
            var role = new RoleRepositoryTestBuilder().WithId(1).WithRoleName("Manager").BuildRole();
            var allocation = new EmployeeAllocation { Id = 1, DepartmentId = department.Id, RoleId = role.Id, EmployeeId = 1 };

            _dbContext.Departments.Add(department);
            _dbContext.Roles.Add(role);
            _dbContext.EmployeeAllocation.Add(allocation);
            _dbContext.SaveChanges();

            // Act
            var result = await _roleRepository.GetRolesForDepartmentAsync(departmentName);

            // Assert
            var dto = result.FirstOrDefault();
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.DepartmentName, Is.EqualTo(departmentName));
            Assert.That(dto.Roles, Contains.Item("Manager"));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}