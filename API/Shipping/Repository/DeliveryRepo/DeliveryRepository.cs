using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using Shipping.DTO.DeliveryDTOs;
using Shipping.Models;
using Shipping.Repository.OrderRepo;

namespace Shipping.Repository.DeliveryRepo
{
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository

    {
        ShippingContext context;
        private readonly UserManager<AppUser> userManager;
        public DeliveryRepository(ShippingContext context , UserManager<AppUser> userManager) :base(context) 
        {
            this.context = context;
            this.userManager = userManager;
        }
        #region Get All Delivery With Specefic Name

        Task<List<Delivery>> IDeliveryRepository.GetAll(string Name)
        {
            var Delivery = context.Deliveries
                .Include(d => d.User)
                .Include(d => d.Branch)
                .Where(d => d.User.Name.Contains(Name) && d.User.IsDeleted == false)
                .ToListAsync();

            return Delivery;
        }

        #endregion

        #region Get All Deliveries

        Task<List<Delivery>> IDeliveryRepository.GetAllDeliveries()
        {
            var Delivery = context.Deliveries
                .Include(d => d.User)
                .Include(d => d.Branch)
                .Where(d => d.User.IsDeleted == false)
                .ToListAsync();

            return Delivery;
        }

        #endregion

        #region Get Delivery By Id

        public Task<Delivery> GetById(string id)
        {
            var delivery =  context.Deliveries
                .Where(m => m.User.IsDeleted == false)
                .FirstOrDefaultAsync(m => m.User.Id == id);

            if (delivery == null)
            {
                return null;
            }

            return delivery;
        }

        #endregion

        #region Edit Delivery
        public async Task<Delivery> EditDelivery(string id, Delivery delivery)
        {
            var oldDelivery = await GetById(id);

            if (oldDelivery != null)
            {
                // Check if the branch is already being tracked
                var trackedBranch = context.Branches.Local.FirstOrDefault(b => b.Id == delivery.BranchId);

                if (trackedBranch != null)
                {
                    // Detach the tracked branch
                    context.Entry(trackedBranch).State = EntityState.Detached;
                }

                // Update the delivery fields
                oldDelivery.Governement = delivery.Governement;
                oldDelivery.Address = delivery.Address;
                oldDelivery.BranchId = delivery.BranchId;
                oldDelivery.DiscountType = delivery.DiscountType;
                oldDelivery.CompanyPercent = delivery.CompanyPercent;
                oldDelivery.User.NormalizedEmail = delivery.User.Email.ToUpper();

                // Attach the updated branch
                context.Attach(delivery).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return oldDelivery;
            }

            return null;
        }


        #endregion

        #region Add Delivery
        public async Task<Delivery> Insert(DeliveryDTO deliverydto)
        {

            var delivery = new Delivery
            {
                Governement = deliverydto.Government,
                Address = deliverydto.Address,
                DiscountType = deliverydto.DiscountType,
                CompanyPercent = deliverydto.CompanyPercentage,
                BranchId = (int)deliverydto.BranchId
            };

            var user = new AppUser
            {
                Name = deliverydto.Name,
                Email = deliverydto.Email,
                PhoneNumber = deliverydto.Phone,
                UserName = deliverydto.Name
            };

            var result = await userManager.CreateAsync(user, deliverydto.Password);

            if (result.Succeeded)
            {
                var _user = await userManager.FindByEmailAsync(user.Email);
                if (_user != null)
                    await userManager.AddToRoleAsync(_user, "المناديب");

                delivery.UserId = _user.Id;
                context.Add(delivery);
                context.SaveChanges();

                return null;
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"فشلت العملية: {errors}");
        }

        #endregion

        #region Update Status
        public void UpdateStatus(Delivery delivery, bool status)
        {
            if (delivery != null)
            {
                delivery.User.Status = status;
                context.SaveChanges();
            }
        }
        #endregion

        #region Get All Branches
        public List<Branch> GetAllBranches()
        {
            var Branchs = context.Branches.Where(b => b.IsDeleted==false).ToList();
            return Branchs;
        }

        #endregion

        #region Get All States
        public List<Government> GetAllGovernments()
        {
            var Governments = context.Governments.Where(s => s.IsDeleted==false).ToList();
            return Governments;
        }
        #endregion

        #region Soft Delete For Merchant (id)
        public async Task SoftDeleteAsync(Delivery delivery)
        {
            if (delivery != null)
            {
                delivery.User.IsDeleted = true;
            }
        }
        #endregion


    }
}
