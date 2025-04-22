using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Repository.GovernmentRepo;
using Shipping.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;
using Shipping.DTO.CityDTO;
using Shipping.DTO.GovernmentDTO;
using Microsoft.VisualBasic;
using Shipping.Constants;
using Shipping.CustomAuth;
using System.Security.Claims;

namespace Shipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GovernmentController : ControllerBase
    {
        private readonly IUnitOfWork<Government> _unit;
        private readonly IMapper _mapper;
        public GovernmentController(IUnitOfWork<Government> unit ,IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region GetAllGovernments
        [HttpGet]     
        [Permission(Permissions.Governments.View)]
        [SwaggerOperation(Summary = "Gets all governments.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of governments.")]
        public ActionResult<IEnumerable<Government>> GetAllGovernments()
        {
            var governments = _unit.GovernmentRepository.GetAll();
            var governmentsList = _mapper.Map<List<GovernmentDTO>>(governments);

            return Ok(governmentsList);
        }
        #endregion

        #region AddGovernment
        [HttpPost("add")]
        [Permission(Permissions.Governments.Create)]
        [SwaggerOperation(Summary = "Adds a new government.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Government successfully created.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]
        public async Task<ActionResult> AddGovernment([FromBody] GovernmentDTO governmentDTO)
        {
            if (ModelState.IsValid)
            {
                var government = _mapper.Map<Government>(governmentDTO);
                _unit.GovernmentRepository.Add(government);
                var newgovernment = _mapper.Map<GovernmentDTO>(government);
                return CreatedAtAction(nameof(GetGovernmentById), new { id = newgovernment.Id }, newgovernment);
            }

            return BadRequest("برجاء ادخال بيانات صحيحه");
        }
        #endregion

        #region EditGovernment
        [HttpPut("edit/{id}")]
        //[Authorize(Permissions.Governments.Edit)]
        [SwaggerOperation(Summary = "Edits an existing government.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Government successfully updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The ID in the URL does not match the ID in the body.")]
        public IActionResult EditGovernment(int id, [FromBody] GovernmentDTO governmentDTO)

        {
            if (id != governmentDTO.Id)
            {
                return BadRequest("برجاء ادخال بيانات صحيحه الرقم التعريفي غير متتطابق");
            }
            var government = _mapper.Map<Government>(governmentDTO);
            _unit.GovernmentRepository.Update(id, government);
    
            return Ok(new{ Msg = "تم تعديل المحافظه بنجاح" });
        }
        #endregion

        #region ChangeGovernmentStatus
        [HttpPut("change-status/{id}")]
        //[Authorize(Permissions.Governments.Edit)]
        [SwaggerOperation(Summary = "Changes the status of a government.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Government status successfully updated.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Government not found.")]
        public IActionResult ChangeGovernmentStatus(int id, [FromQuery] bool status)
        {
            var government = _unit.GovernmentRepository.GetById(id);
            if (government == null)
            {
                return NotFound("المحافظه غير موجوده");
            }

            _unit.GovernmentRepository.UpdateStatus(government, status);
            return Ok(new { Msg = "تم تعديل الحاله بنجاح" });
        }
        #endregion

        #region SearchGovernments
        [HttpGet("search")]
        //[Authorize(Permissions.Governments.View)]
        [SwaggerOperation(Summary = "Searches for governments by name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of governments matching the search criteria.")]
        public ActionResult<IEnumerable<Government>> SearchGovernments([FromQuery] string query)
        {
            List<Government> governments;
            if (string.IsNullOrWhiteSpace(query))
            {
                governments = _unit.GovernmentRepository.GetAll().ToList();
            }
            else
            {
                governments = _unit.GovernmentRepository.GetAll().Where(g => g.Name.Contains(query)).ToList();
            }
            var governmentsList = _mapper.Map<List<GovernmentDTO>>(governments);
            return Ok(governmentsList);
        }
        #endregion

        #region DeleteGovernment
        [HttpDelete("delete/{id}")]
        //[Authorize(Permissions.Governments.Delete)]
        [SwaggerOperation(Summary = "Deletes a government by marking it as deleted.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Government successfully marked as deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Government not found.")]
        public IActionResult DeleteGovernment(int id)
        {
            var government = _unit.GovernmentRepository.GetById(id);
            if (government == null)
            {
                return NotFound("المحافظه غير موجوده");
            }

            government.IsDeleted = true;
            _unit.GovernmentRepository.Update(id, government);
            return Ok(new { Msg = "تم الحذف  بنجاح" });
        }
        #endregion

        #region GetGovernmentById
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a government by ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the government object.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Government not found.")]
        public ActionResult<GovernmentDTO> GetGovernmentById(int id)
        {
            var government = _unit.GovernmentRepository.GetById(id);
            if (government == null)
            {
                return NotFound("المحافظه غير موجوده ");
            }
            var governmentDTO = _mapper.Map<GovernmentDTO>(government);
            return Ok(governmentDTO);

        }
        #endregion
    }
}
