namespace Cooliemint.ApiServer.Models
{
    public class NotificationDetails
    {
        public int Id { get; set; }
        public required Notification Notification { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? LongDescription { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; } // TODO: own type?
    }
}