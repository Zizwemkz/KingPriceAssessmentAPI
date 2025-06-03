using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using System.Collections.Generic;

namespace KingPriceAssessmentAPI.Tests.Helpers
{
    public class DepartmentServiceTestBuilder
    {
        private int _id;
        private string _departmentName;
        private Faker _faker = new Faker();

        public DepartmentServiceTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _departmentName = _faker.Commerce.Department();
        }

        public DepartmentServiceTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public DepartmentServiceTestBuilder WithDepartmentName(string departmentName)
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

        public IEnumerable<Department> BuildDepartmentList(int count = 2)
        {
            var list = new List<Department>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new DepartmentServiceTestBuilder().WithId(_id + i).WithDepartmentName($"{_departmentName}{i}").BuildDepartment());
            }
            return list;
        }
    }
}