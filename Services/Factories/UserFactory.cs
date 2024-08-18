using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class UserFactory(UserNotificationFactory userNotificationFactory)
    {
        public User CreateUser(Models.User user)
        {
            return new User
            (
                user.Id,
                user.Name,
                user.Email,
                user.Notifications.Select(userNotificationFactory.CreateNotification).ToArray()
            );
        }

        public Models.User CreateUserModel(User user)
        {
            throw new NotImplementedException();
            //return new Models.User
            //{
            //    Id=user.Id,
            //    Name = user.Name,
            //    Email = user.Email,
            //    Notifications = user.Notifications.Select(userNotificationFactory.CreateNotification).ToList()
            //};
        }
    }
}
