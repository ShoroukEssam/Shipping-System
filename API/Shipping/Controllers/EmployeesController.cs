using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.Constants;
using Shipping.DTO.Employee_DTOs;
using Shipping.Models;
using Shipping.UnitOfWork;
using Swashbuckle.AspNetCore.Annotations;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork<Employee> _unit;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public EmployeesController(IUnitOfWork<Employee> unit, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region Get List Of Employees
        [HttpGet]
        [Permission(Permissions.Employees.View)]
        [SwaggerOperation(Summary = "Retrieves a paginated list of employees.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of employees.", typeof(List<EmpDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No employees found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<List<EmpDTO>>> GetEmployees(int page = 1, int pageSize = 9)
        {
            try
            {
                var employees = await _unit.EmployeeRepository.GetAllEmployees();
                if (!employees.Any())
                    return NotFound(new { message = "لا يوجد موظفين" });

                int totalCount = employees.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                employees = employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var employeesList = _mapper.Map<List<EmpDTO>>(employees);

                foreach (var empDto in employeesList)
                {
                    var employee = employees.FirstOrDefault(e => e.UserId == empDto.id);
                    var roles = await _userManager.GetRolesAsync(employee.User);
                    empDto.role = roles.FirstOrDefault();
                }

                return Ok(new { totalCount, totalPages, employeesList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"حطأ في استرجاع بيانات الموظفين : {ex.Message}");
            }
        }

        #endregion

        #region Get Specific Employee
        [HttpGet("{id}")]
        [Permission(Permissions.Employees.View)]
        [SwaggerOperation(Summary = "Retrieves a specific employee by unique id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the employee details.", typeof(EmpDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Employee not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<EmpDTO>> GetEmployee(string id)
        {
            try
            {
                var employee = await _unit.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new { message = $" لا يوجد موظف هذا الرقم" });

                var employeeDto = _mapper.Map<EmpDTO>(employee);
                var roles = await _userManager.GetRolesAsync(employee.User);
                employeeDto.role = roles.FirstOrDefault();

                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في استرجاع بيانات الموظف : {ex.Message}");
            }
        }
        #endregion

        #region Search Employees
        [HttpGet("search")]
        [Permission(Permissions.Employees.View)]
        [SwaggerOperation(Summary = "Searches employees based on a query.", Description = "Searches for employees that match the given query.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EmpDTO>>> SearchEmployeesAsync(string query)
        {
            try
            {
                var employees = _unit.EmployeeRepository.Search(query);
                if (employees == null || !employees.Any())
                    return NotFound(new { message = "لا يوجد موظفين يتوافقوا مع بحثك" });

                var employeesList = _mapper.Map<List<EmpDTO>>(employees);

                foreach (var empDto in employeesList)
                {
                    var employee = employees.FirstOrDefault(e => e.UserId == empDto.id);
                    var roles = await _userManager.GetRolesAsync(employee.User);
                    empDto.role = roles.FirstOrDefault();
                }

                return Ok(employeesList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في البحث عن الموظفين: {ex.Message}");
            }
        }
        #endregion

        #region Update Employee
        [HttpPut("{id}")]
        [Permission(Permissions.Employees.Edit)]
        [SwaggerOperation(Summary = "Updates the data of an employee.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Employee updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Employee ID mismatch.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Employee not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> PutEmployee(string id, EmpDTO employeeDto)
        {
            try
            {
                if (id != employeeDto.id)
                    return BadRequest(new { message = "الرقم الخاص بالموظف غير متطابق" });

                var updated = await _unit.EmployeeRepository.Update(employeeDto, _userManager);
                _unit.SaveChanges();

                var UpdateData = _mapper.Map<EmpDTO>(updated);
                var rolesAfterUpdated = await _userManager.GetRolesAsync(updated.User);
                UpdateData.role = rolesAfterUpdated.FirstOrDefault();

                return Ok(new {Employee = UpdateData, Msg = "تم تعديل البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في تعديل بيانات الموظف: {ex.Message}");
            }
        }
        #endregion

        #region Add Employee
        [HttpPost]
        [Permission(Permissions.Employees.Create)]
        [SwaggerOperation(Summary = "Creates a new employee.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Employee created successfully.", typeof(EmpDTO))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<Employee>> PostEmployee(EmpDTO employee)
        {
            try
            {
                Employee NewEmployee = await _unit.EmployeeRepository.Add(employee,_userManager);
                await _unit.Repository.SaveAsync();

                var employeeDto = _mapper.Map<EmpDTO>(NewEmployee);
                var roles = await _userManager.GetRolesAsync(NewEmployee.User);
                employeeDto.role = roles.FirstOrDefault();

                return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.id },employeeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في اضافة موظف جديد : {ex.Message}");
            }
        }
        #endregion

        #region Update Status
        [HttpPut("status/{id}")]
        [Permission(Permissions.Employees.Edit)]
        [SwaggerOperation(Summary = "Updates the status of an existing employee.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Employee status updated successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Employee not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> UpdateEmployeeStatus(string id, bool status)
        {
            try
            {
                var employee = await _unit.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new { message = $"لا يوجد موظف يحمل هذا الرقم" });

                var updated = await _unit.EmployeeRepository.UpdateStatus(employee, status);
                _unit.SaveChanges();

                var employeeDto = _mapper.Map<EmpDTO>(updated);
                var roles = await _userManager.GetRolesAsync(updated.User);
                employeeDto.role = roles.FirstOrDefault();

                return Ok(new {Employee = employeeDto, Msg = "تم تعديل الحالة بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في تعديل حالة الموظف : {ex.Message}");
            }
        }
        #endregion

        #region SoftDelete
        [HttpDelete("{id}")]
        [Permission(Permissions.Employees.Delete)]
        [SwaggerOperation(Summary = "Soft delete for an existing employee.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Employee deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Employee not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                var employee = await _unit.EmployeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new { message = $"لا يوجد موظف يحمل هذا الرقم" });

                await _unit.EmployeeRepository.SoftDeleteAsync(employee);
                _unit.SaveChanges();

                return Ok(new { Status = 201, Msg = "تم حذف الموظف بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطأ في حذف الموظف : {ex.Message}");
            }
        }
        #endregion
    }
}
