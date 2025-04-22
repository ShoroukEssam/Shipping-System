using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.DTO.AccountDTOs;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper;
using Shipping.UnitOfWork;
using Shipping.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Shipping.Constants;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightSettingController : ControllerBase
    {

        private readonly IUnitOfWork<WeightSetting> _WeightSettingUnit;
        private readonly IMapper _Mapper;

        public WeightSettingController(IUnitOfWork<WeightSetting> Unit, IMapper Mapper)
        {
            _Mapper = Mapper;
            _WeightSettingUnit = Unit;
        }

        #region Get Weight Setting
        [HttpGet]
        [Permission(Permissions.WeightSettings.View)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "GetWeightSetting")]
        [SwaggerResponse(StatusCodes.Status200OK,"when get Weight Settings Successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound,"when can't find Weight Settings")]
        public async Task<IActionResult> GetWeightSetting()
        {
            WeightSetting ws = await _WeightSettingUnit.Repository.GetByIdAsync(1);
            if (ws == null) return NotFound();
             WeightSettingDTO wsDTO =_Mapper.Map<WeightSettingDTO>(ws);
            return Ok(wsDTO);
        }
        #endregion


        #region Edit Weight Setting
        [HttpPut]
        [Permission(Permissions.WeightSettings.Edit)]
        [Permission(Permissions.WeightSettings.View)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "GetWeightSetting")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "when  Weight Settings had been updated Successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "when can't update Weight Settings")]
        public async Task<IActionResult> PutWeightSetting(WeightSettingDTO wsDTO)
        {
            WeightSetting ws = _Mapper.Map<WeightSetting>(wsDTO);
            try
            {
                await _WeightSettingUnit.Repository.UpdateAsync(ws);
                 _WeightSettingUnit.SaveChanges();
            }
            catch(Exception ex) 
            {
                return BadRequest(new { message=ex.Message});
            }
           
            return NoContent();
        }
        #endregion

    }
}
