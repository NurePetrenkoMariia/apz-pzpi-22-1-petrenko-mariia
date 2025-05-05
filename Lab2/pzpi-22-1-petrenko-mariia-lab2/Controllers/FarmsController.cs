using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmRepository farmRepository;
        private readonly IUserRepository userRepository;
        public FarmsController(IFarmRepository farmRepository, IUserRepository userRepository)
        {
            this.farmRepository = farmRepository;
            this.userRepository = userRepository;
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

            List<Farm> farmDomain = new();

            if (role == nameof(UserRole.DatabaseAdmin))
            {
                farmDomain = await farmRepository.GetAllAsync();
            }

            if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                farmDomain = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
            }

            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
           
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                if (farm != null)
                {
                    farmDomain.Add(farm);
                }
            }

            var farmDto = farmDomain.Select(f => f.ToFarmDto());
            return Ok(farmDto);
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

            var farmDomain = await farmRepository.GetByIdAsync(id);
            if (farmDomain == null)
            {
                return NotFound();
            }

            if (userRole == "Owner" && farmDomain.OwnerId != Guid.Parse(userId))
            {
                return Forbid("You do not have access to this farm.");
            }

            if (userRole == "Admin" )
            {
                var adminId = Guid.Parse(userId);
                bool isAdminOfThisFarm = farmDomain.Administrators.Any(a => a.Id == adminId);

                if (!isAdminOfThisFarm)
                {
                    return Forbid("You do not have access to this farm.");
                }
            }

            return Ok(farmDomain.ToFarmDto());
        }

        [HttpPost("{ownerId}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid ownerId, [FromBody] AddFarmRequestDto addFarmRequestDto)
        {
            var user = await userRepository.GetByIdAsync(ownerId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var farmDomain = addFarmRequestDto.ToFarmFromCreate(ownerId);

            farmDomain = await farmRepository.CreateAsync(farmDomain);

            var farmDto = farmDomain.ToFarmDto();
            return CreatedAtAction(nameof(GetById), new { id = farmDomain.Id }, farmDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFarmRequestDto updateFarmRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var user = await userRepository.GetByIdAsync(updateFarmRequestDto.OwnerId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var farmDomain = await farmRepository.GetByIdAsync(id);
            if (farmDomain == null)
            {
                return NotFound("Farm not found");
            }

            if (farmDomain.OwnerId != Guid.Parse(userId))
            {
                return Forbid("You do not have access to update this farm.");
            }

            farmDomain = await farmRepository.UpdateAsync(id, updateFarmRequestDto.ToFarmFromUpdate());
            return Ok(farmDomain.ToFarmDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var farmDomain = await farmRepository.GetByIdAsync(id);

            if (farmDomain == null)
            {
                return NotFound("Farm does not exist");
            }

            if (farmDomain.OwnerId != Guid.Parse(userId))
            {
                return Forbid("You do not have access to delete this farm.");
            }

            farmDomain = await farmRepository.DeleteAsync(id);
            return Ok(farmDomain.ToFarmDto());
        }
    }
}
