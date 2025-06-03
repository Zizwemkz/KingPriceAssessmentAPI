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
    public class EmployeeControllerTests : IDisposable
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private EmployeeController _controller;

        [SetUp]
        public void SetUp()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnOkWithEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new EmployeeTestBuilder().BuildEmployee(),
                new EmployeeTestBuilder().BuildEmployee()
            };
            _employeeServiceMock.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employees));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenEmployeeExists_Then_ShouldReturnOk()
        {
            // Arrange
            var employee = new EmployeeTestBuilder().WithId(1).BuildEmployee();
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employee));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenEmployeeDoesNotExist_Then_ShouldReturnNotFound()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenValidModel_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new EmployeeTestBuilder().BuildAddRequest();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Added"
            };

            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(addRequest)).ReturnsAsync(response);

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
            _controller.ModelState.AddModelError("Name", "Required");

            var addRequest = new EmployeeTestBuilder().WithName(null).BuildAddRequest();

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_AddIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var addRequest = new EmployeeTestBuilder().BuildAddRequest();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenValidModelAndEmployeeExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new EmployeeTestBuilder().WithId(1).BuildUpdateRequest();
            var employee = new EmployeeTestBuilder().WithId(1).BuildEmployee();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Updated"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(updateRequest)).ReturnsAsync(response);

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
            var updateRequest = new EmployeeTestBuilder().WithId(2).BuildUpdateRequest();

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenEmployeeDoesNotExist_Then_ShouldReturnNotFound()
        {
            // Arrange
            var updateRequest = new EmployeeTestBuilder().WithId(1).BuildUpdateRequest();
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var updateRequest = new EmployeeTestBuilder().WithId(1).BuildUpdateRequest();
            var employee = new EmployeeTestBuilder().WithId(1).BuildEmployee();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenEmployeeExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var employee = new EmployeeTestBuilder().WithId(1).BuildEmployee();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Deleted"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenEmployeeDoesNotExist_Then_ShouldReturnNotFound()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenServiceReturnsFailure_Then_ShouldReturnBadRequest()
        {
            // Arrange
            var employee = new EmployeeTestBuilder().WithId(1).BuildEmployee();
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}