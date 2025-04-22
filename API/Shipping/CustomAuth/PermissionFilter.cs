using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shipping.CustomAuth.RoleClaimService;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.IdentityModel.Tokens;

public class PermissionFilter : IAsyncActionFilter
{
    private readonly string _permission;
    private readonly IRoleClaimService _roleClaimService;

    public PermissionFilter(string permission, IRoleClaimService roleClaimService)
    {
        _permission = permission ?? throw new ArgumentNullException(nameof(permission));
        _roleClaimService = roleClaimService ?? throw new ArgumentNullException(nameof(roleClaimService));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        ClaimsPrincipal user;

        try
        {
            user = handler.ValidateToken(token, new TokenValidationParameters
            {
                // Validate the token parameters based on your configuration
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                {
                    var jwt = new JwtSecurityToken(token);

                    return jwt;
                }
            }, out SecurityToken validatedToken);
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (user != null && _permission == "anyUser") await next();

        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        if (userId == null || !await _roleClaimService.UserHasPermissionAsync(userId, _permission))
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}
