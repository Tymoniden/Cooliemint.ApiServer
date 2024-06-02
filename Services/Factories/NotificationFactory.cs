namespace Cooliemint.ApiServer.Services.Factories
{
    public class NotificationFactory
    {
        public Shared.Dtos.Notification CreateNotification(Models.Notification notification) 
        {
            return new Shared.Dtos.Notification
            {
                Id = notification.Id,
                Title = notification.Title ?? string.Empty,
                Description = notification.Description ?? string.Empty,
            };
        }
    }
}
