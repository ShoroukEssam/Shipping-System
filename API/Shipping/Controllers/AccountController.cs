using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.DTO.AccountDTOs;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;
using Shipping.DTO.Employee_DTOs;
using static Shipping.Constants.Permissions;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<UserRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        #region Login
        [HttpPost("login")]
        [AllowAnonymous]
        //[SwaggerOperation(Summary = "Authenticates a user and returns a JWT token.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully authenticated and returns a JWT token.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email or password.")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.email);
                if (user == null)
                {
                    return BadRequest(new { message = "البريد الالكتروني او اسم المستخدم غير صحيح" });
                }

                var result = await _signInManager.PasswordSignInAsync(user, login.password, login.rememberMe, false);
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();
                var userClaims = await _userManager.GetClaimsAsync(user);
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                userClaims.Add(new Claim(ClaimTypes.Role, roleName));

                string key = "Iti ii iii ii ii iiiiiiii iiiiiiii iiiiii iii ii";
                var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var credential = new SigningCredentials(secertkey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: credential
                );

                var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    var UserData = _mapper.Map<UserDTO>(user);
                    UserData.role = roles.FirstOrDefault();
                    UserData.roleId = role?.Id;
                    UserData.token = tokenstring;
                    return Ok(new { message = "تم تسجيل الدخول",User= UserData });
                }
                else
                {
                    return BadRequest(new { message = "كلمة المرور غير صحيحة" });
                }
            }
            return BadRequest(new { message = "البريد الالكتروني او كلمة المرور غير صحيحين" });
        }
        #endregion

        #region Logout
        [HttpPost("logout")]
        [Permission("anyUser")]
        [SwaggerOperation(Summary = "Logs out the current user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully logged out.")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "تم تسجيل الخروج" });
        }
        #endregion

        #region Change Password
        [HttpPost("changePassword")]
        [Permission("anyUser")]
        [SwaggerOperation(Summary = "Changes the password for the logged-in user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password changed successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid password details.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not found.")]
        public async Task<IActionResult> ChangePassword(PasswordDTO password)
        {
            var userClaims = ReturnUser(HttpContext);
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { message = "هذا المستخدم غير موجود" });
            }
            bool OldPassCheck = await _userManager.CheckPasswordAsync(user, password.oldPassword);
            if (!OldPassCheck)
            {
                return BadRequest(new { message = "كلمة المرور القديمة غير متطابقة" });
            }
            if (OldPassCheck && password.newPassword == password.confirmNewPassword)
            {
                var result = await _userManager.ChangePasswordAsync(user, password.oldPassword, password.newPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "خطأ في تغير كلمة المرور", errors = result.Errors });
                }

                //await _signInManager.SignOutAsync();
                return Ok(new { message = "تم تغير كلمة المرور بنجاح" });
            }
            else
            {
                return BadRequest(new { message = "كلمة المرور غير متطابقة" });
            }
        }
        #endregion

        private ClaimsPrincipal ReturnUser(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                // Handle missing or invalid Authorization header
                throw new UnauthorizedAccessException("Missing or invalid Authorization header.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal user;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,  // Set to true if you need to validate issuer
                    ValidateAudience = false, // Set to true if you need to validate audience
                    ValidateIssuerSigningKey = true, // Ensure issuer signing key is validated
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Iti ii iii ii ii iiiiiiii iiiiiiii iiiiii iii ii")),

                };

                user = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid token.", ex);
            }

            return user;
        }
    }
}
