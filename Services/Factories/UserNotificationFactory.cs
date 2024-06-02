using Cooliemint.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class UserNotificationFactory
    {
        public UserNotification CreateNotification(Models.UserNotification notification)
        {
            return new UserNotification
            {
                Id = notification.Id,
                UserId = notification.UserId,
                NotificationId = notification.NotificationId,
                IsActive = notification.IsActive
            };
        }

        public Models.UserNotification CreateNotification(UserNotification notification)
        {
            return new Models.UserNotification
            {
                Id = notification.Id,
                UserId = notification.UserId,
                NotificationId = notification.NotificationId,
                IsActive = notification.IsActive
            };
        }
    }
}
