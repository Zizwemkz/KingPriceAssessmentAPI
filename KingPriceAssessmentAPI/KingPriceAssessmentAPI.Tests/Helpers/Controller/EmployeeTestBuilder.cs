using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;

namespace KingPriceAssessmentAPI.Tests.Helpers.Controller
{
    public class EmployeeTestBuilder
    {
        private int _id;
        private string _name;
        private string _lastname;
        private Faker _faker = new Faker();

        public EmployeeTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _name = _faker.Name.FirstName();
            _lastname = _faker.Name.LastName();
        }

        public EmployeeTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public EmployeeTestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public EmployeeTestBuilder WithLastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }

        public Employee BuildEmployee()
        {
            return new Employee
            {
                Id = _id,
                Name = _name,
                Lastname = _lastname
            };
        }

        public AddEmployeeRequest BuildAddRequest()
        {
            return new AddEmployeeRequest
            {
                Name = _name,
                Lastname = _lastname
            };
        }

        public UpdateEmployeeRequest BuildUpdateRequest()
        {
            return new UpdateEmployeeRequest
            {
                Id = _id,
                Name = _name,
                Lastname = _lastname
            };
        }
    }
}