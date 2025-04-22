using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shipping.Constants;
using Shipping.DTO.BranchDTOs;
using Shipping.DTO.CityDTO;
using Shipping.DTO.OrderDTO;
using Shipping.Models;
using Shipping.Repository.CityRepo;
using Shipping.Repository.GovernmentRepo;
using Shipping.Repository.OrderRepo;
using Shipping.UnitOfWork;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Shipping.Constants.Permissions;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork<Order> _unit;
        private readonly IUnitOfWork<WeightSetting> _WeightSettingUnit;
        private readonly IUnitOfWork<SpecialCitiesPrice> _SpecialCityPriceUnit;
        private readonly IUnitOfWork<City> _CityUnit;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly ShippingContext _myContext;
        private readonly IMapper _mapper;

        public OrderController(ShippingContext myContext,
            IUnitOfWork<WeightSetting> weightSettingUnit,
            IUnitOfWork<SpecialCitiesPrice> specialCityPriceUnit,
            IUnitOfWork<City> cityUnit,
            IUnitOfWork<Order> unit,
            UserManager<AppUser> userManager,
            RoleManager<UserRole> roleManager,
            IMapper mapper)
        {
            _unit = unit;
            _myContext = myContext;
            _WeightSettingUnit = weightSettingUnit;
            _SpecialCityPriceUnit = specialCityPriceUnit;
            _CityUnit = cityUnit;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

       #region Get Orders

        [HttpGet("Index")]
        [SwaggerOperation(Summary = "Retrieves all orders.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of orders.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<List<OrderDTO>>> Index()
        {
            try
            {
                var orders = await _unit.OrderRepository.GetAllOrdersAsync();
                var orderDTOs = _mapper.Map<List<OrderDTO>>(orders);
                return Ok(orderDTOs);
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب جميع الطلبات.");
            }
        }
        #endregion

       #region GetOrdersDependonStatus
        [HttpGet("GetOrdersDependonStatus")]
        [SwaggerOperation(Summary = "Retrieves orders based on status.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of orders based on status.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersDependonStatus(string? status = null)
        {
            try
            {
                var orders = status == null
                    ? await _unit.OrderRepository.GetAllOrdersAsync()
                    : await _unit.OrderRepository.GetOrdersByStatusAsync(status);

                var orderDTOs = _mapper.Map<List<OrderDTO>>(orders);
                return Ok(orderDTOs);
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب الطلبات حسب الحالة.");
            }
        }
        #endregion 

       #region SearchByClientName
        [HttpGet("SearchByClientName")]
        [SwaggerOperation(Summary = "Searches orders by client name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of orders that match the client name.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<List<OrderDTO>>> SearchByClientName(string query)
        {
            try
            {
                var orders = string.IsNullOrWhiteSpace(query)
                    ? await _unit.OrderRepository.GetAllOrdersAsync( )
                    : (await _unit.OrderRepository.GetAllOrdersAsync( ))
                        .Where(i => i.ClientName.ToUpper().Contains(query.ToUpper()))
                        .ToList();

                var orderDTOs = _mapper.Map<List<OrderDTO>>(orders);
                return Ok(orderDTOs);
            }
            catch
            {
                return StatusCode(500, "خطأ في البحث عن الطلبات بواسطة اسم العميل.");
            }
        }
        #endregion

       #region SearchByDeliveryName
        [HttpGet("SearchByDeliveryName")]
        [SwaggerOperation(Summary = "Searches orders by delivery name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of orders that match the delivery name.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> SearchByDeliveryName(string query)
        {
            try
            {
                List<Order> orders = new List<Order>();   
                var deliveries = await _unit.DeliveryRepository.GetAll(query);

                if (string.IsNullOrWhiteSpace(query))
                {
                    orders = await _unit.OrderRepository.GetAllOrdersAsync();
                }
                else
                {
                    var filteredDeliveries = deliveries.Where(d => d.User.Name.ToUpper().Contains(query.ToUpper())).ToList();

                    foreach (var delivery in filteredDeliveries)
                    {
                        var filteredOrders = (await _unit.OrderRepository.GetAllOrdersAsync()).Where(o => o.DeliveryId == delivery.Id).ToList();
                        orders.AddRange(filteredOrders);
                    }
                }
                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "خطأ في البحث عن الطلبات بواسطة اسم المندوب.");
            }
        }
        #endregion

       #region OrderReceipt
        [HttpGet("OrderReceipt")]
        [SwaggerOperation(Summary = "Retrieves the receipt of a specific order.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the receipt of the order.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<OrderDTO>> OrderReceipt(int id)
        {
            try
            {
                var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound("الطلب غير موجود.");

                var orderDTO = _mapper.Map<OrderDTO>(order);
                return Ok(orderDTO);
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب إيصال الطلب.");
            }
        }

        #endregion

       #region ChangeDelivery

        [HttpPut("ChangeDelivery")]
        [SwaggerOperation(Summary = "Changes the delivery of a specific order.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Delivery changed successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        [Permission(Permissions.Orders.Edit)]
        public async Task<IActionResult> ChangeDelivery(int id, int deliveryId)
        {
            try
            {
                var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { message = "تم تغير حالة المندوب" });

                await _unit.OrderRepository.UpdateOrderDeliveryAsync(id, deliveryId);
                return Ok(new { message = "تم تغير حالة المندوب" });

            }
            catch
            {
                return StatusCode(500, "خطأ في تغيير تسليم الطلب.");
            }
        }

        #endregion

       #region Change Order Status
        [HttpPut("ChangeStatus")]
        [SwaggerOperation(Summary = "Changes the status of a specific order.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Status changed successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        [Permission(Permissions.Orders.Edit)]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            try
            {
                var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { message = "الطلب غير موجود." });

                await _unit.OrderRepository.UpdateOrderStatusAsync(id, status);
                return Ok(new { message = "تم تغير حالة الطلب" });
            }
            catch
            {
                return StatusCode(500, "خطأ في تغيير حالة الطلب.");
            }
        }
        #endregion

        #region Edit Order 
        [HttpPut("Edit/{id}")]
        [SwaggerOperation(Summary = "Edits a specific order.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order edited successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid order data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        [Permission(Permissions.Orders.Edit)]
        public async Task<IActionResult> Edit(int id,OrderDTO orderDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userClaims = ReturnUser(HttpContext);
                    var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.FindByIdAsync(userId);
                    var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
                    if (order == null)
                        return NotFound(new { message = "الطلب غير موجود."});

                    orderDto.ShippingCost = await _unit.OrderRepository.CalculateShippingPrice(orderDto,_CityUnit,_WeightSettingUnit,_SpecialCityPriceUnit);
                    await _unit.OrderRepository.EditOrderAsync(id, orderDto);
                    return Ok(new { message = " تم التعديل"});
                }
                catch
                {
                    return StatusCode(500, "خطأ في تعديل الطلب.");
                }
            }
            return BadRequest(ModelState);
        }

        #endregion

       #region Add Orders
        [HttpPost("Add")]
        [SwaggerOperation(Summary = "Adds a new order.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Order created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to add order.")]
        [Permission(Permissions.Orders.Create)]
        public async Task<IActionResult> Add(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderDTO.OrderProducts == null || orderDTO.OrderProducts.Count == 0)
            {
                ModelState.AddModelError("", "يجب عليك اضافه منتاجات");
                return BadRequest(ModelState);
            }

            try
            {
                var user = ReturnUser(HttpContext);
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var merchant = await _unit.MerchantRepository.GetMerchantByIdAsync(userId);
                int merchantId = merchant.Id;
                orderDTO.MerchantId = merchantId;

                orderDTO.ShippingCost = await _unit.OrderRepository.CalculateShippingPrice(orderDTO, _CityUnit, _WeightSettingUnit, _SpecialCityPriceUnit);

                var addedOrder = await _unit.OrderRepository.AddOrderAsync(orderDTO, userId);
                if (addedOrder == null)
                {
                    return BadRequest(new { message =  "فشل في إضافة الطلب."});
                }

                var orderDTOResult = _mapper.Map<OrderDTO>(addedOrder);
                return CreatedAtAction(nameof(Index), new { id = orderDTOResult.Id }, orderDTOResult);
            }
            catch (Exception ex)
            {

           
                return StatusCode(500, "خطأ في إضافة الطلب.");
            }
        }

    

    #endregion

       #region Delete Orders
    [HttpDelete("Delete")]
        [SwaggerOperation(Summary = "Deletes a specific order.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        [Permission(Permissions.Orders.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { message =  "الطلب غير موجود."});

                await _unit.OrderRepository.DeleteOrderAsync(id);
                return Ok(new { message = "تم الحذف "});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

       #region GetCitiesByGovernment

        [HttpGet("GetCitiesByGovernment")]
        [SwaggerOperation(Summary = "Retrieves cities based on government ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of cities.")]
        [Permission(Permissions.Cities.View)]
        public IActionResult GetCitiesByGovernment(int governmentId)
        {
            try
            {
                var cities = _unit.CityRepository.GetAllByGovernmentId(governmentId);
                var cityDTOs = _mapper.Map<List<CityDTO>>(cities);
                return Ok(cityDTOs);
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب المدن حسب المحافظه.");
            }
        }
        #endregion

       #region GetBranchesByGovernment
        [HttpGet("GetBranchesByGovernment")]
        [SwaggerOperation(Summary = "Retrieves branches based on government name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of branches.")]
        [Permission(Permissions.Branches.View)]
        public async Task<IActionResult> GetBranchesByGovernmentAsync(int government)
        {
            try
            {
                var branches =  await _unit.BranchRepository.GetBranchesByGovernmentNameAsync(government);
                var branchesDTO = _mapper.Map<List<BranchDTO>>(branches);
                return Ok(branchesDTO);
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب الفروع حسب المحافظه.");
            }
        }
        #endregion

       #region Get Order Count
        [HttpGet("OrderCount")]
        [SwaggerOperation(Summary = "Retrieves the count of orders based on user role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of orders.")]
        [Permission("anyUser")]
        public async Task<IActionResult> OrderCount()
        {
            try
            {
                var user = ReturnUser(HttpContext);
                var roleName = user.FindFirstValue(ClaimTypes.Role);
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var orders = await _unit.OrderRepository.GetAllOrdersAsync();

                if (roleName == "Admin" || roleName == "الموظفين")
                    return Ok(orders);
                else if (roleName == "التجار")
                {
                    var merchantId = _myContext.Merchants.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
                    var filteredOrders = orders.Where(o => o.MerchantId == merchantId).ToList();
                    return Ok(filteredOrders);
                }
                else if (roleName == "المناديب")
                {
                    var deliveryId = _myContext.Deliveries.Where(d => d.UserId == userId).Select(d => d.Id).FirstOrDefault();
                    var filteredOrders = orders.Where(o => o.DeliveryId == deliveryId).ToList();
                    return Ok(filteredOrders);
                }
                else
                {
                    return Forbid();
                }
            }
            catch
            {
                return StatusCode(500, "خطأ في جلب عدد الطلبات حسب دور المستخدم.");
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


        #region Get Orders After Filter
        [HttpGet("IndexAfterFilter")]
        [SwaggerOperation(Summary = "Retrieves orders based on status and user role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of filtered orders.")]
        [Permission(Permissions.Orders.View)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> IndexAfterFilter(string query)
        {
            try
            {
                var user = ReturnUser(HttpContext);

                var roleName = user.FindFirstValue(ClaimTypes.Role);
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var orders = await _unit.OrderRepository.GetAllOrdersAsync();

                var ordersData = orders.Where(o => o.OrderStatus == query).ToList();

                if (roleName == "Admin" || roleName == "الموظفين")
                {
                    var filteredOrders = _mapper.Map<List<OrderDTO>>(ordersData);
                    return Ok(filteredOrders);
                }
                else if (roleName == "التجار")
                {
                    var merchantId = _myContext.Merchants.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
                    ordersData = ordersData.Where(o => o.MerchantId == merchantId).ToList();
                    var filteredOrders = _mapper.Map<List<OrderDTO>>(ordersData);
                    return Ok(filteredOrders);
                }
                else if (roleName == "المناديب")
                {
                    var deliveryId = _myContext.Deliveries.Where(d => d.UserId == userId).Select(d => d.Id).FirstOrDefault();
                    ordersData = ordersData.Where(o => o.DeliveryId == deliveryId).ToList();
                    var filteredOrders = _mapper.Map<List<OrderDTO>>(ordersData);
                    return Ok(filteredOrders);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "خطأ في جلب الطلبات بعد الفلترة.");
            }
        }

        #endregion
    }
}
//GetBranchesByGovernment /