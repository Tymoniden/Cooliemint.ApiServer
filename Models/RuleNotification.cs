namespace Cooliemint.ApiServer.Models
{
    public class RuleNotification
    {
        public int Id { get; set; }
        public virtual required Rule Rule { get; set; }
        public virtual required Notification Notification { get; set; }
    }
}
