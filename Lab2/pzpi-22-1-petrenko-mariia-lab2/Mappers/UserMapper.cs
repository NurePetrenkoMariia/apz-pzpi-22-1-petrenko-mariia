using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userDomain)
        {
            return new UserDto
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                DateOfBirth = userDomain.DateOfBirth,
                PhoneNumber = userDomain.PhoneNumber,
                Email = userDomain.Email,
                PasswordHash = userDomain.PasswordHash,
                Role = userDomain.Role,
                FarmId = userDomain.FarmId,
                AdministeredFarmId = userDomain.AdministeredFarmId,
                Farms = userDomain.Farms?.Select(f => f.ToFarmDto()).ToList(),
                Notifications = userDomain.Notifications?.Select(n => n.ToNotificationDto()).ToList(),
                //UserTasks = userDomain.UserTasks?.Select(ut => ut.ToUserTaskDto()).ToList()
                //Assignments = userDomain.Assignments?.Select(n => n.ToAssignmentDto()).ToList(),
            };
        }

        public static User ToUserFromCreate(this AddUserRequestDto addUserRequestDto)
        {
            return new User
            {
                FirstName = addUserRequestDto.FirstName,
                LastName = addUserRequestDto.LastName,
                DateOfBirth = addUserRequestDto.DateOfBirth,
                PhoneNumber = addUserRequestDto.PhoneNumber,
                Email = addUserRequestDto.Email,
                PasswordHash = addUserRequestDto.PasswordHash,
                Role = addUserRequestDto.Role,
                FarmId = addUserRequestDto.FarmId,
                AdministeredFarmId = addUserRequestDto.AdministeredFarmId,
            };
        }

        public static User ToUserFromUpdate(this UpdateUserRequestDto updateUserRequestDto)
        {
            return new User
            {
                FirstName = updateUserRequestDto.FirstName,
                LastName = updateUserRequestDto.LastName,
                DateOfBirth = updateUserRequestDto.DateOfBirth,
                PhoneNumber = updateUserRequestDto.PhoneNumber,
                Email = updateUserRequestDto.Email,
                PasswordHash = updateUserRequestDto.PasswordHash,
                Role = updateUserRequestDto.Role,
                FarmId = updateUserRequestDto.FarmId,
                AdministeredFarmId = updateUserRequestDto.AdministeredFarmId
            };
        }
    }
}
