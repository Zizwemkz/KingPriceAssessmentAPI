using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using System.Collections.Generic;

namespace KingPriceAssessmentAPI.Tests.Helpers.Service
{
    public class EmployeeServiceTestBuilder
    {
        private int _id;
        private string _employeeNumber;
        private string _name;
        private string _lastname;
        private Faker _faker = new Faker();

        public EmployeeServiceTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _employeeNumber = _faker.Random.Replace("EMP####");
            _name = _faker.Name.FirstName();
            _lastname = _faker.Name.LastName();
        }

        public EmployeeServiceTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public EmployeeServiceTestBuilder WithEmployeeNumber(string employeeNumber)
        {
            _employeeNumber = employeeNumber;
            return this;
        }

        public EmployeeServiceTestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public EmployeeServiceTestBuilder WithLastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }

        public Employee BuildEmployee()
        {
            return new Employee
            {
                Id = _id,
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname
            };
        }

        public AddEmployeeRequest BuildAddEmployeeRequest()
        {
            return new AddEmployeeRequest
            {
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname
            };
        }

        public UpdateEmployeeRequest BuildUpdateEmployeeRequest()
        {
            return new UpdateEmployeeRequest
            {
                Id = _id,
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname
            };
        }

        public IEnumerable<Employee> BuildEmployeeList(int count = 2)
        {
            var list = new List<Employee>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new EmployeeServiceTestBuilder().WithId(_id + i).WithEmployeeNumber($"{_employeeNumber}{i}").BuildEmployee());
            }
            return list;
        }
    }
}