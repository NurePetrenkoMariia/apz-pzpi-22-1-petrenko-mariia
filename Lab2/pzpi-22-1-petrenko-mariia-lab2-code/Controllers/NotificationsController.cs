using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IUserRepository userRepository;
        public NotificationsController(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            this.notificationRepository = notificationRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Admin,Worker,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }
            var userIdGuid = Guid.Parse(userId);

            List<Notification> notificationDomain;

            if (userRole == nameof(UserRole.DatabaseAdmin))
            {
                notificationDomain = await notificationRepository.GetAllAsync();
            }
            else
            {
                notificationDomain = await notificationRepository.GetByUserIdAsync(userIdGuid);
            }
            var notificationDto = notificationDomain.Select(n => n.ToNotificationDto());
            return Ok(notificationDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,Worker,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userIdGuid = Guid.Parse(userId);

            var notificationDomain = await notificationRepository.GetByIdAsync(id);
            if (notificationDomain == null)
            {
                return NotFound();
            }

            if (userRole == nameof(UserRole.DatabaseAdmin) || notificationDomain.UserId == userIdGuid)
            {
                return Ok(notificationDomain.ToNotificationDto());
            }

            return Forbid("You do not have access to this notification.");
        }

        [HttpPost("{userId}")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid userId, [FromBody] AddNotificationRequestDto addNotificationRequestDto)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var notificationDomain = addNotificationRequestDto.ToNotificationFromCreate(userId);

            notificationDomain = await notificationRepository.CreateAsync(notificationDomain);

            var notificationDto = notificationDomain.ToNotificationDto();

            return CreatedAtAction(nameof(GetById), new { id = notificationDomain.Id }, notificationDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNotificationRequestDto updateNotificationRequestDto)
        {
            var user = await userRepository.GetByIdAsync(updateNotificationRequestDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var notificationDomain = await notificationRepository.UpdateAsync(id, updateNotificationRequestDto.ToNotificationFromUpdate());
            if (notificationDomain == null)
            {
                return NotFound("Notification not found");
            }
     
            return Ok(notificationDomain.ToNotificationDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var notificationDomain = await notificationRepository.DeleteAsync(id);
            if (notificationDomain == null)
            {
                return NotFound("Notification does not exist");
            }
         
            return Ok(notificationDomain.ToNotificationDto());
        }
    }
}
