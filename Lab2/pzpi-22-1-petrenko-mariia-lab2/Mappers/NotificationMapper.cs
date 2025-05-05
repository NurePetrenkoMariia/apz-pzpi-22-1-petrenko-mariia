using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDto ToNotificationDto(this Notification notificationDomain)
        {
            return new NotificationDto
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Text = notificationDomain.Text,
                DateTimeCreated = notificationDomain.DateTimeCreated,
                UserId = notificationDomain.UserId,
            };
        }

        public static Notification ToNotificationFromCreate(this AddNotificationRequestDto addNotificationRequestDto, Guid userId)
        {
            return new Notification
            {
                Title = addNotificationRequestDto.Title,
                Text = addNotificationRequestDto.Text,
                UserId = userId,
            };
        }

        public static Notification ToNotificationFromUpdate(this UpdateNotificationRequestDto updateNotificationRequestDto)
        {
            return new Notification
            {
                Title = updateNotificationRequestDto.Title,
                Text = updateNotificationRequestDto.Text,
                UserId = updateNotificationRequestDto.UserId,
            };
        }
    }
}
