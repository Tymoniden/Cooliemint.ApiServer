using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class UserNotificationFactory
    {
        public UserNotification CreateNotification(Models.UserNotification notification)
        {
            return new UserNotification(notification.Id, notification.User.Id, notification.Notification.Id, notification.IsActive);
        }

        public Models.UserNotification CreateNotification(UserNotification notification)
        {
            throw new NotImplementedException();
            //return new Models.UserNotification
            //{
            //    Id = notification.Id,
            //    User = notification.User,
            //    NotificationId = notification.Notification,
            //    IsActive = notification.IsActive
            //};
        }
    }
}
