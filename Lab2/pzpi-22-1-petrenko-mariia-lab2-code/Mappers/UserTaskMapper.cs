using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class UserTaskMapper
    {
        public static UserTaskDto ToUserTaskDto(this UserTask userTaskDomain)
        {
            return new UserTaskDto
            {
                Id = userTaskDomain.Id,
                UserId = userTaskDomain.UserId,
                User = userTaskDomain.User,
                AssignmentId = userTaskDomain.AssignmentId,
                Assignment = userTaskDomain.Assignment,
            };
        }

        public static UserTask ToUserTaskFromCreate(this AddUserTaskRequestDto addUserTaskRequestDto)
        {
            return new UserTask
            {
                UserId = addUserTaskRequestDto.UserId,
                AssignmentId = addUserTaskRequestDto.AssignmentId,
            };
        }
    }
}
