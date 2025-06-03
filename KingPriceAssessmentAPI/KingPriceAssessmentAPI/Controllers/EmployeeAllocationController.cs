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
    public class EmployeeAllocationController : Controller
    {
        private readonly IEmployeeAllocationService _employeeAllocationService;

        public EmployeeAllocationController(IEmployeeAllocationService service)
        {
            _employeeAllocationService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allocations = await _employeeAllocationService.GetAllAllocationsAsync();
            return Ok(allocations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var allocation = await _employeeAllocationService.GetAllocationByIdAsync(id);
            if (allocation == null)
                return NotFound();
            return Ok(allocation);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEmployeeAllocationRequest employeeAllocationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _employeeAllocationService.AddAllocationAsync(employeeAllocationRequest);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeAllocationRequest employeeAllocationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != employeeAllocationRequest.Id) return BadRequest("ID mismatch");

            var allocation = await _employeeAllocationService.GetAllocationByIdAsync(id);
            if (allocation == null) return NotFound();

            var response = await _employeeAllocationService.UpdateAllocationAsync(employeeAllocationRequest);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Delete employees by department id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var allocation = await _employeeAllocationService.GetAllocationByIdAsync(id);
            if (allocation == null) return NotFound();

            var response = await _employeeAllocationService.DeleteAllocationAsync(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Get employees in a department by department name (with their roles)
        /// </summary>
        [HttpGet("employeesbydepartment/{departmentName}")]
        public async Task<IActionResult> GetEmployeesByDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return BadRequest("Department name is required.");

            var employees = await _employeeAllocationService.GetEmployeesByDepartmentNameAsync(departmentName);
            return Ok(employees);
        }
    }
}
