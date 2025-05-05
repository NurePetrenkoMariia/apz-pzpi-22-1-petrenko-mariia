using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class AssignmentMapper
    {
        public static AssignmentDto ToAssignmentDto(this Assignment assignmentDomain)
        {
            return new AssignmentDto
            {
                Id = assignmentDomain.Id,
                Name = assignmentDomain.Name,
                Description = assignmentDomain.Description,
                NumberOfParticipants = assignmentDomain.NumberOfParticipants,
                Status = assignmentDomain.Status,
                Priority = assignmentDomain.Priority,
                FarmId = assignmentDomain.FarmId,
            };
        }

        public static Assignment ToAssignmentFromCreate(this AddAssignmentRequestDto addAssignmentRequestDto)
        {
            return new Assignment
            {
                Name = addAssignmentRequestDto.Name,
                Description = addAssignmentRequestDto.Description,
                NumberOfParticipants = addAssignmentRequestDto.NumberOfParticipants,
                Status = addAssignmentRequestDto.Status,
                Priority = addAssignmentRequestDto.Priority,
                FarmId = addAssignmentRequestDto.FarmId,
            };
        }

        public static Assignment ToAssignmentFromUpdate(this UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            return new Assignment
            {
                Name = updateAssignmentRequestDto.Name,
                Description = updateAssignmentRequestDto.Description,
                NumberOfParticipants = updateAssignmentRequestDto.NumberOfParticipants,
                Status = updateAssignmentRequestDto.Status,
                Priority = updateAssignmentRequestDto.Priority,
                FarmId = updateAssignmentRequestDto.FarmId,
            };
        }
    }
}
