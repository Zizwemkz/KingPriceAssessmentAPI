using KingPriceAssessment.Data;
using Microsoft.AspNetCore.Mvc;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Service;

namespace KingPriceAssessmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService service)
        {
            _roleService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRoleRequest roleModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _roleService.AddRoleAsync(roleModel);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleRequest updateRoleModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != updateRoleModel.Id) return BadRequest("ID mismatch");

            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();


            var response = await _roleService.UpdateRoleAsync(updateRoleModel);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();

            var response = await _roleService.DeleteRoleAsync(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Get roles for a department by department name
        /// </summary>
        [HttpGet("rolesbydepartment/{departmentName}")]
        public async Task<IActionResult> GetRolesByDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return BadRequest("Department name is required.");

            var roles = await _roleService.GetRolesForDepartmentAsync(departmentName);
            return Ok(roles);
        }
    }
}
