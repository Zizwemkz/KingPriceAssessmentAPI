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

namespace KingPriceAssessmentAPI.Tests.Service
{
    [TestFixture]
    public class DepartmentServiceTests
    {
        private Mock<IDepartmentRepository> _departmentRepositoryMock;
        private DepartmentService _departmentService;

        [SetUp]
        public void SetUp()
        {
            _departmentRepositoryMock = new Mock<IDepartmentRepository>();
            _departmentService = new DepartmentService(_departmentRepositoryMock.Object);
        }

        [Test]
        public async Task When_GetAllDepartmentsIsCalled_Then_ShouldReturnAllDepartments()
        {
            // Arrange
            var departments = new DepartmentServiceTestBuilder().BuildDepartmentList(2);
            _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            Assert.That(result, Is.EqualTo(departments));
        }

        [Test]
        public async Task When_GetDepartmentByIdIsCalled_GivenValidId_Then_ShouldReturnDepartment()
        {
            // Arrange
            var department = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(department));
        }

        [Test]
        public void When_GetDepartmentByIdIsCalled_GivenIdIsInvalid_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.GetDepartmentByIdAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void When_GetDepartmentByIdIsCalled_GivenDepartmentNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _departmentService.GetDepartmentByIdAsync(1));
            Assert.That(ex.Message, Does.Contain("No Department found with ID 1"));
        }

        [Test]
        public async Task When_AddDepartmentIsCalled_GivenValidRequest_Then_ShouldAddAndReturnSuccess()
        {
            // Arrange
            var addRequest = new DepartmentServiceTestBuilder().WithDepartmentName("Finance").BuildAddDepartmentRequest();
            _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Department>());
            _departmentRepositoryMock.Setup(r => r.AddAsync(addRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.AddDepartmentAsync(addRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Department Added successfully."));
        }

        [Test]
        public void When_AddDepartmentIsCalled_GivenNullRequest_Then_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _departmentService.AddDepartmentAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("departmentRequest"));
        }

        [Test]
        public void When_AddDepartmentIsCalled_GivenDepartmentNameIsEmpty_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var addRequest = new DepartmentServiceTestBuilder().WithDepartmentName("").BuildAddDepartmentRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.AddDepartmentAsync(addRequest));
            Assert.That(ex.ParamName, Is.EqualTo("DepartmentName"));
        }

        [Test]
        public void When_AddDepartmentIsCalled_GivenDepartmentNameAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var addRequest = new DepartmentServiceTestBuilder().WithDepartmentName("HR").BuildAddDepartmentRequest();
            var departments = new List<Department> { new DepartmentServiceTestBuilder().WithDepartmentName("HR").BuildDepartment() };
            _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.AddDepartmentAsync(addRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_UpdateDepartmentIsCalled_GivenValidRequestAndDepartmentExists_Then_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var updateRequest = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("Updated").BuildUpdateDepartmentRequest();
            var department = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
            _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Department> { department });
            _departmentRepositoryMock.Setup(r => r.UpdateAsync(updateRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.UpdateDepartmentAsync(updateRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Department Updated successfully."));
        }

        [Test]
        public void When_UpdateDepartmentIsCalled_GivenRequestIsNullOrIdInvalid_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new DepartmentServiceTestBuilder().WithId(0).WithDepartmentName("Any").BuildUpdateDepartmentRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.UpdateDepartmentAsync(updateRequest));
            Assert.That(ex.ParamName, Is.EqualTo("Id"));
        }

        [Test]
        public void When_UpdateDepartmentIsCalled_GivenDepartmentNameIsEmpty_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("").BuildUpdateDepartmentRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.UpdateDepartmentAsync(updateRequest));
            Assert.That(ex.ParamName, Is.EqualTo("DepartmentName"));
        }

        [Test]
        public void When_UpdateDepartmentIsCalled_GivenDepartmentNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var updateRequest = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("Updated").BuildUpdateDepartmentRequest();
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _departmentService.UpdateDepartmentAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("No Department found with ID 1"));
        }

        [Test]
        public void When_UpdateDepartmentIsCalled_GivenDepartmentNameAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("Finance").BuildUpdateDepartmentRequest();
            var departments = new List<Department>
            {
                new DepartmentServiceTestBuilder().WithId(2).WithDepartmentName("Finance").BuildDepartment(),
                new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment()
            };
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(departments[1]);
            _departmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.UpdateDepartmentAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_DeleteDepartmentIsCalled_GivenValidId_Then_ShouldDeleteAndReturnSuccess()
        {
            // Arrange
            var department = new DepartmentServiceTestBuilder().WithId(1).WithDepartmentName("HR").BuildDepartment();
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
            _departmentRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _departmentService.DeleteDepartmentAsync(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Department Deleted successfully."));
        }

        [Test]
        public void When_DeleteDepartmentIsCalled_GivenIdIsInvalid_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _departmentService.DeleteDepartmentAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void When_DeleteDepartmentIsCalled_GivenDepartmentNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _departmentRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _departmentService.DeleteDepartmentAsync(1));
            Assert.That(ex.Message, Does.Contain("No Department found with ID 1"));
        }
    }
}