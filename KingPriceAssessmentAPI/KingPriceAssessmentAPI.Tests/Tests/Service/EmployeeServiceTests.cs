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
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private EmployeeService _employeeService;

        [SetUp]
        public void SetUp()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Test]
        public async Task When_GetAllEmployeesIsCalled_Then_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new EmployeeServiceTestBuilder().BuildEmployeeList(2);
            _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.That(result, Is.EqualTo(employees));
        }

        [Test]
        public async Task When_GetEmployeeByIdIsCalled_GivenValidId_Then_ShouldReturnEmployee()
        {
            // Arrange
            var employee = new EmployeeServiceTestBuilder().WithId(1).BuildEmployee();
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(employee));
        }

        [Test]
        public void When_GetEmployeeByIdIsCalled_GivenIdIsInvalid_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _employeeService.GetEmployeeByIdAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void When_GetEmployeeByIdIsCalled_GivenEmployeeNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _employeeService.GetEmployeeByIdAsync(1));
            Assert.That(ex.Message, Does.Contain("No Employee found with ID 1"));
        }

        [Test]
        public async Task When_AddEmployeeIsCalled_GivenUniqueEmployeeNumber_Then_ShouldAddAndReturnSuccess()
        {
            // Arrange
            var addRequest = new EmployeeServiceTestBuilder().WithEmployeeNumber("EMP1001").BuildAddEmployeeRequest();
            _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());
            _employeeRepositoryMock.Setup(r => r.AddAsync(addRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.AddEmployeeAsync(addRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Employee added successfully."));
        }

        [Test]
        public void When_AddEmployeeIsCalled_GivenEmployeeNumberAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var addRequest = new EmployeeServiceTestBuilder().WithEmployeeNumber("EMP1001").BuildAddEmployeeRequest();
            var employees = new List<Employee> { new EmployeeServiceTestBuilder().WithEmployeeNumber("EMP1001").BuildEmployee() };
            _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _employeeService.AddEmployeeAsync(addRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_UpdateEmployeeIsCalled_GivenValidModelAndEmployeeExists_Then_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var updateRequest = new EmployeeServiceTestBuilder().WithId(1).WithEmployeeNumber("EMP1001").BuildUpdateEmployeeRequest();
            var employee = new EmployeeServiceTestBuilder().WithId(1).WithEmployeeNumber("EMP1002").BuildEmployee();
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee> { employee });
            _employeeRepositoryMock.Setup(r => r.UpdateAsync(updateRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(updateRequest);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Employee Updated successfully."));
        }

        [Test]
        public void When_UpdateEmployeeIsCalled_GivenIdIsInvalid_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new EmployeeServiceTestBuilder().WithId(0).BuildUpdateEmployeeRequest();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _employeeService.UpdateEmployeeAsync(updateRequest));
            Assert.That(ex.ParamName, Is.EqualTo("Id"));
        }

        [Test]
        public void When_UpdateEmployeeIsCalled_GivenEmployeeNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var updateRequest = new EmployeeServiceTestBuilder().WithId(1).WithEmployeeNumber("EMP1001").BuildUpdateEmployeeRequest();
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _employeeService.UpdateEmployeeAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("No Employee found with ID 1"));
        }

        [Test]
        public void When_UpdateEmployeeIsCalled_GivenEmployeeNumberAlreadyExists_Then_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new EmployeeServiceTestBuilder().WithId(1).WithEmployeeNumber("EMP1001").BuildUpdateEmployeeRequest();
            var employees = new List<Employee>
            {
                new EmployeeServiceTestBuilder().WithId(2).WithEmployeeNumber("EMP1001").BuildEmployee(),
                new EmployeeServiceTestBuilder().WithId(1).WithEmployeeNumber("EMP2002").BuildEmployee()
            };
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employees[1]);
            _employeeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _employeeService.UpdateEmployeeAsync(updateRequest));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public async Task When_DeleteEmployeeIsCalled_GivenValidId_Then_ShouldDeleteAndReturnSuccess()
        {
            // Arrange
            var employee = new EmployeeServiceTestBuilder().WithId(1).BuildEmployee();
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Employee added successfully."));
        }

        [Test]
        public void When_DeleteEmployeeIsCalled_GivenIdIsInvalid_Then_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _employeeService.DeleteEmployeeAsync(0));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void When_DeleteEmployeeIsCalled_GivenEmployeeNotFound_Then_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _employeeService.DeleteEmployeeAsync(1));
            Assert.That(ex.Message, Does.Contain("No Employee found with ID 1"));
        }
    }
}