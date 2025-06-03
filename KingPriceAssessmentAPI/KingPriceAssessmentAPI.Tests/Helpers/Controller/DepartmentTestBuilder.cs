using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;

namespace KingPriceAssessmentAPI.Tests.Helpers.Controller
{
    public class DepartmentTestBuilder
    {
        private int _id;
        private string _departmentName;
        private Faker _faker = new Faker();

        public DepartmentTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _departmentName = _faker.Commerce.Department();
        }

        public DepartmentTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public DepartmentTestBuilder WithDepartmentName(string departmentName)
        {
            _departmentName = departmentName;
            return this;
        }

        public Department BuildDepartment()
        {
            return new Department
            {
                Id = _id,
                DepartmentName = _departmentName
            };
        }

        public AddDepartmentRequest BuildAddDepartmentRequest()
        {
            return new AddDepartmentRequest
            {
                DepartmentName = _departmentName
            };
        }

        public UpdateDepartmentRequest BuildUpdateDepartmentRequest()
        {
            return new UpdateDepartmentRequest
            {
                Id = _id,
                DepartmentName = _departmentName
            };
        }
    }
}