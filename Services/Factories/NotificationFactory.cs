using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class NotificationFactory
    {
        public NotificationDto CreateNotification(NotificationModel notification)
        {
            return new NotificationDto
            (
                notification.Id,
                notification.Title ?? string.Empty,
                notification.Description ?? string.Empty
            );
        }
    }
}
