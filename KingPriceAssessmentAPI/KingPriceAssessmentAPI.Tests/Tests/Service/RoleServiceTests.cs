using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Service;
using KingPriceAssessment.Common.Interfaces.Repository;
using KingPriceAssessmentAPI.Tests.Helpers;
using Moq;
using NUnit.Framework;
using KingPriceAssessmentAPI.Tests.Helpers.Service;

namespace KingPriceAssessmentAPI.Tests.Service
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<IRoleRepository> _roleRepositoryMock;
        private RoleService _roleService;

        [SetUp]
        public void SetUp()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleService = new RoleService(_roleRepositoryMock.Object);
        }

        [Test]
        public async Task When_GetAllRolesIsCalled_Then_ShouldReturnAllRoles()
        {
            // Arrange
            var roles = new RoleServiceTestBuilder().BuildRolesList(2);
            _roleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            Assert.That(result, Is.EqualTo(roles));
        }

        [Test]
        public async Task When_GetRoleByIdIsCalled_GivenValidId_Then_ShouldReturnRole()
        {
            // Arrange
            var role = new RoleServiceTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(role);

            // Act
            var result = await _roleService.GetRoleByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(role));
        }

        [Test]
        public void When_GetRoleByIdIsCalled_GivenIdIsNegative_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.GetRoleByIdAsync(-1));
            Assert.That(ex.Message, Does.Contain("Role ID cannot be 0 or negative"));
        }

        [Test]
        public void When_GetRoleByIdIsCalled_GivenRoleNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Role)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _roleService.GetRoleByIdAsync(1));
            Assert.That(ex.Message, Does.Contain("No Role found with ID 1"));
        }

        [Test]
        public async Task When_AddRoleIsCalled_GivenValidRoleRequest_Then_ShouldAddAndReturnSuccess()
        {
            // Arrange
            var addRequest = new RoleServiceTestBuilder().WithRoleName("Finance").BuildAddRoleRequest();
            _roleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Role>());
            _roleRepositoryMock.Setup(r => r.AddAsync(addRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _roleService.AddRoleAsync(addRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role added successfully."));
        }

        [Test]
        public void When_AddRoleIsCalled_GivenNullRoleRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _roleService.AddRoleAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("roleRequest"));
        }

        [Test]
        public void When_AddRoleIsCalled_GivenRoleNameIsEmpty_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var addRequest = new RoleServiceTestBuilder().WithRoleName("").BuildAddRoleRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.AddRoleAsync(addRequest));
            Assert.That(ex.ParamName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void When_AddRoleIsCalled_GivenRoleNameAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var addRequest = new RoleServiceTestBuilder().WithRoleName("Admin").BuildAddRoleRequest();
            var roles = new List<Role> { new RoleServiceTestBuilder().WithRoleName("Admin").BuildRole() };
            _roleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.AddRoleAsync(addRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_UpdateRoleIsCalled_GivenValidUpdateRequest_Then_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var updateRequest = new RoleServiceTestBuilder().WithId(1).WithRoleName("Updated").BuildUpdateRoleRequest();
            var existingRole = new RoleServiceTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingRole);
            _roleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Role> { existingRole });
            _roleRepositoryMock.Setup(r => r.UpdateAsync(updateRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _roleService.UpdateRoleAsync(updateRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role uodated successfully."));
        }

        [Test]
        public void When_UpdateRoleIsCalled_GivenUpdateRequestIsNullOrIdInvalid_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new RoleServiceTestBuilder().WithId(0).WithRoleName("Any").BuildUpdateRoleRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.UpdateRoleAsync(updateRequest));
            Assert.That(ex.ParamName, Is.EqualTo("Id"));
        }

        [Test]
        public void When_UpdateRoleIsCalled_GivenRoleNameIsEmpty_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new RoleServiceTestBuilder().WithId(1).WithRoleName("").BuildUpdateRoleRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.UpdateRoleAsync(updateRequest));
            Assert.That(ex.ParamName, Is.EqualTo("RoleName"));
        }

        [Test]
        public void When_UpdateRoleIsCalled_GivenRoleDoesNotExist_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var updateRequest = new RoleServiceTestBuilder().WithId(1).WithRoleName("Updated").BuildUpdateRoleRequest();
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Role)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _roleService.UpdateRoleAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("No Role found with ID 1"));
        }

        [Test]
        public void When_UpdateRoleIsCalled_GivenRoleNameAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new RoleServiceTestBuilder().WithId(1).WithRoleName("Admin").BuildUpdateRoleRequest();
            var roles = new List<Role>
            {
                new RoleServiceTestBuilder().WithId(2).WithRoleName("Admin").BuildRole(),
                new RoleServiceTestBuilder().WithId(1).WithRoleName("Other").BuildRole()
            };
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(roles[1]);
            _roleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.UpdateRoleAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_DeleteRoleIsCalled_GivenValidId_Then_ShouldDeleteAndReturnSuccess()
        {
            // Arrange
            var existingRole = new RoleServiceTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingRole);
            _roleRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _roleService.DeleteRoleAsync(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role Deleted successfully."));
        }

        [Test]
        public void When_DeleteRoleIsCalled_GivenIdInvalid_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.DeleteRoleAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void When_DeleteRoleIsCalled_GivenRoleDoesNotExist_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _roleRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Role)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _roleService.DeleteRoleAsync(1));
            Assert.That(ex.Message, Does.Contain("No Role found with ID 1"));
        }

        [Test]
        public async Task When_GetRolesForDepartmentIsCalled_GivenValidDepartmentName_Then_ShouldReturnDepartmentRoles()
        {
            // Arrange
            var departmentName = "Finance";
            var departmentRoles = new List<DepartmentRolesDto>
            {
                new DepartmentRolesDto { DepartmentName = departmentName, Roles = new List<string> { "Analyst", "Manager" } }
            };
            _roleRepositoryMock.Setup(r => r.GetRolesForDepartmentAsync(departmentName)).ReturnsAsync(departmentRoles);

            // Act
            var result = await _roleService.GetRolesForDepartmentAsync(departmentName);

            // Assert
            Assert.That(result, Is.EqualTo(departmentRoles));
        }

        [Test]
        public void When_GetRolesForDepartmentIsCalled_GivenDepartmentNameIsNullOrWhitespace_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _roleService.GetRolesForDepartmentAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentName"));
        }
    }
}