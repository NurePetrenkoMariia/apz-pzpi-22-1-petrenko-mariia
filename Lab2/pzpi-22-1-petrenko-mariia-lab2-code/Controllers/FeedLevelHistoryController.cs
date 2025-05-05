using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedLevelHistoryController : ControllerBase
    {
        private readonly IFeedMonitoringService feedMonitoringService;
        private readonly IStableRepository stableRepository;
        public FeedLevelHistoryController(IFeedMonitoringService feedMonitoringService, IStableRepository stableRepository)
        {
            this.feedMonitoringService = feedMonitoringService;
            this.stableRepository = stableRepository;
        }

        [HttpGet("get-feed-history/{stableId}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetFeedHistory(Guid stableId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userGuid = Guid.Parse(userId);

            var stable = await stableRepository.GetByIdAsync(stableId);
            if (stable == null)
            {
                return NotFound("Stable not found.");
            }

            var farm = stable.Farm;

            if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
            {
                return Forbid("You do not own this farm.");
            }
               
            if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
            {
                return Forbid("You are not an admin of this farm.");
            }

            var history = await feedMonitoringService.GetFeedHistoryAsync(stableId);
            if (history == null || !history.Any())
            {
                return NotFound("No feed history found for the given stable ID.");
            }

            return Ok(history);
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userGuid = Guid.Parse(userId);

            List<FeedLevelHistory> allHistoryDomain = new();
            
            if (userRole == nameof(UserRole.DatabaseAdmin))
            {
                allHistoryDomain = await feedMonitoringService.GetAllAsync();
            }
            else
            {
                var userIdGuid = Guid.Parse(userId);
                if (userRole == nameof(UserRole.Owner))
                {
                    allHistoryDomain = await feedMonitoringService.GetByOwnerIdAsync(userIdGuid);
                }
                else if (userRole == nameof(UserRole.Admin))
                {
                    allHistoryDomain = await feedMonitoringService.GetByAdminIdAsync(userIdGuid);
                }
            }

            var feedLevelHistoryDto = allHistoryDomain.Select(h => h.ToFeedLevelHistoryDto());
            return Ok(feedLevelHistoryDto);
        }

    }
}
