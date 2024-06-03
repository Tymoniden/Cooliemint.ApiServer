namespace Cooliemint.Shared.Dtos
{
    public sealed class UserNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsActive { get; set; }
    }
}
