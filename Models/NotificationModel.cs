namespace Cooliemint.ApiServer.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; } = [];

        public ICollection<NotificationDetailsModel> Details { get; set; } = [];

        public ICollection<RuleNotification> RuleNotifications { get; set; } = [];
    }
}
