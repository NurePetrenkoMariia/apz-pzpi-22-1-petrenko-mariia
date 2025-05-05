using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedMonitoringController : ControllerBase
    {
        private readonly IFeedMonitoringService feedMonitoringService;

        public FeedMonitoringController(IFeedMonitoringService feedMonitoringService)
        {
            this.feedMonitoringService = feedMonitoringService;
        }

        [HttpPost("update-feed-level")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> UpdateFeedLevel([FromBody] FeedUpdateDto feedUpdate)
        {
            var jsonBody = JsonSerializer.Serialize(feedUpdate);
            Console.WriteLine($"Received JSON: {jsonBody}");

            if (feedUpdate == null)
            {
                return BadRequest("Feed update data is required.");
            }

            try
            {
                var feedLevelHistory = new FeedLevelHistory
                {
                    StableId = feedUpdate.StableId,
                    FeedLevel = feedUpdate.CurrentFeedLevel,
                    Timestamp = DateTime.UtcNow,
                    PredictedTimeToEmpty = feedUpdate.PredictedTimeToEmpty
                };
                await feedMonitoringService.MonitorFeedLevelAsync(feedLevelHistory);
                return Ok("Feed level was updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
