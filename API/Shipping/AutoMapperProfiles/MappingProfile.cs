using AutoMapper;
using Shipping.DTO;
using Shipping.DTO.BranchDTOs;
using Shipping.DTO.DeliveryDTOs;
using Shipping.DTO.Employee_DTOs;
using Shipping.DTO.MerchantDTOs;
using Shipping.Models;
using AutoMapper;
using CloudinaryDotNet.Core;
using Shipping.DTO.BranchDTOs;
using Shipping.DTO.MerchantDTOs;
using Shipping.DTO.MerchantDTOs;
using Shipping.Models;
using Shipping.DTO.CityDTO;
using Shipping.DTO.GovernmentDTO;
using Shipping.DTO.OrderDTO;
using Shipping.DTO.AccountDTOs;
using Microsoft.AspNetCore.Identity;
using Shipping.DTO.SpecialCitiesPricesDTO;

namespace Shipping.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        private readonly UserManager<AppUser> _userManager;

        public MappingProfile()
        {
            #region user
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status));
            #endregion

            #region City 
            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.shippingPrice, opt => opt.MapFrom(src => src.ShippingPrice))
                .ForMember(dest => dest.pickUpPrice, opt => opt.MapFrom(src => src.PickUpPrice))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.governmentId, opt => opt.MapFrom(src => src.GovernmentId))
                .ForMember(dest => dest.governmentName, opt => opt.MapFrom(src => src.Government.Name))

                ;
            CreateMap<CityDTO, City>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
               .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.shippingPrice))
               .ForMember(dest => dest.PickUpPrice, opt => opt.MapFrom(src => src.pickUpPrice))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
               .ForMember(dest => dest.GovernmentId, opt => opt.MapFrom(src => src.governmentId))

               ;
            #endregion

            #region from WeightSetting to WeightSettingDTO
            CreateMap<WeightSetting, WeightSettingDTO>()
            .ForMember(dest => dest.standaredWeight, opt => opt.MapFrom(src => src.StandaredWeight))
            .ForMember(dest => dest.addition_Cost, opt => opt.MapFrom(src => src.Addition_Cost)).ReverseMap()
            ;

            #endregion

            #region from UserRole to UserRoleDTO
            CreateMap<UserRole, UserRoleDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date)).ReverseMap()
            ;

            #endregion


            #region Government 
            CreateMap<Government, GovernmentDTO>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap()
                ;
            #endregion

            #region Order
            CreateMap<Order, OrderDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SerialNumber))
                    .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
                    .ForMember(dest => dest.ClientPhoneNumber1, opt => opt.MapFrom(src => src.ClientPhoneNumber1))
                    .ForMember(dest => dest.ClientPhoneNumber2, opt => opt.MapFrom(src => src.ClientPhoneNumber2))
                    .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.ClientEmail))
                    .ForMember(dest => dest.OrderCost, opt => opt.MapFrom(src => src.OrderCost))
                    .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.TotalWeight))
                    .ForMember(dest => dest.IsVillage, opt => opt.MapFrom(src => src.IsVillage))
                    .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.City.Government.Name))
                    .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                    .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.StreetName))
                    .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                    .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost))
                    .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
                    .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.orderProducts))
                    .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                    .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => src.Delivery.User.Name))
                    .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.ShippingType, opt => opt.MapFrom(src => src.ShippingType))
                    .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                    .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                    .ReverseMap();
 
            CreateMap<OrderProduct, OrderProductDTO>().ReverseMap();


            #endregion

            #region map Employee - EmpDTO
            CreateMap<Employee, EmpDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.User.Status))
                .ForMember(dest => dest.branchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region From Delivery To DeliveryDTO

            CreateMap<Delivery, DeliveryDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Government, opt => opt.MapFrom(src => src.Governement))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.User.Status))
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType))
            .ForMember(dest => dest.CompanyPercentage, opt => opt.MapFrom(src => src.CompanyPercent))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch.Id));

            // Map from DeliveryDTO to Delivery
            CreateMap<DeliveryDTO, Delivery>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.DeliveryId))
                .ForPath(dest => dest.User.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForPath(dest => dest.Branch.Name, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.Governement, opt => opt.MapFrom(src => src.Government))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(dest => dest.User.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType))
                .ForMember(dest => dest.CompanyPercent, opt => opt.MapFrom(src => src.CompanyPercentage))
                .ForPath(dest => dest.Branch.Id, opt => opt.MapFrom(src => src.BranchId));



            #endregion

            #region Map Merchant - MerchantDTO


            CreateMap<Merchant, MerchantDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.User.Status))
                .ForMember(dest => dest.branchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.government, opt => opt.MapFrom(src => src.Government.Name))
                .ForMember(dest => dest.pickUpSpecialCost, opt => opt.MapFrom(src => src.PickUpSpecialCost))
                .ForMember(dest => dest.refusedOrderPercent, opt => opt.MapFrom(src => src.RefusedOrderPercent))
                .ForMember(dest => dest.SpecialCitiesPrices, opt => opt.MapFrom(src => src.SpecialCitiesPrices));

            CreateMap<MerchantDTO, Merchant>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.PickUpSpecialCost, opt => opt.MapFrom(src => src.pickUpSpecialCost))
                .ForMember(dest => dest.RefusedOrderPercent, opt => opt.MapFrom(src => src.refusedOrderPercent))
                .ForMember(dest => dest.BranchId, opt => opt.Ignore())
                .ForMember(dest => dest.CityId, opt => opt.Ignore())
                .ForMember(dest => dest.GovernmentId, opt => opt.Ignore())
                .ForMember(dest => dest.SpecialCitiesPrices, opt => opt.MapFrom(src => src.SpecialCitiesPrices));

            
            /*
            CreateMap<Merchant, MerchantDTO>()
                    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.User.Email))
                    .ForMember(dest => dest.password, opt => opt.Ignore())
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.User.Name))
                    .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
                    .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.User.Status))
                    .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.City.Name))
                    .ForMember(dest => dest.pickUpSpecialCost, opt => opt.MapFrom(src => src.PickUpSpecialCost))
                    .ForMember(dest => dest.refusedOrderPercent, opt => opt.MapFrom(src => src.RefusedOrderPercent))
                    .ReverseMap();
            CreateMap<MerchantDTO, Merchant>()
          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.id))
          .ForMember(dest => dest.User.Email, opt => opt.MapFrom(src => src.email))
          .ForMember(dest => dest.User.PasswordHash, opt => opt.MapFrom(src => src.password))
          .ForMember(dest => dest.User.Name, opt => opt.MapFrom(src => src.name))
          .ForMember(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.phone))
          .ForMember(dest => dest.User.Status, opt => opt.MapFrom(src => src.status))
         .ForMember(dest => dest.Branch.Name, opt => opt.MapFrom(src => src.branchName))
          .ForMember(dest => dest.City.Name, opt => opt.MapFrom(src => src.city))
          .ForMember(dest => dest.Government.Name, opt => opt.MapFrom(src => src.government))
          .ForMember(dest => dest.PickUpSpecialCost, opt => opt.MapFrom(src => src.pickUpSpecialCost))
          .ForMember(dest => dest.RefusedOrderPercent, opt => opt.MapFrom(src => src.refusedOrderPercent));
            */



            #endregion


            #region Branch Mapper
            CreateMap<Branch, BranchDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.StateId))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Government.Name)) // تعيين اسم المحافظة
                    ;
            CreateMap<BranchDTO, Branch>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.StateId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            #endregion


            CreateMap<SpecialCitiesPrice, SpecialCitiesPriceDTO>()
               .ForMember(dest => dest.Government, opt => opt.MapFrom(src => src.Government))
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId));



        }
        public async Task<string> GetRole(AppUser user)
        {
            if (user == null) return string.Empty;

            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? string.Empty;
        }
    }
}
