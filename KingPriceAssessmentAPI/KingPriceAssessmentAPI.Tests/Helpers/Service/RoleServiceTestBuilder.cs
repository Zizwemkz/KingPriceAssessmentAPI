using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using System.Collections.Generic;

namespace KingPriceAssessmentAPI.Tests.Helpers.Service
{
    public class RoleServiceTestBuilder
    {
        private int _id;
        private string _roleName;
        private Faker _faker = new Faker();

        public RoleServiceTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _roleName = _faker.Name.JobTitle();
        }

        public RoleServiceTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public RoleServiceTestBuilder WithRoleName(string roleName)
        {
            _roleName = roleName;
            return this;
        }

        public Role BuildRole()
        {
            return new Role
            {
                Id = _id,
                RoleName = _roleName
            };
        }

        public AddRoleRequest BuildAddRoleRequest()
        {
            return new AddRoleRequest
            {
                RoleName = _roleName
            };
        }

        public UpdateRoleRequest BuildUpdateRoleRequest()
        {
            return new UpdateRoleRequest
            {
                Id = _id,
                RoleName = _roleName
            };
        }

        public IEnumerable<Role> BuildRolesList(int count = 2)
        {
            var roles = new List<Role>();
            for (int i = 0; i < count; i++)
            {
                roles.Add(new RoleServiceTestBuilder().WithId(_id + i).WithRoleName($"{_roleName}{i}").BuildRole());
            }
            return roles;
        }
    }
}