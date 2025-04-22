using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.Constants;
using Shipping.DTO.MerchantDTOs;
using Shipping.Models;
using Shipping.UnitOfWork;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IUnitOfWork<Merchant> _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public MerchantController(IUnitOfWork<Merchant> unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        #region GetAllMerchants

        [HttpGet]
        [SwaggerOperation(Summary = "Get all merchants")]
        [SwaggerResponse(200, "Returns the list of all merchants", typeof(List<MerchantDTO>))]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.View)]
        public async Task<ActionResult<List<MerchantDTO>>> GetAllMerchants()
        {
            try
            {
                var merchants = await _unitOfWork.MerchantRepository.GetAllMerchants();
                var merchantDTOs = _mapper.Map<List<MerchantDTO>>(merchants);
                return Ok(merchantDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
#endregion

        #region  AddMerchant
        [HttpPost("AddMerchant")]
        [SwaggerOperation(Summary = "Add a new merchant")]
        [SwaggerResponse(200, "Merchant added successfully", typeof(MerchantDTO))]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.Create)]
        public async Task<IActionResult> AddMerchant(MerchantDTO newMerchant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var merchant = await _unitOfWork.MerchantRepository.Add(newMerchant, _userManager);
                    _unitOfWork.SaveChanges();
                    var merchantDTO = _mapper.Map<MerchantDTO>(merchant);
                    return Ok(merchantDTO);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            return BadRequest("Invalid data");
        }
#endregion

        #region GetMerchantById
        [HttpGet("GetMerchantById/{id}")]
        [SwaggerOperation(Summary = "Get a merchant by ID")]
        [SwaggerResponse(200, "Returns the merchant", typeof(MerchantDTO))]
        [SwaggerResponse(404, "Merchant not found")]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.View)]

        public async Task<IActionResult> GetMerchantById(string id)
        {
            try
            {
                var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
                if (merchant == null)
                {
                    return NotFound("Merchant not found");
                }
                var merchantDTO = _mapper.Map<MerchantDTO>(merchant);
                return Ok(merchantDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
#endregion

        #region  UpdateMerchant
        [HttpPut("UpdateMerchant")]
        [SwaggerOperation(Summary = "Update a merchant's details")]
        [SwaggerResponse(200, "Merchant updated successfully", typeof(MerchantDTO))]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.Edit)]

        public async Task<IActionResult> UpdateMerchant(MerchantDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var merchant = await _unitOfWork.MerchantRepository.Update(newData, _userManager);
                _unitOfWork.SaveChanges();
                var updatedMerchantDTO = _mapper.Map<MerchantDTO>(merchant);
                return Ok(updatedMerchantDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       #endregion

        #region UpdateStatus for merment
        [HttpPut("UpdateStatus")]
        [SwaggerOperation(Summary = "Update a merchant's status")]
        [SwaggerResponse(200, "Merchant status updated successfully", typeof(MerchantDTO))]
        [SwaggerResponse(404, "Merchant not found")]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.Edit)]

        public async Task<IActionResult> UpdateStatus(string id, bool status)
        {
            try
            {
                var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
                if (merchant == null)
                {
                    return NotFound("Merchant not found");
                }

                var updatedMerchant = await _unitOfWork.MerchantRepository.UpdateStatus(merchant, status);
                _unitOfWork.SaveChanges();
                var merchantDTO = _mapper.Map<MerchantDTO>(updatedMerchant);
                return Ok(merchantDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
#endregion

        #region DeleteMerchant
        [HttpDelete("DeleteMerchant")]
        [SwaggerOperation(Summary = "Delete a merchant by ID")]
        [SwaggerResponse(200, "Merchant deleted successfully")]
        [SwaggerResponse(404, "Merchant not found")]
        [SwaggerResponse(500, "Internal server error")]
        [Permission(Permissions.Merchants.Delete)]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            try
            {
                var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
                if (merchant == null)
                {
                    return NotFound("Merchant not found");
                }

                await _unitOfWork.MerchantRepository.SoftDeleteAsync(merchant);
                _unitOfWork.SaveChanges();
                return Ok("Merchant deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
