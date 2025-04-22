// DeliveryController.cs

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Shipping.Constants;
using NuGet.Protocol.Plugins;
using Shipping.DTO.AccountDTOs;
using Shipping.DTO.DeliveryDTOs;
using Shipping.Models;
using Shipping.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using swagger = Swashbuckle.AspNetCore.Annotations;
namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IUnitOfWork<Delivery> unitOfWork;
        private readonly IMapper mapper;

        public DeliveryController(IUnitOfWork<Delivery> unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [Permission(Permissions.Deliveries.View)]
        [swagger.SwaggerOperation(Summary = "Show all Deliveries." , Description = "Retrieves a list of all Deliveries")]
        [swagger.SwaggerResponse(StatusCodes.Status201Created, "Deliveries successfully Retrieved.")]
        [swagger.SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            try
            {
                var deliveries = await unitOfWork.DeliveryRepository.GetAllDeliveries();
                var deliveryDTOs = mapper.Map<List<DeliveryDTO>>(deliveries);
                return Ok(deliveryDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve deliveries: {ex.Message}");
            }
        }


        // POST: api/Delivery/AddDelivery
        [HttpPost("AddDelivery")]
        [Permission(Permissions.Deliveries.Create)]
        [swagger.SwaggerOperation(Summary = "Add New Delivery.", Description ="Add New Delivery on Delivery List")]
        [swagger.SwaggerResponse(StatusCodes.Status201Created, "Delivery successfully Created.")]
        [swagger.SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]

        public async Task<IActionResult> Add(DeliveryDTO newDeliveryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                //var newDelivery = mapper.Map<Delivery>(newDeliveryDto);
                var addedDelivery = await unitOfWork.DeliveryRepository.Insert(newDeliveryDto);
                var addedDeliveryDto = mapper.Map<DeliveryDTO>(addedDelivery);

                return Ok(addedDeliveryDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add delivery: {ex.Message}");
            }
        }

        [HttpPut("EditDelivery/{id}")]
        [Permission(Permissions.Deliveries.Edit)]
        [swagger.SwaggerOperation(Summary = "Edit Existing Delivery." ,  Description = "Edit an Existing Delivery data")]
        [swagger.SwaggerResponse(StatusCodes.Status201Created, "Delivery successfully Updated.")]
        [swagger.SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]

        public async Task<IActionResult> EditDelivery(string id, DeliveryDTO updatedDeliveryDto)
        {
            var existingDelivery = await unitOfWork.DeliveryRepository.GetById(id);
            if (existingDelivery == null)
            {
                return NotFound("Delivery not found.");
            }
            var originalId = existingDelivery.Id;

            mapper.Map(updatedDeliveryDto, existingDelivery);
            existingDelivery.Id = originalId;

            var editedDelivery = await unitOfWork.DeliveryRepository.EditDelivery(id, existingDelivery);
            var editedDeliveryDto = mapper.Map<DeliveryDTO>(editedDelivery);

            return Ok(editedDeliveryDto);
        }


        [HttpPut("ChangeStatus/{id}")]
        [Permission(Permissions.Deliveries.Edit)]
        [swagger.SwaggerOperation(Summary = "Update Delivery Status." , Description = "Edit the status of an existing Delivery")]
        [swagger.SwaggerResponse(StatusCodes.Status201Created, "Status successfully Retrieved.")]
        [swagger.SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]

        public async Task<IActionResult> ChangeDeliveryStatus(string id, bool status)
        {
            var delivery = await unitOfWork.DeliveryRepository.GetById(id);
            if (delivery == null)
            {
                return NotFound("Delivery not found.");
            }

            unitOfWork.DeliveryRepository.UpdateStatus(delivery, status);
            return NoContent();
        }

        [HttpDelete("DeleteDelivery/{id}")]
        [Permission(Permissions.Deliveries.Delete)]
        [swagger.SwaggerOperation(Summary = "Delete Delivery." ,  Description = "Delete an existing Delivery")]
        [swagger.SwaggerResponse(StatusCodes.Status201Created, "Delivery successfully Deleted.")]
        [swagger.SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]
        public async Task<IActionResult> SoftDeleteDelivery(string id)
        {
            try
            {
                var delivery = await unitOfWork.DeliveryRepository.GetById(id);
                if (delivery != null)
                {
                    await unitOfWork.DeliveryRepository.SoftDeleteAsync(delivery);
                    unitOfWork.SaveChanges();
                    return Ok(new { Message = "Delivery deleted successfully."});
             
                }
                return NotFound("Delivery not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Get Delivery By Id

        [HttpGet("{id}")]
        [Permission(Permissions.Deliveries.View)]

        public async Task<IActionResult> GetDeliveryById(string id)
        {
            try
            {
                var delivery = await unitOfWork.DeliveryRepository.GetById(id);
                if (delivery == null)
                {
                    return NotFound($"Delivery with id {id} not found");
                }
                var deliveryDTO = mapper.Map<DeliveryDTO>(delivery);
                return Ok(deliveryDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve delivery: {ex.Message}");
            }
        }
        #endregion

    }
}
