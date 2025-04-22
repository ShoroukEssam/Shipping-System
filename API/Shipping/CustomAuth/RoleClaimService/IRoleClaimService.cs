namespace Shipping.CustomAuth.RoleClaimService
{
    public interface IRoleClaimService
    {
        Task<bool> UserHasPermissionAsync(string userId, string permission);
    }
}
