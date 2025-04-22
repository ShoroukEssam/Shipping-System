using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Constants;
using Shipping.DTO;
using Shipping.DTO.ClaimsDTO;
using Shipping.Models;
using Shipping.Repository.ArabicNamesForRoleClaims;
using Shipping.UnitOfWork;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IAddArabicNamesForRoleClaims _addArabicNamesForRoleClaims;
        private readonly IMapper _Mapper;

        public AdministrationController(IMapper Mapper,RoleManager<UserRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAddArabicNamesForRoleClaims addArabicNamesForRoleClaims)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _Mapper=Mapper;
            _addArabicNamesForRoleClaims = addArabicNamesForRoleClaims;
        }

        #region Get the list of roles
        [HttpGet]
        [Permission(Permissions.Controls.View)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the list of roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "when get the List of Roles Successfully")]
        public async Task<IActionResult> ListRoles()
        {
           var roles = await _roleManager.Roles.ToListAsync();
            List<UserRoleDTO> rolesDTO = _Mapper.Map<List<UserRoleDTO>>(roles);
           return Ok(rolesDTO);
        }
        #endregion

        #region Create new Role
        [HttpPost]
        [Permission(Permissions.Controls.Create)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Create new Role")]
        [SwaggerResponse(StatusCodes.Status200OK, "when Create new Role Successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "when can't Create new Role")]
        public async Task<IActionResult> CreateRole(UserRoleDTO roleDTO)
        {
            UserRole userRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleDTO.roleName,
                Date = DateTime.Now.ToString()
            };

            var result = await _roleManager.CreateAsync(userRole);
            if (result.Succeeded)
            {
                return Ok(userRole);
            }
            return BadRequest(new {message="لم يتم اضافة المجموعة من فضلك تاكد من البيانات المدخلة"});
        }
        #endregion

        #region Edit  Role
        [HttpPut("{id}")]
        [Permission(Permissions.Controls.Edit)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Edit Role")]
        [SwaggerResponse(StatusCodes.Status200OK, "when Edit the Role Successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "when the Edit process failed or you are not allowed to edit this role")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "when can't find a role with this id ")]
        public async Task<IActionResult> EditRole(string id,UserRoleDTO roleDTO)
        {
            if(id!=roleDTO.id) return BadRequest(new {message=$"{id} لا يساوي {roleDTO.id}"});

            UserRole role = await _roleManager.FindByIdAsync(id);
            if (role == null) 
                return NotFound(new { message = $"{id}  لا توجد مجموعة تحمل هذا الرقم التعريفي  " });
            else if (role.Name == "Admin" || role.Name == "التجار" || role.Name == "الموظفين" || role.Name == "المناديب")
             return BadRequest(new { message = $"{role.Name} لا يمكن التعديل علي هذة المجموعة  " });
            else
            {
                role.Name = roleDTO.roleName;
                role.Date =roleDTO.date= DateTime.Now.ToString();
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded) return Ok(roleDTO);
                else return BadRequest(new { message = "لم يتم تحديث بيانات المجموعة" });

            }
        }
        #endregion

        #region Delete  Role
        [HttpDelete("{id}")]
        [Permission(Permissions.Controls.Delete)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Delete Role")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "when delete the Role Successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "when the Delete process failed or you are not allowed to delete this role")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "when can't find a role with this id ")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            UserRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { message = $"{id}  لا توجد مجموعة تحمل هذا الرقم التعريفي  " });
            else if (role.Name == "Admin" || role.Name == "التجار" || role.Name == "الموظفين" || role.Name == "المناديب")
                return BadRequest(new { message = $"{role.Name} لا يمكن  حذف هذة المجموعة  " });
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) return NoContent();
                else return BadRequest(new { message = "لم يتم  حذف المجموعة" });

            }
        }
        #endregion

        #region Search for Role
        [HttpGet("{query}")]
        [Permission(Permissions.Controls.View)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Search for Role")]
        [SwaggerResponse(StatusCodes.Status200OK, "when roles are found Successfully")]
        public async Task<IActionResult> SearchRole(string query)
        {
            List<UserRole> roles;
            if (string.IsNullOrWhiteSpace(query)) { roles = await _roleManager.Roles.ToListAsync(); }
            else roles = await _roleManager.Roles.Where(r => r.Name.Contains(query)).ToListAsync();
        
            List<UserRoleDTO> rolesDTO=_Mapper.Map<List<UserRoleDTO>>(roles);
            return Ok(rolesDTO);

        }
        #endregion

        #region Get Permissions on roles
        [HttpGet("GetPermissionsOnRole/{id}")]
        [Permission(Permissions.Controls.View)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Manage Permissions on roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "when return the Role and All Claims Successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "when can't find a role with this id ")]
        public async Task<IActionResult> GetPermissionsOnRole(string id)
        {

            UserRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { message = $"{id}  لا توجد مجموعة تحمل هذا الرقم التعريفي  " });

            List<string> claimsOfThisRole = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            List<string> allClaimsInApp = Permissions.GenerateAllPermissions();

            List<ClaimsForCheckBoxDTO> claimsForCheckBoxDTOs=allClaimsInApp.Select(p => new ClaimsForCheckBoxDTO { DisplayValue = p }).ToList();
            foreach (var claimDTO in claimsForCheckBoxDTOs)
            {
                if (claimsOfThisRole.Any(c => c == claimDTO.DisplayValue)) claimDTO.IsSelected = true;

               
                foreach (var item in EnglishVsArabic.ModulesInEn_AR)
                {
                    if (item.Key == claimDTO.DisplayValue.Split(".")[1])
                    {
                        claimDTO.ArabicName= item.Value;
                        break;
                    }
                }
            }
            RoleWithAllClaimsDTO roleWithAllClaimsDTOs = new()
            {
                RoleId = role.Id,
                RoleName = role.Name,
                AllRoleCalims = claimsForCheckBoxDTOs
            };

            return Ok(roleWithAllClaimsDTOs);

        }
        #endregion

        #region Edit Permissions on role
        [HttpPut("EditPermissionsOnRole/{id}")]
        [Permission(Permissions.Controls.View)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Manage Permissions on roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "when return the Role and All Claims Successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "when can't find a role with this id ")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "when id in url don't match the role id ")]
        public async Task<IActionResult> EditPermissionsOnRole(string id, RoleWithAllClaimsDTO roleWithAllClaimsDTO)
        {
            if(id!=roleWithAllClaimsDTO.RoleId)return BadRequest(new { message = $"{id} لا يساوي {roleWithAllClaimsDTO.RoleId}" });
            UserRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { message = $"{id}  لا توجد مجموعة تحمل هذا الرقم التعريفي  " });

            //removing old claims
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims){ await _roleManager.RemoveClaimAsync(role, claim); }

            //creating new claims based on checked permissons
           List<ClaimsForCheckBoxDTO> selectedClaims = roleWithAllClaimsDTO.AllRoleCalims.Where(c => c.IsSelected).ToList();
            //adding Arabic names for Claims
            foreach (ClaimsForCheckBoxDTO claim in selectedClaims)
            {
                var arabicName = "";
                foreach (var item in EnglishVsArabic.ModulesInEn_AR)
                {
                    if (item.Key == claim.DisplayValue.Split(".")[1])
                    {
                        arabicName = item.Value;
                        break;
                    }
                }
                await _roleManager.AddClaimAsync(role, new Claim("Permissions", claim.DisplayValue));

                var result = _addArabicNamesForRoleClaims.AddArabicNamesToRoleCaims(role, arabicName, claim.DisplayValue);
                if (!result) return NotFound();

            }

            return NoContent();

        }
        #endregion








    }
}
