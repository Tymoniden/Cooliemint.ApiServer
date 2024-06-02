using Cooliemint.ApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class UserNotificationRepository(Models.CooliemintDbContext dbContext)
    {
        public async Task AddUserNotification(int userId, int notificationId, bool isActive, CancellationToken cancellationToken)
        {
            dbContext.UserNotifications.Add(new UserNotification { UserId = userId, NotificationId = notificationId, IsActive = isActive });
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserNotification(int userNotificationId, bool isActive, CancellationToken cancellationToken)
        {
            var userNotification = await dbContext.UserNotifications.FirstAsync(un => un.Id == userNotificationId);
            userNotification.IsActive = isActive;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
