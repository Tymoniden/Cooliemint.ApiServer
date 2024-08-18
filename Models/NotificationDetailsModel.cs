namespace Cooliemint.ApiServer.Models
{
    public class NotificationDetailsModel
    {
        public int Id { get; set; }
        public required NotificationModel Notification { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? LongDescription { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; } // TODO: own type?
    }
}