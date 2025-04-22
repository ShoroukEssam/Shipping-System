using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Constants;
using Shipping.DTO.BranchDTOs;
using Shipping.Models;
using Shipping.Repository.BranchRepository;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchController(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }
        #region GetAllBranches
        /// <summary>
        /// Gets all branches.
        /// </summary>
        /// <returns>A list of branches.</returns>
        [HttpGet]
        [Permission(Permissions.Branches.View)]
        [SwaggerOperation(Summary = "Gets all branches", Description = "Retrieves a list of all branches")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of branches retrieved successfully", typeof(List<BranchDTO>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
        public async Task<ActionResult<List<BranchDTO>>> GetAllBranches()
        {
            var branches = await _branchRepository.GetAllAsync();
            var branchDTOs = _mapper.Map<List<BranchDTO>>(branches);
            return Ok(branchDTOs);
        }

        #endregion

        #region Gets a branch by ID
        /// <summary>
        /// Gets a branch by ID.
        /// </summary>
        /// <param name="id">The ID of the branch.</param>
        /// <returns>A branch.</returns>
        [HttpGet("{id}")]
        [Permission(Permissions.Branches.View)]
        [SwaggerOperation(Summary = "Gets a branch by ID", Description = "Retrieves a branch by its ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Branch retrieved successfully", typeof(BranchDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Branch not found")]
        public async Task<ActionResult<BranchDTO>> GetBranchById(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                return NotFound(new { message = "الفرع غير موجود" });
            }

            var branchDTO = _mapper.Map<BranchDTO>(branch);
            return Ok(branchDTO);
        }
        #endregion

        #region Adds a new branch
        /// <summary>
        /// Adds a new branch.
        /// </summary>
        /// <param name="branchDTO">The branch DTO.</param>
        /// <returns>The created branch.</returns>
        [HttpPost]
        [Permission(Permissions.Branches.Create)]
        [SwaggerOperation(Summary = "Adds a new branch", Description = "Creates a new branch")]
        [SwaggerResponse(StatusCodes.Status201Created, "Branch created successfully", typeof(BranchDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
        public async Task<ActionResult<BranchDTO>> AddBranch(BranchDTO branchDTO)
        {
            var branch = _mapper.Map<Branch>(branchDTO);

            await _branchRepository.AddAsync(branch);

            var createdDto = _mapper.Map<BranchDTO>(branch);
            return CreatedAtAction(nameof(GetBranchById), new { id = createdDto.Id }, new { message = $"تمت إضافة الفرع بنجاح. معرف الفرع الجديد: {createdDto.Id}"});
        }
        #endregion

        #region Updates an existing branch
        /// <summary>
        /// Updates an existing branch.
        /// </summary>
        /// <param name="id">The ID of the branch.</param>
        /// <param name="branchDTO">The branch DTO.</param>
        /// <returns>The updated branch.</returns>
        [HttpPut("{id}")]
        [Permission(Permissions.Branches.Edit)]
        [SwaggerOperation(Summary = "Updates an existing branch", Description = "Updates a branch by its ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Branch updated successfully", typeof(BranchDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or ID mismatch")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Branch not found")]
        public async Task<IActionResult> UpdateBranch(int id, BranchDTO branchDTO)
        {
            if (id != branchDTO.Id)
            {
                return BadRequest("معرف الفرع المحدث غير متطابق مع معرف الفرع في الطلب.");
            }

            var existingBranch = await _branchRepository.GetByIdAsync(id);
            if (existingBranch == null)
            {
                return NotFound("الفرع غير موجود");
            }

            try
            {
                var branch = _mapper.Map(branchDTO, existingBranch);
                await _branchRepository.UpdateAsync(branch);
                return Ok(_mapper.Map<BranchDTO>(branch));
            }
            catch (Exception ex)
            {
                return BadRequest($"فشلت العملية: {ex.Message}");
            }
        }
        #endregion

        #region Deletes a branch by ID
        /// <summary>
        /// Deletes a branch by ID.
        /// </summary>
        /// <param name="id">The ID of the branch.</param>
        /// <returns>A status message.</returns>
        [HttpDelete("{id}")]
        [Permission(Permissions.Branches.Delete)]
        [SwaggerOperation(Summary = "Deletes a branch by ID", Description = "Deletes a branch by its ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Branch deleted successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Branch not found")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
            {
                return NotFound(new { message = "الفرع غير موجود" });
            }

            try
            {
                await _branchRepository.DeleteAsync(id);
                return Ok(new { message = "تم حذف الفرع بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest($"فشلت العملية: {ex.Message}");
            }
        }
        #endregion

        #region Searches for branches by query
        /// <summary>
        /// Searches for branches by query.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of matching branches.</returns>
        [HttpGet("search")]
        [Permission(Permissions.Branches.View)]
        [SwaggerOperation(Summary = "Searches for branches by query", Description = "Searches for branches based on a query string")]
        [SwaggerResponse(StatusCodes.Status200OK, "Branches retrieved successfully", typeof(List<BranchDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search query")]
        public async Task<ActionResult<List<BranchDTO>>> SearchBranches(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "يرجى إدخال نص للبحث." });
            }

            var branches = await _branchRepository.SearchAsync(query);
            var branchDTOs = _mapper.Map<List<BranchDTO>>(branches);

            return Ok(branchDTOs);
        }
        #endregion
    }
}
