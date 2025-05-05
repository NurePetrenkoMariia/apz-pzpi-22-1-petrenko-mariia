using FarmKeeper.Enums;
using FarmKeeper.Models;
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
    public class TaskAssignmentController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IAssignmentRepository assignmentRepository;
        private readonly ITaskAssignmentService taskAssignmentService;
        private readonly IUserTaskRepository userTaskRepository;
        private readonly IFarmRepository farmRepository;
        public TaskAssignmentController(
            IUserRepository userRepository, 
            IAssignmentRepository assignmentRepository,
            ITaskAssignmentService taskAssignmentService,
            IUserTaskRepository userTaskRepository,
            IFarmRepository farmRepository)
        {
            this.userRepository = userRepository;
            this.assignmentRepository = assignmentRepository;
            this.taskAssignmentService = taskAssignmentService;
            this.userTaskRepository = userTaskRepository;
            this.farmRepository = farmRepository;
        }

        [HttpPost("assign-tasks/{farmId:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> AssignTasks([FromRoute] Guid farmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userGuid = Guid.Parse(userId);

            var farm = await farmRepository.GetByIdAsync(farmId);
            if (farm == null)
            {
                return NotFound("Farm not found.");
            }

            if (userRole == nameof(UserRole.Owner) && farm.OwnerId != userGuid)
            {
                return Forbid("You do not have access to this farm.");
            }

            if (userRole == nameof(UserRole.Admin) && !farm.Administrators.Any(a => a.Id == userGuid))
            {
                return Forbid("You do not have access to this farm.");
            }

            var workers = await userRepository.GetWorkersForAssignmentsAsync(farmId);
            var tasks = await assignmentRepository.GetNotStartedAssignmentsAsync(farmId);

            var assignedTasks = await taskAssignmentService.AssignTasks(workers, tasks);

            await userTaskRepository.AddUserTasksAsync(assignedTasks);

            return Ok(assignedTasks);
        }
    }
}
