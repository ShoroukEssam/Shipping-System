using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.DTO.OrderDTO;
using Shipping.Models;
using Shipping.UnitOfWork;
using static Shipping.Constants.Permissions;

namespace Shipping.Repository.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShippingContext _myContext;


        public OrderRepository(ShippingContext myContext)
        {
            _myContext = myContext ?? throw new ArgumentNullException(nameof(myContext));

        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _myContext.Orders
                    .Where(o => !o.IsDeleted)
                    .ToListAsync();

                return (orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _myContext.Orders
                    .FirstOrDefaultAsync(o => o.SerialNumber == id);

                if (order == null)
                    throw new Exception("الطلب غير موجود.");

                return (order);
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في جلب الطلب حسب الرقم.", ex);
            }
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string orderStatus)
        {
            try
            {
                var orders = await _myContext.Orders
                    .Where(o => o.OrderStatus == orderStatus && !o.IsDeleted)
                    .ToListAsync();

                return (orders);
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في جلب الطلبات حسب الحالة.", ex);
            }
        }

        public async Task<Order> AddOrderAsync(OrderDTO Order, string userId)
        {
            try
            {
                var city = await _myContext.Cities.FirstOrDefaultAsync(c => c.Name == Order.CityName);

                if (city == null)
                    throw new Exception("المدينة غير موجودة.");
                if (Order.MerchantId == 0)
                    throw new Exception("التاجر غير موجود.");

                var order = new Order
                {
                    MerchantId = Order.MerchantId ?? 0,
                    CityId = city.Id,
                    IsVillage = Order.IsVillage,
                    ClientEmail = Order.ClientEmail,
                    ClientName = Order.ClientName,
                    ClientPhoneNumber1 = Order.ClientPhoneNumber1,
                    ClientPhoneNumber2 = Order.ClientPhoneNumber2,
                    Notes = Order.Notes,
                    OrderCost = Order.OrderCost,
                    PaymentType = Order.PaymentType,
                    ShippingType = Order.ShippingType,
                    StreetName = Order.StreetName,
                    TotalWeight = Order.TotalWeight,
                    Type = Order.Type,
                    GovernmentId = city.GovernmentId,
                    ShippingCost = Order.ShippingCost,
                    orderProducts = Order.OrderProducts.Select(op => new OrderProduct
                    {
                        ProductName = op.ProductName,
                        ProductQuantity = op.ProductQuantity,
                        Weight = op.Weight
                    }).ToList()
                };

                var branch = await _myContext.Branches.FirstOrDefaultAsync(b => b.Name == Order.BranchName);
                if (branch == null)
                    throw new Exception("الفرع غير موجود.");

                order.BranchId = branch.Id;

                _myContext.Orders.Add(order);
                await _myContext.SaveChangesAsync();



                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في إضافة الطلب.", ex);
            }
        }

        public async Task<Order> EditOrderAsync(int id, OrderDTO orderDTO)
        {
            try
            {
                var order = await _myContext.Orders.FirstOrDefaultAsync(o => o.SerialNumber == id);
                if (order == null)
                    throw new Exception("الطلب غير موجود.");

                var city = await _myContext.Cities.FirstOrDefaultAsync(c => c.Name == orderDTO.CityName);
                if (city == null)
                    throw new Exception("المدينة غير موجودة.");

                order.CityId = city.Id;
                order.IsVillage = orderDTO.IsVillage;
                order.ClientEmail = orderDTO.ClientEmail;
                order.ClientName = orderDTO.ClientName;
                order.ClientPhoneNumber1 = orderDTO.ClientPhoneNumber1;
                order.ClientPhoneNumber2 = orderDTO.ClientPhoneNumber2;
                order.Notes = orderDTO.Notes;
                order.OrderCost = orderDTO.OrderCost;
                order.PaymentType = orderDTO.PaymentType;
                order.ShippingType = orderDTO.ShippingType;
                order.StreetName = orderDTO.StreetName;
                order.TotalWeight = orderDTO.TotalWeight;
                order.Type = orderDTO.Type;
                order.GovernmentId = city.GovernmentId;
                order.ShippingCost = orderDTO.ShippingCost;

                var branch = await _myContext.Branches.FirstOrDefaultAsync(b => b.Name == orderDTO.BranchName);
                if (branch == null)
                    throw new Exception("الفرع غير موجود.");

                order.BranchId = branch.Id;

                order.orderProducts.Clear();
                order.orderProducts = orderDTO.OrderProducts.Select(op => new OrderProduct
                {
                    ProductName = op.ProductName,
                    ProductQuantity = op.ProductQuantity,
                    Weight = op.Weight
                }).ToList();

                await _myContext.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في تعديل الطلب.", ex);
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                var order = await _myContext.Orders.FirstOrDefaultAsync(o => o.SerialNumber == orderId);
                if (order == null)
                    throw new Exception("الطلب غير موجود.");

                order.OrderStatus = status;
                await _myContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في تحديث حالة الطلب.", ex);
            }
        }

        public async Task UpdateOrderDeliveryAsync(int orderId, int deliveryId)
        {
            try
            {
                var order = await _myContext.Orders.FirstOrDefaultAsync(o => o.SerialNumber == orderId);
                if (order == null)
                    throw new Exception("الطلب غير موجود.");

                order.OrderStatus = "قيد_الانتظار";
                order.DeliveryId = deliveryId;
                await _myContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في تحديث تسليم الطلب.", ex);
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _myContext.Orders.FirstOrDefaultAsync(o => o.SerialNumber == orderId);
                if (order == null)
                    throw new Exception("الطلب غير موجود.");

                order.IsDeleted = true;
                await _myContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في حذف الطلب.", ex);
            }
        }

        public async Task<int> CalculateShippingPrice(OrderDTO order, IUnitOfWork<City> _city, IUnitOfWork<WeightSetting> _WeightSettingUnit, IUnitOfWork<SpecialCitiesPrice> _SpecialCityPriceUnit)
        {
            int baseShippingPrice = 0;
            int merchant = order.MerchantId ?? 0;

            SpecialCitiesPrice specialPrice = await GetSpecialPrices(merchant, order.CityName, _SpecialCityPriceUnit);
            if (specialPrice != null)
            {
                baseShippingPrice = specialPrice.Price;
            }
            else
            {
                baseShippingPrice = await CityShippingPrice(order, _city);
            }

            int totalShippingPrice = baseShippingPrice;
            totalShippingPrice += await CostToVillage(order.IsVillage, baseShippingPrice);
            totalShippingPrice += (int)await CostShippingType(order.ShippingType, baseShippingPrice);
            totalShippingPrice += (int)await CostAdditionalWeight(order.TotalWeight, _WeightSettingUnit);
            //totalShippingPrice += (int)await CostPaymentType(order.PaymentType, baseShippingPrice);

            return totalShippingPrice;
        }


        #region calculate price
        private async Task<SpecialCitiesPrice> GetSpecialPrices(int merchantId, string city, IUnitOfWork<SpecialCitiesPrice> _SpecialCityPriceUnit)
        {
            var result = await _SpecialCityPriceUnit.Repository.GetAllAsync();
            if (result != null)
            {
                var specialPrices = result.Where(sp => sp.City == city && sp.MerchantId == merchantId).FirstOrDefault();
                return specialPrices;
            }
            return null;
        }
        private async Task<int> CityShippingPrice(OrderDTO order, IUnitOfWork<City> _unit)
        {
            var result = _unit.CityRepository.GetByName(order.CityName);
            if (result != null)
            {
                if (order.Type == Models.Type.تسليم_فالفرع)
                    return result.PickUpPrice;

                else if (order.Type == Models.Type.توصيل_الي_المنزل)
                    return result.ShippingPrice;
            }
            return 0;
        }
        private async Task<int> CostToVillage(bool isDeliverToVillage, int basePrice)
        {
            if (isDeliverToVillage)
            {
                return (int)(basePrice * 0.20);
            }
            return 0;
        }
        private async Task<double> CostShippingType(ShippingType shippingType, int basePrice)
        {
            return shippingType switch
            {
                ShippingType.توصيل_سريع => basePrice * 0.75,
                ShippingType.توصيل_في_نفس_اليوم => basePrice * 0.50,
                _ => 0,
            };
        }
        private async Task<double> CostAdditionalWeight(double totalWeight, IUnitOfWork<WeightSetting> _WeightSettingUnit)
        {
            double cost = 0;
            double defaultWeight = 0;
            double additionalPrice = 0;
            WeightSetting result = await _WeightSettingUnit.Repository.GetByIdAsync(1);
            if (result != null)
            {
                defaultWeight = result.StandaredWeight;
                totalWeight = totalWeight / 1000;
                if (totalWeight > defaultWeight)
                {
                    additionalPrice = result.Addition_Cost;
                    totalWeight = totalWeight - defaultWeight;
                    cost = totalWeight * additionalPrice;
                }
            }
            return cost;
        }
        private async Task<double> CostPaymentType(PaymentType paymentType, double price)
        {
            if (paymentType == PaymentType.دفع_مقدم)
            {
                return 0;
            }
            else if (paymentType == PaymentType.واجبة_التحصيل)
            {
                return 0;
            }
            else if (paymentType == PaymentType.طرد_مقابل_طرد)
            {
                return 0;
            }
            return 0;
        }

        #endregion

        // under 
        public async Task<List<string>> GenerateTableAsync(OrdersPlusDeliveriesDTO ordersPlusDeliveriesDTO)
        {
            try
            {
                var table = new List<string>
                {
                    "Order ID, Client Name, Delivery ID" // Example headers
                };

                foreach (var order in ordersPlusDeliveriesDTO.Orders)
                {
                    var delivery = ordersPlusDeliveriesDTO.Deliveries.FirstOrDefault(d => d.DeliveryId == order.DeliveryId.ToString());
                    var row = $"{order.Id}, {order.ClientName}, {delivery?.DeliveryId ?? "N/A"}";
                    table.Add(row);
                }

                return await Task.FromResult(table);
            }
            catch (Exception ex)
            {
                throw new Exception("خطأ في إنشاء الجدول.", ex);
            }
        }
    }

}

