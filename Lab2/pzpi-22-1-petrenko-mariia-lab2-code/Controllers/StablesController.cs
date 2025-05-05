using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StablesController : ControllerBase
    {
        private readonly IStableRepository stableRepository;
        private readonly IFarmRepository farmRepository;
        public StablesController(IStableRepository stableRepository, IFarmRepository farmRepository)
        {
            this.stableRepository = stableRepository;
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            List<Stable> stableDomain = new();
            if (role == nameof(UserRole.DatabaseAdmin))
            {
                stableDomain = await stableRepository.GetAllAsync();
            }

            else if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                stableDomain = farms.SelectMany(f => f.Stables).ToList();
            }

            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                if (farm != null)
                {
                    stableDomain = farm.Stables.ToList();
                }
            }

            var stableDto = stableDomain.Select(a => a.ToStableDto());

            return Ok(stableDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var stableDomain = await stableRepository.GetByIdAsync(id);
            if (stableDomain == null)
            {
                return NotFound();
            }

            if (userRole == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                if (stableDomain.Farm.OwnerId != ownerId)
                {
                    return Forbid("You do not have access to this stable.");
                }
            }

            if (userRole == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                if (stableDomain.Farm.Administrators.All(a => a.Id != adminId))
                {
                    return Forbid("You do not have access to this stable.");
                }
            }
            return Ok(stableDomain.ToStableDto());
        }

        [HttpPost("{farmId}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid farmId, [FromBody] AddStableRequestDto addStableRequestDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var farm = await farmRepository.GetByIdAsync(farmId);
            if (farm == null)
            {
                return NotFound("Farm not found.");
            }

            var userIdGuid = Guid.Parse(userId);
            if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userIdGuid)
            {
                return Forbid("You do not have permission to add stables to this farm.");
            }

            if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userIdGuid))
            {
                return Forbid("You do not have permission to add stables to this farm.");
            }

            var stableDomain = addStableRequestDto.ToStableFromCreate(farmId);
            stableDomain = await stableRepository.CreateAsync(stableDomain);
            var stableDto = stableDomain.ToStableDto();

            return CreatedAtAction(nameof(GetById), new { id = stableDomain.Id }, stableDto);
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStableRequestDto updateStableRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var farm = await farmRepository.GetByIdAsync(updateStableRequestDto.FarmId);
            if (farm == null)
            {
                return NotFound($"Farm with ID {updateStableRequestDto.FarmId} not found.");
            }

            var userIdGuid = Guid.Parse(userId);
            if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userIdGuid)
            {
                return Forbid("You do not have permission to edit stables to this farm.");
            }

            if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userIdGuid))
            {
                return Forbid("You do not have permission to edit stables to this farm.");
            }

            var stableDomain = await stableRepository.UpdateAsync(id, updateStableRequestDto.ToStableFromUpdate());
            if (stableDomain == null)
            {
                return NotFound("Stable not found");
            }
                
            return Ok(stableDomain.ToStableDto());

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stableDomain = await stableRepository.DeleteAsync(id);
            if (stableDomain == null)
            {
                return NotFound("Stable does not exist");
            }

            return Ok(stableDomain.ToStableDto());
        }
    }
}
