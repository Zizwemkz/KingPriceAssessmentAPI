using KingPriceAssessment.Common.Interfaces.Service;
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
    public class EmployeeAllocationControllerTests : IDisposable
    {
        private Mock<IEmployeeAllocationService> _employeeAllocationServiceMock;
        private EmployeeAllocationController _controller;

        [SetUp]
        public void SetUp()
        {
            _employeeAllocationServiceMock = new Mock<IEmployeeAllocationService>();
            _controller = new EmployeeAllocationController(_employeeAllocationServiceMock.Object);
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnOkWithAllocations()
        {
            // Arrange
            var allocations = new List<EmployeeAllocation>
            {
                new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation(),
                new EmployeeAllocationTestBuilder().WithId(2).WithEmployeeId(3).WithDepartmentId(4).WithRoleId(5).BuildEmployeeAllocation()
            };
            _employeeAllocationServiceMock.Setup(s => s.GetAllAllocationsAsync()).ReturnsAsync(allocations);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(allocations));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenAllocationExists_Then_ShouldReturnOk()
        {
            // Arrange
            var allocation = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation();
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(allocation));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenAllocationNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenValidModel_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new EmployeeAllocationTestBuilder().WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildAddRequest();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Added"
            };

            _employeeAllocationServiceMock.Setup(s => s.AddAllocationAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_AddIsCalled_GivenInvalidModel_Then_ShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("EmployeeId", "Required");
            var addRequest = new EmployeeAllocationTestBuilder().WithEmployeeId(0).BuildAddRequest();

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var addRequest = new EmployeeAllocationTestBuilder().WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildAddRequest();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.AddAllocationAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenValidModelAndAllocationExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildUpdateRequest();
            var allocation = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Updated"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.UpdateAllocationAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenIdMismatch_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var updateRequest = new EmployeeAllocationTestBuilder().WithId(2).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildUpdateRequest();

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenAllocationNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            var updateRequest = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildUpdateRequest();
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var updateRequest = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildUpdateRequest();
            var allocation = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.UpdateAllocationAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenAllocationExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var allocation = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Deleted"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.DeleteAllocationAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenAllocationNotFound_Then_ShouldReturnNotFound()
        {
            // Arrange
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var allocation = new EmployeeAllocationTestBuilder().WithId(1).WithEmployeeId(2).WithDepartmentId(3).WithRoleId(4).BuildEmployeeAllocation();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.DeleteAllocationAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_GetEmployeesByDepartmentIsCalled_GivenValidDepartment_Then_ShouldReturnOk()
        {
            // Arrange
            string departmentName = "HR";
            var employees = new List<EmployeeDetailsDto>
            {
                new EmployeeDetailsDto { EmployeeNumber = "1", Name = "John", Lastname = "Doe", RoleName = "Manager", DepartmentName = "HR" },
                new EmployeeDetailsDto { EmployeeNumber = "2", Name = "Jane", Lastname = "Smith", RoleName = "Consultant", DepartmentName = "IT" }
            };

            _employeeAllocationServiceMock.Setup(s => s.GetEmployeesByDepartmentNameAsync(departmentName)).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployeesByDepartment(departmentName) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employees));
        }

        [Test]
        public async Task When_GetEmployeesByDepartmentIsCalled_GivenDepartmentNameMissing_Then_ShouldReturnBadRequest()
        {
            // Act
            var result = await _controller.GetEmployeesByDepartment(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}