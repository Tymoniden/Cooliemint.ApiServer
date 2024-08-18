using Cooliemint.ApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class UserNotificationRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory)
    {
        public async Task AddUserNotification(int userId, int notificationId, bool isActive, CancellationToken cancellationToken)
        {
            using var ctx = dbContextFactory.CreateDbContext();

            ctx.Add(new UserNotification
            {
                User = await ctx.Users.FirstAsync(u => u.Id == userId),
                Notification = await ctx.Notifications.FirstAsync(n => n.Id == notificationId),
                IsActive = isActive
            });

            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserNotification(int userNotificationId, bool isActive, CancellationToken cancellationToken)
        {
            using var ctx = dbContextFactory.CreateDbContext();

            var userNotification = await ctx.UserNotifications.FirstAsync(un => un.Id == userNotificationId);
            userNotification.IsActive = isActive;
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
