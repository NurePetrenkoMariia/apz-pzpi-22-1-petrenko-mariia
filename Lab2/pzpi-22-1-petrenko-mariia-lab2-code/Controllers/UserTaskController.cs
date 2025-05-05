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
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskRepository userTaskRepository;
        private readonly IAssignmentRepository assignmentRepository;
        private readonly IFarmRepository farmRepository;
        public UserTaskController(IUserTaskRepository userTaskRepository, 
            IAssignmentRepository assignmentRepository, 
            IFarmRepository farmRepository)
        {
            this.userTaskRepository = userTaskRepository;
            this.assignmentRepository = assignmentRepository;
            this.farmRepository = farmRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin,Worker")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            IEnumerable<UserTask> userTaskDomain;
            if (role == nameof(UserRole.DatabaseAdmin))
            {
                userTaskDomain = await userTaskRepository.GetAllAsync();
            }
            else if (role == "Owner" || role == "Admin")
            {
         
                var farms = await farmRepository.GetAllAsync();
                var myFarmIds = farms
                    .Where(f => f.OwnerId.ToString() == userId || f.Administrators.Any(a => a.Id.ToString() == userId))
                    .Select(f => f.Id)
                    .ToList();

                var allTasks = await userTaskRepository.GetAllAsync();

                userTaskDomain = allTasks
                    .Where(t => myFarmIds.Contains(t.Assignment.FarmId))
                    .ToList();
            }
            else if (role == "Worker")
            {
                var allTasks = await userTaskRepository.GetAllAsync();
                userTaskDomain = allTasks
                    .Where(t => t.UserId.ToString() == userId)
                    .ToList();
            }
            else
            {
                return Forbid(); 
            }
            var userTaskDto = userTaskDomain.Select(a => a.ToUserTaskDto());
            return Ok(userTaskDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userTaskDomain = await userTaskRepository.GetByIdAsync(id);
            if (userTaskDomain == null)
            {
                return NotFound();
            }

            return Ok(userTaskDomain.ToUserTaskDto());
        }
        [HttpPost]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromBody] AddUserTaskRequestDto addUserTaskRequestDto)
        {
            var userTaskDomain = addUserTaskRequestDto.ToUserTaskFromCreate();

            userTaskDomain = await userTaskRepository.CreateAsync(userTaskDomain);

            var userTaskDto = userTaskDomain.ToUserTaskDto();

            return CreatedAtAction(nameof(GetById), new { id = userTaskDomain.Id }, userTaskDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var userTask = await userTaskRepository.GetByIdAsync(id);
            if (userTask == null)
            {
                return NotFound("UserTask not found.");
            }

            var assignment = await assignmentRepository.GetByIdAsync(userTask.AssignmentId);
            if (assignment == null)
            {
                return NotFound("Associated assignment not found.");
            }

            var farmId = assignment.FarmId;
            var hasAccess = false;

            if (role == nameof(UserRole.DatabaseAdmin))
            {
                hasAccess = true;
            }
            else if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(currentUserId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                hasAccess = farms.Any(f => f.Id == farmId);
            }
            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(currentUserId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                hasAccess = farm?.Id == farmId;
            }

            if (!hasAccess)
            {
                return Forbid("You do not have access to this farm.");
            }

            var deletedUserTask = await userTaskRepository.DeleteAsync(id);
            return Ok("UserTask successfully deleted.");
        }


    }
}
