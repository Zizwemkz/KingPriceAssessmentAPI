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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService service)
        {
            _employeeService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEmployeeRequest addEmployeeRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _employeeService.AddEmployeeAsync(addEmployeeRequest);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest updateEmployeeRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != updateEmployeeRequest.Id) return BadRequest("ID mismatch");

            var employeeRecord = await _employeeService.GetEmployeeByIdAsync(id);
            if (employeeRecord == null) return NotFound();

            var response = await _employeeService.UpdateEmployeeAsync(updateEmployeeRequest);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            var response = await _employeeService.DeleteEmployeeAsync(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
