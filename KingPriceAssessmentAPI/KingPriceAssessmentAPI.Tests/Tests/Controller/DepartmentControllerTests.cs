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
    public class DepartmentControllerTests : IDisposable
    {
        private Mock<IDepartmentService> _departmentServiceMock;
        private DepartmentController _controller;

        [SetUp]
        public void SetUp()
        {
            _departmentServiceMock = new Mock<IDepartmentService>();
            _controller = new DepartmentController(_departmentServiceMock.Object);
        }

        [Test]
        public async Task When_GetAllIsCalled_Then_ShouldReturnOkWithDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new DepartmentTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment(),
                new DepartmentTestBuilder().WithId(2).WithDepartmentName("IT").BuildDepartment()
            };
            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync()).ReturnsAsync(departments);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(departments));
        }

        [Test]
        public async Task When_GetByIdIsCalled_GivenDepartmentExists_Then_ShouldReturnOk()
        {
            // Arrange
            var department = new DepartmentTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(department));
        }

        [Test]
        public async Task When_AddIsCalled_GivenValidModel_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new DepartmentTestBuilder().WithDepartmentName("Finance").BuildAddDepartmentRequest();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.AddDepartmentAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_UpdateIsCalled_GivenValidModelAndDepartmentExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new DepartmentTestBuilder().WithId(1).WithDepartmentName("Updated").BuildUpdateDepartmentRequest();
            var department = new DepartmentTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);
            _departmentServiceMock.Setup(s => s.UpdateDepartmentAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task When_DeleteIsCalled_GivenDepartmentExists_Then_ShouldReturnOkWithSuccessResponse()
        {
            // Arrange
            var department = new DepartmentTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);
            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}