using Microsoft.AspNetCore.Identity;
using Shipping.DTO.Employee_DTOs;
using Shipping.Models;

namespace Shipping.Repository.Employee_Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> Add(EmpDTO newEmp, UserManager<AppUser> _userManager);
        Task<Employee> GetEmployeeByIdAsync(string id);
        List<Employee> Search(string query);
        Task<Employee> Update(EmpDTO NewData, UserManager<AppUser> _userManager);
        Task<Employee> UpdateStatus(Employee employee, bool status);
        Task SoftDeleteAsync(Employee employee);
    }
}
