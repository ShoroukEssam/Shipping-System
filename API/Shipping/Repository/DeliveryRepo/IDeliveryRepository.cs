using Microsoft.AspNetCore.Identity;
using Mono.TextTemplating;
using Shipping.DTO.DeliveryDTOs;
using Shipping.Models;

namespace Shipping.Repository.DeliveryRepo
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetAll(string Name);
        Task<List<Delivery>> GetAllDeliveries();
        Task<Delivery> GetById(string id);
        Task<Delivery> EditDelivery(string id, Delivery delivery);
        Task<Delivery> Insert(DeliveryDTO delivery);

        Task SoftDeleteAsync(Delivery delivery);
        void UpdateStatus(Delivery delivery, bool status);
        List<Branch> GetAllBranches();
        List<Government> GetAllGovernments();
    }
}
