using Bogus;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;

namespace KingPriceAssessmentAPI.Tests.Helpers.Controller
{
    public class RoleTestBuilder
    {
        private int _id;
        private string _roleName;
        private Faker _faker = new Faker();

        public RoleTestBuilder()
        {
            _id = _faker.Random.Int(1, 1000);
            _roleName = _faker.Random.Word();
        }

        public RoleTestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public RoleTestBuilder WithRoleName(string roleName)
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
    }
}