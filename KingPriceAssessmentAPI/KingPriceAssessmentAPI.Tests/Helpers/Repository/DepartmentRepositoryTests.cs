using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessmentAPI.Tests.Helpers.Repositories
{
    public class DepartmentRepositoryTestBuilder
    {
        private int _id = 1;
        private string _departmentName = "TestDept";

        public DepartmentRepositoryTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public DepartmentRepositoryTestBuilder WithDepartmentName(string name)
        {
            _departmentName = name;
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

        public List<Department> BuildDepartmentList(int count)
        {
            var list = new List<Department>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Department
                {
                    Id = i,
                    DepartmentName = $"{_departmentName}{i}"
                });
            }
            return list;
        }

        public AddDepartmentRequest BuildAddRequest()
        {
            return new AddDepartmentRequest
            {
                DepartmentName = _departmentName
            };
        }

        public UpdateDepartmentRequest BuildUpdateRequest()
        {
            return new UpdateDepartmentRequest
            {
                Id = _id,
                DepartmentName = _departmentName
            };
        }
    }
}