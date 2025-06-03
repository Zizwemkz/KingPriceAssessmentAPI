using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessmentAPI.Controllers;
using KingPriceAssessmentAPI.Tests.Helpers;
using KingPriceAssessmentAPI.Tests.Helpers.Controller;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace KingPriceAssessmentAPI.Tests.Controllers
{
    [TestFixture]
    public class RoleControllerTests : IDisposable
    {
        private Mock<IRoleService> _roleServiceMock;
        private RoleController _controller;

        [SetUp]
        public void SetUp()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _controller = new RoleController(_roleServiceMock.Object);
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnOkWithRoles()
        {
            // Arrange
            var response = new List<Role>
            {
                new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole(),
                new RoleTestBuilder().WithId(2).WithRoleName("User").BuildRole()
            };
            _roleServiceMock.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenRoleExists_Then_ShouldReturnOk()
        {
            // Arrange
            var role = new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(role));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenRoleNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenValidModel_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new RoleTestBuilder().WithRoleName("Finance").BuildAddRoleRequest();
            var response = new ResponseMessage { Success = true, Message = "Added" };

            _roleServiceMock.Setup(s => s.AddRoleAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_AddIsCalled_GivenInvalidModel_Then_ShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("RoleName", "Required");

            // Act
            var result = await _controller.Add(new RoleTestBuilder().WithRoleName(null).BuildAddRoleRequest());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var addRequest = new RoleTestBuilder().WithRoleName("Finance").BuildAddRoleRequest();
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.AddRoleAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenValidModelAndRoleExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new RoleTestBuilder().WithId(1).WithRoleName("Updated").BuildUpdateRoleRequest();
            var role = new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            var response = new ResponseMessage { Success = true, Message = "Updated" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenIdMismatch_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var updateRequest = new RoleTestBuilder().WithId(2).WithRoleName("Updated").BuildUpdateRoleRequest();

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenRoleNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            var updateRequest = new RoleTestBuilder().WithId(1).WithRoleName("Updated").BuildUpdateRoleRequest();
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var updateRequest = new RoleTestBuilder().WithId(1).WithRoleName("Updated").BuildUpdateRoleRequest();
            var role = new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenRoleExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var role = new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            var response = new ResponseMessage { Success = true, Message = "Deleted" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.DeleteRoleAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenRoleNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var role = new RoleTestBuilder().WithId(1).WithRoleName("Admin").BuildRole();
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.DeleteRoleAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_GetRolesByDepartmentIsCalled_GivenValidDepartment_Then_ShouldReturnOk()
        {
            // Arrange
            var departmentName = "Finance";
            var response = new List<DepartmentRolesDto>
            {
                new DepartmentRolesDto
                {
                    DepartmentName = departmentName,
                    Roles = new List<string> { "Analyst", "Manager" }
                }
            };
            _roleServiceMock.Setup(s => s.GetRolesForDepartmentAsync(departmentName)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRolesByDepartment(departmentName) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_GetRolesByDepartmentIsCalled_GivenDepartmentNameMissing_Then_ShouldReturnBadRequest()
        {
            // Act
            var result = await _controller.GetRolesByDepartment(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}