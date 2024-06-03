namespace Cooliemint.Shared.Dtos
{
    public sealed class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<UserNotification> Notifications { get; set; } = [];
    }
}
