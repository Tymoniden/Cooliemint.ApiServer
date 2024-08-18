using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Controllers
{
    public class NotificationDetailsFactory()
    {
        public NotificationDetailsDto CreateNotificationDetailsDto(NotificationDetailsModel notificationDetailsModel)
        {
            return new NotificationDetailsDto(notificationDetailsModel.Id,
                notificationDetailsModel.Title,
                notificationDetailsModel.Description,
                notificationDetailsModel.LongDescription,
                notificationDetailsModel.Url,
                notificationDetailsModel.Icon);
        }
    }
}
