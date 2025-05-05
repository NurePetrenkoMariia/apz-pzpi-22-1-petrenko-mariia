using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        private readonly IFarmRepository farmRepository;

        public UsersController(IUserRepository userRepository, IFarmRepository farmRepository)
        {
            this.userRepository = userRepository;
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            IEnumerable<User> userDomain;

            if (role == nameof(UserRole.DatabaseAdmin))
            {
                userDomain = await userRepository.GetAllAsync();
            }
            else if (role == nameof(UserRole.Owner) && userId != null)
            {
                userDomain = await userRepository.GetUsersByOwnerIdAsync(Guid.Parse(userId));
            }
            else
            {
                return Forbid();
            }
            
            var userDto = userDomain.Select(u => u.ToUserDto());
            return Ok(userDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var userDomain = await userRepository.GetByIdAsync(id);
            if (userDomain == null)
            {
                return NotFound();
            }

            if (role == nameof(UserRole.DatabaseAdmin))
            {
                return Ok(userDomain.ToUserDto());
            }

            else if (role == nameof(UserRole.Owner) && userId != null)
            {
                bool isUserFromOwnersFarm = await userRepository
                    .IsUserFromOwnersFarm(id, Guid.Parse(userId));
                if (isUserFromOwnersFarm)
                {
                    return Ok(userDomain.ToUserDto());
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();

        }

        [HttpPost]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto addUserRequestDto)
        {
            var userDomain = addUserRequestDto.ToUserFromCreate();

            userDomain.PasswordHash = BCrypt.Net.BCrypt.HashPassword(addUserRequestDto.PasswordHash);

            userDomain = await userRepository.CreateAsync(userDomain);

            var userDto = userDomain.ToUserDto();

            return CreatedAtAction(nameof(GetById), new { id = userDomain.Id }, userDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        { 
            var userDomain = await userRepository.UpdateAsync(id, updateUserRequestDto.ToUserFromUpdate());
            if (userDomain == null)
            {
                return NotFound("User not found");
            }
           
            return Ok(userDomain.ToUserDto());
        }

        //дописати логіку для власника
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userDomain = await userRepository.DeleteAsync(id);
            if (userDomain == null)
            {
                return NotFound("User does not exist");
            }

            return Ok(userDomain.ToUserDto());
        }

        //дописати логіку для власника
        [HttpPost("add-admin/{farmId}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> AddAdminToFarm(Guid farmId, [FromBody] AddAdminRequestDto addAdminRequestDto)
        {
            var farm = await farmRepository.GetByIdAsync(farmId);
            if (farm == null)
            {
                return NotFound("Farm not found.");
            }

            var newAdmin = new User
            {
                Id = Guid.NewGuid(),
                FirstName = addAdminRequestDto.FirstName,
                LastName = addAdminRequestDto.LastName,
                DateOfBirth = addAdminRequestDto.DateOfBirth,
                PhoneNumber = addAdminRequestDto.PhoneNumber,
                Email = addAdminRequestDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(addAdminRequestDto.Password),
                Role = UserRole.Admin,
                FarmId = farmId
            };

            farm.Administrators.Add(newAdmin);

            var adminDomain = await userRepository.CreateAsync(newAdmin);

            return CreatedAtAction(nameof(GetById), new { id = newAdmin.Id }, newAdmin.ToUserDto());

        }
    }
}
