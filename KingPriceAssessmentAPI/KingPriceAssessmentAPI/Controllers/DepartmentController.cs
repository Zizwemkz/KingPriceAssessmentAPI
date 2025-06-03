using KingPriceAssessment.Data;
using Microsoft.AspNetCore.Mvc;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessment.Service;
using Azure;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Common.Interfaces.Service;

namespace KingPriceAssessmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService service)
        {
            _departmentService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddDepartmentRequest departmentRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _departmentService.AddDepartmentAsync(departmentRequest);
            
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentRequest departmentRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != departmentRequest.Id) return BadRequest("ID mismatch");

            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();

            var response = await _departmentService.UpdateDepartmentAsync(departmentRequest);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();

            var response = await _departmentService.DeleteDepartmentAsync(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
