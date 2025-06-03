using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;

namespace KingPriceAssessmentAPI.Tests.Helpers.Controller
{
    public class EmployeeAllocationTestBuilder
    {
        private int _id;
        private int _employeeId;
        private int _departmentId;
        private int _roleId;
        private Faker _faker = new Faker();

        public EmployeeAllocationTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _employeeId = _faker.Random.Int(1, 1000);
            _departmentId = _faker.Random.Int(1, 100);
            _roleId = _faker.Random.Int(1, 50);
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

        public EmployeeAllocation BuildEmployeeAllocation()
        {
            return new EmployeeAllocation
            {
                Id = _id,
                EmployeeId = _employeeId,
                DepartmentId = _departmentId,
                RoleId = _roleId
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