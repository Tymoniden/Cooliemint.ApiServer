namespace Cooliemint.ApiServer.Models
{
    public class UserNotification
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public required User User { get; set; }

        public required NotificationModel Notification { get; set; }
    }
}
