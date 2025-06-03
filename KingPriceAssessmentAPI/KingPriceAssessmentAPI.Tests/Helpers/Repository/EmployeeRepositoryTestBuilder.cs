using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using System.Collections.Generic;

namespace KingPriceAssessmentAPI.Tests.Helpers
{
    public class EmployeeRepositoryTestBuilder
    {
        private int _id;
        private string _employeeNumber;
        private string _name;
        private string _lastname;
        private int _age;
        private string _position;
        private Faker _faker = new Faker();

        public EmployeeRepositoryTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _employeeNumber = _faker.Random.Replace("EMP####");
            _name = _faker.Name.FirstName();
            _lastname = _faker.Name.LastName();
            _age = _faker.Random.Int(20, 60);
            _position = _faker.Name.JobTitle();
        }

        public EmployeeRepositoryTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public EmployeeRepositoryTestBuilder WithEmployeeNumber(string employeeNumber)
        {
            _employeeNumber = employeeNumber;
            return this;
        }

        public EmployeeRepositoryTestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public EmployeeRepositoryTestBuilder WithLastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }

        public EmployeeRepositoryTestBuilder WithAge(int age)
        {
            _age = age;
            return this;
        }

        public EmployeeRepositoryTestBuilder WithPosition(string position)
        {
            _position = position;
            return this;
        }

        public Employee BuildEmployee()
        {
            return new Employee
            {
                Id = _id,
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname,
                Age = _age,
                Position = _position
            };
        }

        public AddEmployeeRequest BuildAddEmployeeRequest()
        {
            return new AddEmployeeRequest
            {
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname,
                Age = _age,
                Position = _position
            };
        }

        public UpdateEmployeeRequest BuildUpdateEmployeeRequest()
        {
            return new UpdateEmployeeRequest
            {
                Id = _id,
                EmployeeNumber = _employeeNumber,
                Name = _name,
                Lastname = _lastname,
                Age = _age,
                Position = _position
            };
        }

        public IEnumerable<Employee> BuildEmployeeList(int count = 2)
        {
            var list = new List<Employee>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new EmployeeRepositoryTestBuilder().WithId(_id + i).WithEmployeeNumber($"{_employeeNumber}{i}").BuildEmployee());
            }
            return list;
        }
    }
}