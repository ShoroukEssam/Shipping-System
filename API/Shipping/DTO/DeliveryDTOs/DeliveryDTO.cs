using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTO.DeliveryDTOs
{
    public class DeliveryDTO
    {
        public string? Id { get; set; }

        public string? DeliveryId { get; set; }
        
        public string? Name { get; set; }

        public string? Email { get; set; }
       
        public string? Password { get; set; }
       
        public string? Phone { get; set; }
       
        public string? BranchName { get; set; }
        
        public string Government { get; set; }
       
        public string Address { get; set; }
        
        public bool status { get; set; }

        public DiscountType DiscountType { get; set; }
        
        public int CompanyPercentage { get; set; }
        
        public int? BranchId { get; set; }
        
        public int? OrignalIdOnlyInDeliveryTable { get; set; }
        
        public string? DeliverName { get; set; }
    }
}
