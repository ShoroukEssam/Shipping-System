using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.Constants;
using Shipping.DTO.CityDTO;
using Shipping.Models;
using Shipping.Repository.CityRepo;
using Shipping.Repository.GovernmentRepo;
using Shipping.UnitOfWork;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork<City> _unit;
        private readonly IMapper _mapper;
        public CityController(IUnitOfWork<City> unit ,IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region GetCitiesByGovernment
        [HttpGet("government/{governmentId}")]
        [Permission(Permissions.Cities.View)]
        [SwaggerOperation(Summary = "Gets cities by government ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of cities.")]
        public ActionResult<IEnumerable<City>> GetCitiesByGovernment(int governmentId)
        {
            var cities = _unit.CityRepository.GetAllByGovernmentId(governmentId);
            var cityList = _mapper.Map<List<CityDTO>>(cities);
            return Ok(cityList);
        }
        #endregion

        #region AddCity

        [HttpPost]
        [Permission(Permissions.Cities.Create)]
        [SwaggerOperation(Summary = "Adds a new city.")]
        [SwaggerResponse(StatusCodes.Status201Created, "City successfully created.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data. Please check the provided information.")]
        public async Task<ActionResult> AddCity([FromBody] CityDTO cityDTO)
        {
            if (ModelState.IsValid)
            {
                var city = _mapper.Map<City>(cityDTO);
                _unit.CityRepository.AddToGovernment(city.GovernmentId, city);

                var newcity = _mapper.Map<CityDTO>(city);
                return CreatedAtAction(nameof(GetCityById), new { id = newcity.id }, newcity);
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region ChangeStatus
        [HttpPut("change-status/{id}")]
        [Permission(Permissions.Cities.Edit)]
        [SwaggerOperation(Summary = "Changes the status of a city.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "City status successfully updated.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "City not found.")]
        public ActionResult ChangeStatus(int id, [FromQuery] bool status)
        {
            var city = _unit.CityRepository.GetById(id);
            if (city == null)
            {
                return NotFound("المدينه غير موجوده");
            }
            city.Status = status;
            _unit.CityRepository.Update(id, city);
            return Ok(new { Msg = "تم تعديل الحاله بنجاح" });
        }
        #endregion

        #region EditCity
        [HttpPut("edit/{id}")]
        //   [Authorize(Permissions.Cities.Edit)]
        [SwaggerOperation(Summary = "Edits an existing city.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "City successfully updated.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The ID in the URL does not match the ID in the body.")]
        public ActionResult EditCity(int id, [FromBody] CityDTO cityDTO)
        {
            if (id != cityDTO.id)
            {

                return BadRequest("برجاء ادخال بيانات صحيحه الرقم التعريفي غير متتطابق");
            }

            var city = _mapper.Map<City>(cityDTO);
            _unit.CityRepository.Update(id, city);
            return Ok(new { Msg = "تم تعديل المدينه بنجاح" });
        }
        #endregion

        #region SearchCities
        [HttpGet("search")]
        [Permission(Permissions.Cities.View)]
        [SwaggerOperation(Summary = "Searches for cities by name within a specific government.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of cities matching the search criteria.")]
        public ActionResult<IEnumerable<City>> SearchCities([FromQuery] int governmentId, [FromQuery] string query)
        {
            List<City> cities;
            if (string.IsNullOrWhiteSpace(query))
            {
                cities = _unit.CityRepository.GetAllByGovernmentId(governmentId).ToList();
            }
            else
            {
                cities = _unit.CityRepository.GetAllByGovernmentId(governmentId)
                    .Where(i => i.Name.ToUpper().Contains(query.ToUpper())).ToList();
            }
            var cityList = _mapper.Map<List<CityDTO>>(cities);
            return Ok(cityList);
        }
        #endregion

        #region DeleteCity
        [HttpDelete("delete/{id}")]
        [Permission(Permissions.Cities.Delete)]
        [SwaggerOperation(Summary = "Deletes a city by marking it as deleted.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "City successfully marked as deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "City not found.")]
        public ActionResult DeleteCity(int id)
        {
            var city = _unit.CityRepository.GetById(id);
            if (city == null)
            {
                return NotFound("المدينه غير موجوده");
            }
            city.IsDeleted = true;
            _unit.CityRepository.Update(id, city);
            return Ok(new { Msg = "تم الحذف  بنجاح" });
        }
        #endregion

        #region GetCityById
        [HttpGet("{id}")]
        [Permission(Permissions.Cities.View)]
        [SwaggerOperation(Summary = "Gets a city by ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the city object.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "City not found.")]
        public ActionResult<CityDTO> GetCityById(int id)
        {
            var city = _unit.CityRepository.GetById(id);
            if (city == null)
            {
                return NotFound("المدينه غير موجوده");
            }
            var cityDTO = _mapper.Map<CityDTO>(city);
            return Ok(cityDTO);
        }
        #endregion
    }
}
