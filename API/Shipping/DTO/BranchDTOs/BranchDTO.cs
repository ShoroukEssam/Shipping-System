using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTO.BranchDTOs
{
    public class BranchDTO
    {
        public int? Id { get; set; }
        public int? StateId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }= DateTime.UtcNow;
        public bool? Status { get; set; } = true;
        public string? State {  get; set; }

    }
}
