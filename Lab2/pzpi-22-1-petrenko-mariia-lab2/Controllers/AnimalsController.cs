using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IStableRepository stableRepository;
        private readonly IFarmRepository farmRepository;
        public AnimalsController(IAnimalRepository animalRepository, IStableRepository stableRepository, IFarmRepository farmRepository)
        {
            this.animalRepository = animalRepository;
            this.stableRepository = stableRepository;
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            List<Animal> animalDomain = new();

            if (userRole == nameof(UserRole.DatabaseAdmin))
            {
                animalDomain = await animalRepository.GetAllAsync();
            }
            else
            {
                var userIdGuid = Guid.Parse(userId);
                if (userRole == nameof(UserRole.Owner))
                {
                    animalDomain = await animalRepository.GetByOwnerIdAsync(userIdGuid);
                }
                else if (userRole == nameof(UserRole.Admin))
                {
                    var farm = await farmRepository.GetFarmByAdminIdAsync(userIdGuid);
                    if (farm != null)
                    {
                        animalDomain = await animalRepository.GetByFarmIdAsync(farm.Id);
                    }
                }
            }
            var animalDto = animalDomain.Select(a => a.ToAnimalDto());

            return Ok(animalDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            var animalDomain = await animalRepository.GetByIdAsync(id);
            if (animalDomain == null)
            {
                return NotFound();
            }
            if (userRole == nameof(UserRole.DatabaseAdmin))
            {
                return Ok(animalDomain.ToAnimalDto());
            }
            var farm = animalDomain.Stable.Farm;
            var userGuid = Guid.Parse(userId);

            if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
            {
                return Forbid("You do not have access to this farm.");
            }
            if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
            {
                return Forbid("You do not have access to this farm.");
            }

            return Ok(animalDomain.ToAnimalDto());

        }

        [HttpPost("{stableId}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid stableId, [FromBody] AddAnimalRequestDto addAnimalRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var stable = await stableRepository.GetByIdAsync(stableId);
            if (stable == null)
            {
                return NotFound("Stable not found.");
            }

            if (userRole != nameof(UserRole.DatabaseAdmin))
            {
                var farm = stable.Farm;
                var userGuid = Guid.Parse(userId);

                if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
                {
                    return Forbid("You do not have access to this farm.");
                }

                if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
                {
                    return Forbid("You do not have access to this farm.");
                }
            }
            var animalDomain = addAnimalRequestDto.ToAnimalFromCreate(stableId);
            animalDomain = await animalRepository.CreateAsync(animalDomain);

             var animalDto = animalDomain.ToAnimalDto();

            return CreatedAtAction(nameof(GetById), new { id = animalDomain.Id }, animalDto);
        }

        [HttpPut] 
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAnimalRequestDto updateAnimalRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var stable = await stableRepository.GetByIdAsync(updateAnimalRequestDto.StableId);
            if (stable == null)
            {
                return NotFound("Stable not found.");
            }

            var farm = stable.Farm;
            var userGuid = Guid.Parse(userId);

            if (userRole != nameof(UserRole.DatabaseAdmin))
            {
                if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
                {
                    return Forbid("You do not have access to this farm.");
                }
                   
                if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
                {
                    return Forbid("You do not have access to this farm.");
                }
            }

            var animalDomain = await animalRepository.UpdateAsync(id, updateAnimalRequestDto.ToAnimalFromUpdate());

            if (animalDomain == null)
            {
                return NotFound("Animal not found");
            }

            return Ok(animalDomain.ToAnimalDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var animalDomain = await animalRepository.GetByIdAsync(id);
            if (animalDomain == null)
            {
                return NotFound("Animal does not exist");
            }

            var farm = animalDomain.Stable.Farm;
            var userGuid = Guid.Parse(userId);

            if (userRole != nameof(UserRole.DatabaseAdmin))
            {
                if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
                {
                    return Forbid("You do not have access to this farm.");
                }

                if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
                {
                    return Forbid("You do not have access to this farm.");
                }
            
            }
            var deletedAnimal = await animalRepository.DeleteAsync(id);
            return Ok(deletedAnimal.ToAnimalDto());
        }

    }
}
