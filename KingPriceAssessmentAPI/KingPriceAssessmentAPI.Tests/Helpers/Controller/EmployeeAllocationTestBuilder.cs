using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;

namespace KingPriceAssessmentAPI.Tests.Helpers.Controller
{
    public class EmployeeAllocationTestBuilder
    {
        private int _id;
        private int _employeeId;
        private int _departmentId;
        private int _roleId;
        private string _employeeName;
        private string _departmentName;
        private string _roleName;
        private Faker _faker = new Faker();

        public EmployeeAllocationTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _employeeId = _faker.Random.Int(1, 1000);
            _departmentId = _faker.Random.Int(1, 100);
            _roleId = _faker.Random.Int(1, 50);
            _employeeName = _faker.Name.FullName();
            _roleName = _faker.Name.JobTitle();
            _departmentName = _faker.Commerce.Department();
        }

        public EmployeeAllocationTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public EmployeeAllocationTestBuilder WithEmployeeId(int employeeId)
        {
            _employeeId = employeeId;
            return this;
        }

        public EmployeeAllocationTestBuilder WithDepartmentId(int departmentId)
        {
            _departmentId = departmentId;
            return this;
        }

        public EmployeeAllocationTestBuilder WithRoleId(int roleId)
        {
            _roleId = roleId;
            return this;
        }

        public EmployeeAllocationDto BuildEmployeeAllocation()
        {
            return new EmployeeAllocationDto
            {
                Id = _id,
                EmployeeId = _employeeId,
                DepartmentId = _departmentId,
                RoleId = _roleId,
                EmployeeName = _employeeName,
                DepartmentName = _departmentName,
                RoleName = _roleName
            };
        }

        public AddEmployeeAllocationRequest BuildAddRequest()
        {
            return new AddEmployeeAllocationRequest
            {
                EmployeeId = _employeeId,
                DepartmentId = _departmentId,
                RoleId = _roleId
            };
        }

        public UpdateEmployeeAllocationRequest BuildUpdateRequest()
        {
            return new UpdateEmployeeAllocationRequest
            {
                Id = _id,
                EmployeeId = _employeeId,
                DepartmentId = _departmentId,
                RoleId = _roleId
            };
        }
    }
}