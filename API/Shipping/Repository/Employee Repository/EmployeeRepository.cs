using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Shipping.DTO.Employee_DTOs;
using Shipping.Models;
using Shipping.Repository.OrderRepo;

namespace Shipping.Repository.Employee_Repository
{
    public class EmployeeRepository: Repository<Employee>, IEmployeeRepository
    {
        ShippingContext context;
        public EmployeeRepository(ShippingContext context): base(context)
        {
            this.context = context;
        }

        #region Get All Employees
        public async Task<List<Employee>> GetAllEmployees()
        {
            var employees = context.Employees.Where(e => e.User.IsDeleted != true).ToList();
            return employees;
        }
        #endregion

        #region Add Employee (empDto)
        public async Task<Employee> Add(EmpDTO newEmp,UserManager<AppUser> _userManager)
        {
            var branch = context.Branches.FirstOrDefault(b => b.Id == newEmp.branchId);
            if (branch == null) throw new Exception("الفرع غير موجود");

            var user = new AppUser
            {
                Email = newEmp.email,
                UserName = newEmp.name.Replace(" ", "") + new Random().Next(1, 999),
                Name = newEmp.name,
                PhoneNumber = newEmp.phone
            };

            var result = await _userManager.CreateAsync(user, newEmp.password);
            if (result.Succeeded)
            {
                var userData = await _userManager.FindByEmailAsync(user.Email);
                if (userData != null)
                {
                    await _userManager.AddToRoleAsync(userData, newEmp.role);
                    var employee = new Employee
                    {
                        UserId = userData.Id,
                        BranchId = branch.Id
                    };
                    context.Employees.Add(employee);
                    return employee;
                }
                throw new Exception("فشلت العملية");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"فشلت العملية: {errors}");
            }
        }
        #endregion

        #region Get Employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            var employee = context.Employees.FirstOrDefault(e => e.User.Id == id && e.User.IsDeleted != true);
            return employee;
        }
        #endregion

        #region Search Employees
        public List<Employee> Search(string query)
        {
            var employees = context.Employees.Where(e => e.User.Name.Contains(query) || e.User.Email.Contains(query) 
                    || e.User.PhoneNumber.Contains(query) ||e.Branch.Name.Contains(query)).Where(e => e.User.IsDeleted != true).ToList();
            return employees;
        }
        #endregion

        #region Update Employee (empDto)
        public async Task<Employee> Update(EmpDTO NewData, UserManager<AppUser> _userManager)
        {
            var employee = context.Employees.FirstOrDefault(e => e.User.Id == NewData.id);
            if (employee == null) throw new Exception("الموظف غير موجود");

            employee.User.Name = NewData.name;

            //employee.User.Email = NewData.email;
            var existingEmail = await _userManager.GetEmailAsync(employee.User);
            if (existingEmail != NewData.email)
            {
                var emailUpdateResult = await _userManager.SetEmailAsync(employee.User, NewData.email);
                if (!emailUpdateResult.Succeeded)
                {
                    throw new Exception("فشلت العملية"); ;
                }
            }

            employee.User.PhoneNumber = NewData.phone;

            var roles = await _userManager.GetRolesAsync(employee.User);
            if (roles.Any() && roles[0] != NewData.role)
            {
                await _userManager.RemoveFromRoleAsync(employee.User, roles[0]);
                await _userManager.AddToRoleAsync(employee.User, NewData.role);
            }

            var branch = context.Branches.FirstOrDefault(b => b.Id == NewData.branchId);
            if (branch != null && employee.BranchId != branch.Id)
            {
                employee.BranchId = branch.Id;
            }
            return employee;
        }
        #endregion

        #region Update Employee Status (emp,status)
        public async Task<Employee> UpdateStatus(Employee employee, bool status)
        {
            if (employee != null)
            {
                employee.User.Status = status;
                return employee;
            }
            throw new Exception("الموظف غير موجود");
        }
        #endregion

        #region Soft Delete For Employee (id)
        public async Task SoftDeleteAsync(Employee employee)
        {
            if (employee != null)
            {
                employee.User.IsDeleted = true;
            }
        }
        #endregion
    }
}
