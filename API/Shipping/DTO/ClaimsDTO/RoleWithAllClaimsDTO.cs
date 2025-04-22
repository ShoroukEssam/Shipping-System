namespace Shipping.DTO.ClaimsDTO
{
    public class RoleWithAllClaimsDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ClaimsForCheckBoxDTO> AllRoleCalims { get; set; }
    }
}
