namespace Cooliemint.ApiServer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; } = [];

    }
}
