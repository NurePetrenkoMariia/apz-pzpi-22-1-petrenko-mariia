using FarmKeeper.Enums;
using FarmKeeper.Mappers;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using FarmKeeper.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository assignmentRepository;
        private readonly IFarmRepository farmRepository;
        private IUserTaskRepository userTaskRepository;
        public AssignmentsController(IAssignmentRepository assignmentRepository, 
            IFarmRepository farmRepository,
            IUserTaskRepository userTaskRepository)
        {
            this.assignmentRepository = assignmentRepository;
            this.farmRepository = farmRepository;
            this.userTaskRepository = userTaskRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Worker,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);


            List<Assignment> assignmentDomain = new();
            if (userRole == nameof(UserRole.DatabaseAdmin))
            {
                assignmentDomain = await assignmentRepository.GetAllAsync();
            }

            else if (userRole == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                var farmIds = farms.Select(f => f.Id).ToList();

                assignmentDomain = await assignmentRepository.GetAllAsync();
                assignmentDomain = assignmentDomain
                    .Where(a => farmIds.Contains(a.FarmId))
                    .ToList();
            }
            else if (userRole == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);

                if (farm != null)
                {
                    assignmentDomain = await assignmentRepository.GetAllAsync();
                    assignmentDomain = assignmentDomain
                        .Where(a => a.FarmId == farm.Id)
                        .ToList();
                }
            }

            else if (userRole == nameof(UserRole.Worker))
            {
                var workerId = Guid.Parse(userId);

                var userTasks = await userTaskRepository.GetByUserIdAsync(workerId);
                var assignmentIds = userTasks.Select(ut => ut.AssignmentId).ToList();
                assignmentDomain = await assignmentRepository.GetByIdsAsync(assignmentIds);
            }
         
            var assignmentDto = assignmentDomain.Select(a => a.ToAssignmentDto());
            return Ok(assignmentDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Worker,Admin,DatabaseAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var assignmentDomain = await assignmentRepository.GetByIdAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound();
            }

            if (role == nameof(UserRole.DatabaseAdmin))
            {
                return Ok(assignmentDomain.ToAssignmentDto());
            }

            var farmId = assignmentDomain.FarmId;

            if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                var hasAccess = farms.Any(f => f.Id == farmId);

                if (!hasAccess)
                {
                    return Forbid("You do not have access to this assignment.");
                }
            }
            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                if (farm == null || farm.Id != farmId)
                {
                    return Forbid("You do not have access to this assignment.");
                }
            }
            else if (role == nameof(UserRole.Worker))
            {
                var workerId = Guid.Parse(userId);
                var userTasks = await userTaskRepository.GetByUserIdAsync(workerId);
                var isAssigned = userTasks.Any(ut => ut.AssignmentId == assignmentDomain.Id);

                if (!isAssigned)
                {
                    return Forbid("You are not assigned to this task.");
                }
            }
            return Ok(assignmentDomain.ToAssignmentDto());

        }

        [HttpPost]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Create([FromBody] AddAssignmentRequestDto addAssignmentRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var farmId = addAssignmentRequestDto.FarmId;
            var hasAccess = false;

            if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                hasAccess = farms.Any(f => f.Id == farmId);
            }
            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                hasAccess = farm?.Id == farmId;
            }
            else if (role == nameof(UserRole.DatabaseAdmin))
            {
                hasAccess = true;
            }

            if (!hasAccess)
            {
                return Forbid("You do not have access to this farm.");
            }

            var assignmentDomain = addAssignmentRequestDto.ToAssignmentFromCreate();

            assignmentDomain = await assignmentRepository.CreateAsync(assignmentDomain);

            var assignmentDto = assignmentDomain.ToAssignmentDto();

            return CreatedAtAction(nameof(GetById), new { id = assignmentDomain.Id }, assignmentDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var assignmentDomain = await assignmentRepository.GetByIdAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound("Assignment not found.");
            }

            var farmId = assignmentDomain.FarmId;
            var hasAccess = false;

            if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                hasAccess = farms.Any(f => f.Id == farmId);
            }
            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                hasAccess = farm?.Id == farmId;
            }
            else if (role == nameof(UserRole.DatabaseAdmin))
            {
                hasAccess = true;
            }

            if (!hasAccess)
            {
                return Forbid("You do not have access to this farm.");
            }

            var updatedAssignment = updateAssignmentRequestDto.ToAssignmentFromUpdate();
            assignmentDomain = await assignmentRepository.UpdateAsync(id, updatedAssignment);

            return Ok(assignmentDomain.ToAssignmentDto());
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Owner,Admin,DatabaseAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var assignmentDomain = await assignmentRepository.GetByIdAsync(id);
            if (assignmentDomain == null)
            {
                return NotFound("Assignment does not exist.");
            }

            var farmId = assignmentDomain.FarmId;
            var hasAccess = false;

            if (role == nameof(UserRole.Owner))
            {
                var ownerId = Guid.Parse(userId);
                var farms = await farmRepository.GetFarmsByOwnerIdAsync(ownerId);
                hasAccess = farms.Any(f => f.Id == farmId);
            }
            else if (role == nameof(UserRole.Admin))
            {
                var adminId = Guid.Parse(userId);
                var farm = await farmRepository.GetFarmByAdminIdAsync(adminId);
                hasAccess = farm?.Id == farmId;
            }
            else if (role == nameof(UserRole.DatabaseAdmin))
            {
                hasAccess = true;
            }
            if (!hasAccess)
            {
                return Forbid("You do not have access to this farm.");
            }

            var deletedAssignment = await assignmentRepository.DeleteAsync(id);
            return Ok(deletedAssignment.ToAssignmentDto());
        }
    }
}
