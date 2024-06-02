using Cooliemint.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class UserFactory(UserNotificationFactory userNotificationFactory)
    {
        public User CreateUser(Models.User user)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Notifications = user.Notifications.Select(userNotificationFactory.CreateNotification).ToList()
            };
        }

        public Models.User CreateUserModel(User user)
        {
            return new Models.User
            {
                Id=user.Id,
                Name = user.Name,
                Email = user.Email,
                Notifications = user.Notifications.Select(userNotificationFactory.CreateNotification).ToList()
            };
        }
    }
}
